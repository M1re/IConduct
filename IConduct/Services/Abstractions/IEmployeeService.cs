using IConduct.Models;

namespace IConduct.Services.Abstractions
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task<bool> EnableEmployeeAsync(int employeeId, bool enable);
    }
}