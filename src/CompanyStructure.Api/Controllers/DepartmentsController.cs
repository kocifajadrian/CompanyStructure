using CompanyStructure.Application.Features.Departments.Dtos;
using CompanyStructure.Application.Features.Departments;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("by-project/{projectId:guid}")]
        public async Task<ActionResult<List<DepartmentResponse>>> GetAllByProjectId(Guid projectId)
        {
            var departments = await _departmentService.GetAllByProjectIdAsync(projectId);
            return Ok(departments);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DepartmentResponse>> GetById(Guid id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentResponse>> Create([FromBody] CreateDepartmentRequest request)
        {
            var department = await _departmentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request)
        {
            await _departmentService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
