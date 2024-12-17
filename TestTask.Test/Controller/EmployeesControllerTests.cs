
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Controllers;
using TestTask.Entities;
using TestTask.Interfaces;
using Xunit.Abstractions;

namespace TestTask.Test.Controller;

public class EmployeesControllerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private EmployeesController _employeesController;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        //Dependency Injection
        _employeeRepository = A.Fake<IEmployeeRepository>();
        //Sut
        _employeesController = new EmployeesController(_employeeRepository);
    }
    [Fact]
    public void EmployeeController_List_ReturnSuccess()
    {
        //Arrange
        var employees = A.Fake<List<Employee>>();
        A.CallTo(() => _employeeRepository.GetAll()).Returns(employees);
        //Act
        var result = _employeesController.List();
        //Assert
        result.Should().BeOfType<Task<IActionResult>>();
    }
    [Fact]
    public async Task Import_ValidFile_ReturnsImportResultViewWithProcessedCounts()
    {
        // Arrange: Prepare a sample CSV file content
        var csvContent = new StringBuilder();
        csvContent.AppendLine("Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records");
        csvContent.AppendLine("1234,John,Doe,05/11/1974,1234567890,0987654321,123 Street,12345,john.doe@example.com,05/11/1974");
        csvContent.AppendLine("5678,Jane,Smith,05/11/1974,1231231234,0980980987,456 Avenue,67890,jane.smith@example.com,05/11/1974");

        var fileName = "dataset.csv";
        var content = Encoding.UTF8.GetBytes(csvContent.ToString());
        var stream = new MemoryStream(content);
        var formFile = new FormFile(stream, 0, stream.Length, "file", fileName);

        // Act
        var result = await _employeesController.Import(formFile);

        // Assert
        if (result is BadRequestObjectResult badRequest)
        {
            // Print the error message for debugging
            _testOutputHelper.WriteLine($"Error: {badRequest.Value}");
        }

        var viewResult = result.Should().BeOfType<IActionResult>().Subject;
        viewResult.Should().Be("ImportResult");

        // Verify ViewData contains correct counts
        _employeesController.ViewData["ProcessedCount"].Should().Be(2);
        _employeesController.ViewData["FailedCount"].Should().Be(0);
    }
}