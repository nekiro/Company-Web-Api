using PumoxTestApp.Dtos;
using PumoxTestApp.Models;
using System.Threading.Tasks;

namespace PumoxTestApp.Services
{
    public interface ICompanyService
    {
        public Task<ServiceResponse<object>> CreateNewCompany(CompanyInputDto companyDto);
        public Task<ServiceResponse<object>> SearchCompanies(SearchCompanyDto searchDto);
        public Task<ServiceResponse<object>> UpdateCompany(long id, CompanyInputDto companyDto);
        public Task<ServiceResponse<object>> DeleteCompany(long id);
    }
}
