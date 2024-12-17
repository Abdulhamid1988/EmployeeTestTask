using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestTask.AppContext;
using TestTask.Entities;
using TestTask.Repository;

namespace TestTask.Test.Repository;

public class EmployeeRepositoryTest
{
    private async Task<ApplicationDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new ApplicationDbContext(options);
        await databaseContext.Database.EnsureCreatedAsync();
        if (await databaseContext.Employees.CountAsync() >= 0) return databaseContext;
        for (var i = 0; i < 10; i++)
        {
            databaseContext.Employees.Add(
                new Employee()
                {
                    Surname = "Surname" + i,
                    ForeNames = "ForeNames" + i,
                    PayrollNumber = "PayrollNumber" + i,
                    DateOfBirth = DateTime.Now,
                    Address = "Address" + i,
                    Postcode = "Postcode" + i,
                    Email = "Email" + i,
                    StartDate = DateTime.Now
                });
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }
    [Fact]
    public async void EmployeeRepository_GetIdAsync_ReturnsEmployee()
    {
        //Arrange
        var id = 1;
        var dbContext = await GetDbContextAsync();
        var employeeRepository = new EmployeeRepository(dbContext);

        //Act
        var result = employeeRepository.GetId(id);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Task<Employee>>();
    }

    [Fact]
    public async void ClubRepository_GetAll_ReturnsList()
    {
        //Arrange
        var dbContext = await GetDbContextAsync();
        var employeeRepository = new EmployeeRepository(dbContext);

        //Act
        var result = await employeeRepository.GetAll();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Employee>>();
    }
}