using Microsoft.EntityFrameworkCore;
using RedisCacheDemo.Database;

namespace RedisCacheDemo.Services;

public class EmployeeRepository(MainContext context)
{
    private readonly MainContext _context = context;

    public async Task CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
                             .Where(r => r.EmployeeId == id)
                             .FirstOrDefaultAsync();
    }

    public async Task DeleteEmployeeAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}
