using Microsoft.EntityFrameworkCore;
using TestTask.AppContext;
using TestTask.Entities;
using TestTask.Interfaces;

namespace TestTask.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Employee> GetId(int employeeId)
    {
        var currentEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
        if(currentEmployee == null)
            throw new Exception("Employee not found");
        return currentEmployee;
    }
    public async Task<List<Employee>> GetAll()
    {
        return await _dbContext.Employees.ToListAsync(); 
    }
    public async Task Update(Employee employee)
    {
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync();
    }
    public async Task AddRangeAsync(IEnumerable<Employee> employees)
    {
        await _dbContext.Employees.AddRangeAsync(employees);
        await _dbContext.SaveChangesAsync();
    }
}