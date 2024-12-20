using CsvHelper.Configuration;
using TestTask.Converter;
using TestTask.Entities;

namespace TestTask.Mappers;

public class EmployeeMap : ClassMap<Employee>
{
    public EmployeeMap()
    {
        Map(m => m.PayrollNumber).Name("Personnel_Records.Payroll_Number");
        Map(m => m.ForeNames).Name("Personnel_Records.Forenames");
        Map(m => m.Surname).Name("Personnel_Records.Surname");
        Map(m => m.DateOfBirth).Name("Personnel_Records.Date_of_Birth").TypeConverter<CustomDateTimeConverter>();
        Map(m => m.Telephone).Name("Personnel_Records.Telephone");
        Map(m => m.Mobile).Name("Personnel_Records.Mobile");
        Map(m => m.Address).Name("Personnel_Records.Address");
        Map(m => m.Postcode).Name("Personnel_Records.Postcode");
        Map(m => m.Email).Name("Personnel_Records.EMail_Home");
        Map(m => m.StartDate).Name("Personnel_Records.Start_Date").TypeConverter<CustomDateTimeConverter>();
    }
}