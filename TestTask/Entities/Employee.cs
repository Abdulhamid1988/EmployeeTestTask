namespace TestTask.Entities;

public class Employee
{
    public int Id { get; set; }
    public required string  PayrollNumber { get; set; }
    public required string ForeNames { get; set; }
    public required string Surname { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public string? Telephone { get; set; }
    public string? Mobile { get; set; }
    public required string Address { get; set; }
    public required string Postcode { get; set; }
    public required string Email { get; set; }
    public required DateTime StartDate { get; set; }
}