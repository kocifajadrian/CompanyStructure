using CompanyStructure.Application.Features.Projects.Dtos;
using CompanyStructure.Application.Features.Projects;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("by-division/{divisionId:guid}")]
        public async Task<ActionResult<List<ProjectResponse>>> GetAllByDivisionId(Guid divisionId)
        {
            var projects = await _projectService.GetAllByDivisionIdAsync(divisionId);
            return Ok(projects);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> Create([FromBody] CreateProjectRequest request)
        {
            var project = await _projectService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
        {
            await _projectService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }
    }
}
