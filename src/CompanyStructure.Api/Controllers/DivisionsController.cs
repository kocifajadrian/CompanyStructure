using CompanyStructure.Application.Features.Divisions.Dtos;
using CompanyStructure.Application.Features.Divisions;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    /// <summary>
    /// Manages divisions within company.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DivisionsController : ControllerBase
    {
        private readonly IDivisionService _divisionService;

        public DivisionsController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        /// <summary>
        /// Get all divisions of a company
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <response code="200">List of divisions.</response>
        /// <response code="404">Company not found.</response>
        [HttpGet("by-company/{companyId:guid}")]
        [ProducesResponseType(typeof(List<DivisionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DivisionResponse>>> GetAllByCompanyId(Guid companyId)
        {
            var divisions = await _divisionService.GetAllByCompanyIdAsync(companyId);
            return Ok(divisions);
        }

        /// <summary>
        /// Get a division
        /// </summary>
        /// <param name="id">The division identifier.</param>
        /// <response code="200">Division details.</response>
        /// <response code="404">Division not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DivisionResponse>> GetById(Guid id)
        {
            var division = await _divisionService.GetByIdAsync(id);
            return Ok(division);
        }

        /// <summary>
        /// Create a division
        /// </summary>
        /// <param name="request">The division data.</param>
        /// <response code="201">Division created.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Company or manager not found.</response>
        /// <response code="409">Manager does not belong to the same company as the division.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DivisionResponse>> Create([FromBody] CreateDivisionRequest request)
        {
            var division = await _divisionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = division.Id }, division);
        }

        /// <summary>
        /// Update a division
        /// </summary>
        /// <param name="id">The division identifier.</param>
        /// <param name="request">The new division data.</param>
        /// <response code="204">Division updated.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Division or manager not found.</response>
        /// <response code="409">Manager does not belong to the same company as the division.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDivisionRequest request)
        {
            await _divisionService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete a division
        /// </summary>
        /// <param name="id">The division identifier.</param>
        /// <response code="204">Division deleted.</response>
        /// <response code="404">Division not found.</response>
        /// <response code="409">Division has existing projects.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _divisionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
