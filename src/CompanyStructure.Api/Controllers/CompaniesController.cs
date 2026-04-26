using CompanyStructure.Application.Features.Companies.Dtos;
using CompanyStructure.Application.Features.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyResponse>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CompanyResponse>> GetById(Guid id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return Ok(company);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyResponse>> Create([FromBody] CreateCompanyRequest request)
        {
            var company = await _companyService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyRequest request)
        {
            await _companyService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _companyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
