using CompanyStructure.Application.Features.Companies.Dtos;
using CompanyStructure.Application.Features.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    /// <summary>
    /// Manages companies.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Get all companies
        /// </summary>
        /// <response code="200">List of companies.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CompanyResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CompanyResponse>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }

        /// <summary>
        /// Get a company
        /// </summary>
        /// <param name="id">The company identifier.</param>
        /// <response code="200">Company details.</response>
        /// <response code="404">Company not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompanyResponse>> GetById(Guid id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return Ok(company);
        }

        /// <summary>
        /// Create a company
        /// </summary>
        /// <param name="request">The company data.</param>
        /// <response code="201">Company created.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Manager not found.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompanyResponse>> Create([FromBody] CreateCompanyRequest request)
        {
            var company = await _companyService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
        }

        /// <summary>
        /// Update a company
        /// </summary>
        /// <param name="id">The company identifier.</param>
        /// <param name="request">The new company data.</param>
        /// <response code="204">Company updated.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="404">Company or manager not found.</response>
        /// <response code="409">Manager does not belong to this company.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyRequest request)
        {
            await _companyService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete a company
        /// </summary>
        /// <param name="id">The company identifier.</param>
        /// <response code="204">Company deleted.</response>
        /// <response code="404">Company not found.</response>
        /// <response code="409">Company has existing divisions or employees.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _companyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
