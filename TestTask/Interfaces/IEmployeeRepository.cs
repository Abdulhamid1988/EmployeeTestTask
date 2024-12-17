using TestTask.Entities;

namespace TestTask.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> GetId(int employeeId);
    Task<List<Employee>> GetAll();
    Task Update(Employee employee);
    Task AddRangeAsync(IEnumerable<Employee> employees);
}