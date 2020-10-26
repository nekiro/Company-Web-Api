using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CompanyApp.Dtos;
using CompanyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<object>> CreateNewCompany(CompanyInputDto companyDto)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
            try
            {
                EntityEntry<Company> newCompany = await _context.Companies.AddAsync(new Company
                {
                    Name = companyDto.Name,
                    EstablishmentYear = companyDto.EstablishmentYear.Value
                });

                if (companyDto.Employees != null)
                {
                    foreach (var dtoEmployee in companyDto.Employees)
                    {
                        _context.Employees.Add(new Employee
                        {
                            FirstName = dtoEmployee.FirstName,
                            LastName = dtoEmployee.LastName,
                            DateOfBirth = dtoEmployee.DateOfBirth.Value,
                            JobTitle = dtoEmployee.JobTitle.Value,
                            Company = newCompany.Entity
                        });
                    }
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new { Id = newCompany.Entity.CompanyID };
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Exception: {ex.Message}, InnerException: {ex.InnerException.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteCompany(long id)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
            Company company = _context.Companies.Where(c => c.CompanyID == id).FirstOrDefault();
            if (company == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Company with given id does not exist.";
                return serviceResponse;
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> SearchCompanies(SearchCompanyDto searchDto)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();

            List<Company> filteredCompanies = null;

            bool isKeywordValid = !string.IsNullOrEmpty(searchDto.Keyword);

            if (!isKeywordValid && searchDto.EmployeeDateOfBirthFrom == null && searchDto.EmployeeDateOfBirthTo == null && searchDto.EmployeesJobTitles == null)
            {
                filteredCompanies = new List<Company>();
            }
            else
            {
                filteredCompanies = await (from company in _context.Set<Company>()
                            join employee in _context.Set<Employee>()
                            on company equals employee.Company
                            where EF.Functions.Like(company.Name, searchDto.Keyword) ||
                            EF.Functions.Like(employee.FirstName, searchDto.Keyword) ||
                            EF.Functions.Like(employee.LastName, searchDto.Keyword) ||
                            searchDto.EmployeeDateOfBirthFrom != null && searchDto.EmployeeDateOfBirthTo != null && employee.DateOfBirth >= searchDto.EmployeeDateOfBirthFrom && employee.DateOfBirth <= searchDto.EmployeeDateOfBirthTo ||
                            searchDto.EmployeesJobTitles != null && searchDto.EmployeesJobTitles.Contains(employee.JobTitle)
                            select company).ToListAsync();
            }

            serviceResponse.Data = new { Results = filteredCompanies };
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateCompany(long id, CompanyInputDto companyDto)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
            try
            {
                Company company = _context.Companies.Where(c => c.CompanyID == id).FirstOrDefault();
                if (company == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "There is no company with that company id.";
                    return serviceResponse;
                }

                company.Name = companyDto.Name;
                company.EstablishmentYear = companyDto.EstablishmentYear.Value;

                _context.Employees.RemoveRange(_context.Employees.Where(e => e.Company == company));

                _context.Employees.AddRange(
                    (from employee in companyDto.Employees
                    select new Employee()
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        DateOfBirth = employee.DateOfBirth.Value,
                        JobTitle = employee.JobTitle.Value,
                        Company = company,
                    }).ToList()
                 );

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Exception: {ex.Message}";
                if (ex.InnerException != null)
                {
                    serviceResponse.Message += $" InnerException: {ex.InnerException.Message}";
                }
            }

            return serviceResponse;
        }
    }
}
