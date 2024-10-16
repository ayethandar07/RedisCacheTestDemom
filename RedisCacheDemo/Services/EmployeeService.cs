using Microsoft.Extensions.Caching.Memory;
using RedisCacheDemo.Database;
using RedisCacheDemo.Services.Caching;

namespace RedisCacheDemo.Services;

public class EmployeeService(EmployeeRepository repository)
{
    private readonly EmployeeRepository _repository = repository;

    public async Task<Employee> CreateEmployeeAsync(Employee request)
    {
        await _repository.CreateEmployeeAsync(request);
        return request;
    }

    public async Task<IEnumerable<Employee>?> GetEmployeesAsync()
    {
        var employees = await _repository.GetEmployeesAsync();
        return employees;
    }

    public async Task<List<Employee>?> GetEmployeesNoCacheAsync()
    {
        var employees = await _repository.GetEmployeesAsync();
        return employees;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _repository.GetEmployeeByIdAsync(id);         
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        if (employee != null) 
        { 
            await _repository.DeleteEmployeeAsync(employee);
            return true;
        }
        return false;
    }
}