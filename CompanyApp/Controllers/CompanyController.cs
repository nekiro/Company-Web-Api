using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyApp.Dtos;
using CompanyApp.Models;
using CompanyApp.Services;
using System.Threading.Tasks;

namespace CompanyApp.Controllers
{
    [ApiController]
    [Route("company")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost()]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewCompany(CompanyInputDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceResponse<object> response = await _companyService.CreateNewCompany(companyDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost()]
        [Route("search")]
        [Authorize]
        public async Task<IActionResult> SearchCompany(SearchCompanyDto searchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceResponse<object> response = await _companyService.SearchCompanies(searchDto);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCompany(long id, CompanyInputDto companyInputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceResponse<object> response = await _companyService.UpdateCompany(id, companyInputDto);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompany(long id)
        {
            ServiceResponse<object> response = await _companyService.DeleteCompany(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok();
        }
    }
}
