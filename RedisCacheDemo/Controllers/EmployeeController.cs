using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisCacheDemo.Database;
using RedisCacheDemo.Services;
using RedisCacheDemo.Services.Caching;

namespace RedisCacheDemo.Controllers;

[Route("api/")]
[ApiController]
public class EmployeeController(EmployeeService service, IRedisCacheService cache) : ControllerBase
{
    private readonly EmployeeService _service = service;
    private readonly IRedisCacheService _cache = cache;

    [HttpPost("employees")]
    public async Task<IActionResult> CreateEmployee(Employee request)
    {
        var employee = await _service.CreateEmployeeAsync(request);
        return Ok(employee);
    }

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployeesWithCache()
    {
        var userId = Request.Headers["UserId"];
        var cachingKey = $"employees_{userId}";

        var employees = _cache.GetData<IEnumerable<Employee>>("cachingKey");
        if (employees is not null)
        {
            return Ok(employees);
        }

        employees = await _service.GetEmployeesAsync();
        _cache.SetData("cachingKey", employees);

        return Ok(employees);
    
    }

    [HttpGet("employees1")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _service.GetEmployeesNoCacheAsync();
        return Ok(employees);
    }

    [HttpGet("employee/{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employees = await _service.GetEmployeeByIdAsync(id);
        return Ok(employees);
    }

    [HttpDelete("employee/{id}")]
    public async Task<IActionResult> DeleteEmployeeById(int id)
    {
        var res = await _service.DeleteEmployeeAsync(id);
        return Ok(res);
    }
}