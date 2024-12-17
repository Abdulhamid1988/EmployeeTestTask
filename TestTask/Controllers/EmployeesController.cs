using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using TestTask.Entities;
using TestTask.Interfaces;
using TestTask.Mappers;
namespace TestTask.Controllers;
public class EmployeesController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var employees = await _employeeRepository.GetAll();
        return View(employees);
    }
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file)
    {
        var employees = new List<Employee>();
        var formats = new[] { "dd/MM/yyyy", "MM/dd/yyyy", "dd/MM/yyyy HH:mm:ss", "MM/dd/yyyy HH:mm:ss" };
        int processedCount = 0;
        int failedCount = 0;
        try
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                   {
                       HeaderValidated = null, // Ignore unexpected headers
                       MissingFieldFound = null, // Ignore missing fields
                   }))
            {
                csv.Context.RegisterClassMap<EmployeeMap>();
                var records = csv.GetRecords<Employee>();

                foreach (var record in records)
                {
                    try
                    {
                        var employee = new Employee
                        {
                            PayrollNumber = record.PayrollNumber,
                            ForeNames = record.ForeNames,
                            Surname = record.Surname,
                            DateOfBirth = ParseDate(record.DateOfBirth.ToString(CultureInfo.InvariantCulture), formats),
                            Telephone = record.Telephone,
                            Mobile = record.Mobile,
                            Address = record.Address,
                            Postcode = record.Postcode,
                            Email = record.Email,
                            StartDate = ParseDate(record.StartDate.ToString(CultureInfo.InvariantCulture), formats)
                        };

                        employees.Add(employee);
                        processedCount++;
                    }
                    catch (Exception)
                    {
                        failedCount++;
                    }
                }
            }
            await _employeeRepository.AddRangeAsync(employees);
            ViewData["ProcessedCount"] = processedCount;
            ViewData["FailedCount"] = failedCount;
            return View("ImportResult");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error processing the file: {ex.Message}");
        }
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeRepository.GetId(id);
        return View(employee);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            await _employeeRepository.Update(employee);
            return RedirectToAction("List");
        }

        return View(employee);
    }
    private DateTime ParseDate(string dateStr, string[] formats)
    {
        if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime parsedDate))
        {
            return parsedDate;
        }

        throw new FormatException($"Invalid date format: {dateStr}");
    }
}