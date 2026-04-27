using CompanyStructure.Application.Features.Departments.Dtos;
using CompanyStructure.Application.Features.Departments;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    /// <summary>
    /// Manages departments within company project.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments of a project
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <response code="200">List of departments.</response>
        /// <response code="404">Project not found.</response>
        [HttpGet("by-project/{projectId:guid}")]
        [ProducesResponseType(typeof(List<DepartmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DepartmentResponse>>> GetAllByProjectId(Guid projectId)
        {
            var departments = await _departmentService.GetAllByProjectIdAsync(projectId);
            return Ok(departments);
        }

        /// <summary>
        /// Get a department
        /// </summary>
        /// <param name="id">The department identifier.</param>
        /// <response code="200">Department details.</response>
        /// <response code="404">Department not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentResponse>> GetById(Guid id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            return Ok(department);
        }

        /// <summary>
        /// Create a department
        /// </summary>
        /// <param name="request">The department data.</param>
        /// <response code="201">Department created.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Project or manager not found.</response>
        /// <response code="409">Manager does not belong to the same company as the department.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DepartmentResponse>> Create([FromBody] CreateDepartmentRequest request)
        {
            var department = await _departmentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        /// <summary>
        /// Update a department
        /// </summary>
        /// <param name="id">The department identifier.</param>
        /// <param name="request">The new department data.</param>
        /// <response code="204">Department updated.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Department or manager not found.</response>
        /// <response code="409">Manager does not belong to the same company as the department.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request)
        {
            await _departmentService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete a department
        /// </summary>
        /// <param name="id">The department identifier.</param>
        /// <response code="204">Department deleted.</response>
        /// <response code="404">Department not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
