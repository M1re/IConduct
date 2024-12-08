using IConduct.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace IConduct.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost("{id}/enable")]
        public async Task<IActionResult> EnableEmployee(int id, [FromBody] bool enable)
        {
            var result = await _employeeService.EnableEmployeeAsync(id, enable);
            return result ? Ok() : NotFound();
        }
    }
}