using CompanyStructure.Application.Features.Employees.Dtos;
using CompanyStructure.Application.Features.Employees;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("by-company/{companyId:guid}")]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAllByCompanyId(Guid companyId)
        {
            var employees = await _employeeService.GetAllByCompanyIdAsync(companyId);
            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
        {
            var employee = await _employeeService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
        {
            await _employeeService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
