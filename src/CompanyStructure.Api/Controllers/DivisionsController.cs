using CompanyStructure.Application.Features.Divisions.Dtos;
using CompanyStructure.Application.Features.Divisions;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DivisionsController : ControllerBase
    {
        private readonly IDivisionService _divisionService;

        public DivisionsController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        [HttpGet("by-company/{companyId:guid}")]
        public async Task<ActionResult<List<DivisionResponse>>> GetAllByCompanyId(Guid companyId)
        {
            var divisions = await _divisionService.GetAllByCompanyIdAsync(companyId);
            return Ok(divisions);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DivisionResponse>> GetById(Guid id)
        {
            var division = await _divisionService.GetByIdAsync(id);
            return Ok(division);
        }

        [HttpPost]
        public async Task<ActionResult<DivisionResponse>> Create([FromBody] CreateDivisionRequest request)
        {
            var division = await _divisionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = division.Id }, division);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDivisionRequest request)
        {
            await _divisionService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _divisionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
