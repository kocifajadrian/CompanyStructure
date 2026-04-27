using CompanyStructure.Application.Features.Projects.Dtos;
using CompanyStructure.Application.Features.Projects;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    /// <summary>
    /// Manages projects within company division.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get all projects of a division
        /// </summary>
        /// <param name="divisionId">The division identifier.</param>
        /// <response code="200">List of projects.</response>
        /// <response code="404">Division not found.</response>
        [HttpGet("by-division/{divisionId:guid}")]
        [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProjectResponse>>> GetAllByDivisionId(Guid divisionId)
        {
            var projects = await _projectService.GetAllByDivisionIdAsync(divisionId);
            return Ok(projects);
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <response code="200">Project details.</response>
        /// <response code="404">Project not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            return Ok(project);
        }

        /// <summary>
        /// Create a project
        /// </summary>
        /// <param name="request">The project data.</param>
        /// <response code="201">Project created.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Division or manager not found.</response>
        /// <response code="409">Manager does not belong to the same company as the project.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProjectResponse>> Create([FromBody] CreateProjectRequest request)
        {
            var project = await _projectService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <param name="request">The new project data.</param>
        /// <response code="204">Project updated.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Project or manager not found.</response>
        /// <response code="409">Manager does not belong to the same company as the project.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
        {
            await _projectService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <response code="204">Project deleted.</response>
        /// <response code="404">Project not found.</response>
        /// <response code="409">Project has existing departments.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }
    }
}
