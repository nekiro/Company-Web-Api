using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PumoxTestApp.Dtos;
using PumoxTestApp.Models;
using PumoxTestApp.Services;

namespace PumoxTestApp.Controllers
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
        public async Task<IActionResult> CreateNewCompany([FromBody] JObject body)
        {
            CompanyInputDto companyDto = new CompanyInputDto();
            (bool Success, string Error) = await Task.Run(() => companyDto.ParseJsonData(body));
            if (!Success)
            {
                // deserialization failed
                return BadRequest(Error);
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
        public async Task<IActionResult> SearchCompany([FromBody] JObject body)
        {
            SearchCompanyDto searchDto = new SearchCompanyDto();
            await Task.Run(() => searchDto.ParseJsonData(body));

            ServiceResponse<ICollection<Company>> response = await _companyService.SearchCompanies(searchDto);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCompany(long id, [FromBody] JObject body)
        {
            CompanyInputDto companyDto = new CompanyInputDto();
            (bool Success, string Error) = await Task.Run(() => companyDto.ParseJsonData(body));
            if (!Success)
            {
                // deserialization failed
                return BadRequest(Error);
            }

            ServiceResponse<object> response = await _companyService.UpdateCompany(id, companyDto);
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
