using CompanyStructure.Application.Features.Employees.Dtos;
using CompanyStructure.Application.Features.Employees;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    /// <summary>
    /// Manages employees within company.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees of a company
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <response code="200">List of employees.</response>
        /// <response code="404">Company not found.</response>
        [HttpGet("by-company/{companyId:guid}")]
        [ProducesResponseType(typeof(List<EmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAllByCompanyId(Guid companyId)
        {
            var employees = await _employeeService.GetAllByCompanyIdAsync(companyId);
            return Ok(employees);
        }

        /// <summary>
        /// Get an employee
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <response code="200">Employee details.</response>
        /// <response code="404">Employee not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> GetById(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee);
        }

        /// <summary>
        /// Create an employee
        /// </summary>
        /// <param name="request">The employee data.</param>
        /// <response code="201">Employee created.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Company not found.</response>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
        {
            var employee = await _employeeService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <param name="request">The new employee data.</param>
        /// <response code="204">Employee updated.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Employee not found.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
        {
            await _employeeService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <response code="204">Employee deleted.</response>
        /// <response code="404">Employee not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
