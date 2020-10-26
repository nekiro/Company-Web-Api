using CompanyApp.Dtos;
using CompanyApp.Models;
using System.Threading.Tasks;

namespace CompanyApp.Services
{
    public interface ICompanyService
    {
        public Task<ServiceResponse<object>> CreateNewCompany(CompanyInputDto companyDto);
        public Task<ServiceResponse<object>> SearchCompanies(SearchCompanyDto searchDto);
        public Task<ServiceResponse<object>> UpdateCompany(long id, CompanyInputDto companyDto);
        public Task<ServiceResponse<object>> DeleteCompany(long id);
    }
}
