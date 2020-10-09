using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PumoxTestApp.Dtos;
using PumoxTestApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTestApp.Services
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
                    EstablishmentYear = companyDto.EstablishmentYear
                });

                foreach (var dtoEmployee in companyDto.Employees)
                {
                    _context.Employees.Add(new Employee
                    {
                        FirstName = dtoEmployee.FirstName,
                        LastName = dtoEmployee.LastName,
                        DateOfBirth = dtoEmployee.DateOfBirth,
                        JobTitle = dtoEmployee.JobTitle,
                        Company = newCompany.Entity
                    });
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

            await _context.Employees.ForEachAsync(employee =>
            {
                if (employee.Company == company)
                    _context.Employees.Remove(employee);
            });

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Company>>> SearchCompanies(SearchCompanyDto searchDto)
        {
            ServiceResponse<List<Company>> serviceResponse = new ServiceResponse<List<Company>>();

            List<Company> companies = new List<Company>();

            bool isKeywordValid = !string.IsNullOrEmpty(searchDto.Keyword);

            await _context.Companies.Include("Employees").ForEachAsync(contextCompany => {
                if (isKeywordValid && contextCompany.Name.IndexOf(searchDto.Keyword, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                    contextCompany.Employees.Any(employee => employee.FirstName.IndexOf(searchDto.Keyword, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                    employee.LastName.IndexOf(searchDto.Keyword, StringComparison.CurrentCultureIgnoreCase) != -1))
                {
                    companies.Add(contextCompany);
                }
                else if (searchDto.EmployeeDateOfBirthFrom != null && searchDto.EmployeeDateOfBirthTo != null)
                {
                    if (contextCompany.Employees.Any(employee => employee.DateOfBirth >= searchDto.EmployeeDateOfBirthFrom &&
                        employee.DateOfBirth <= searchDto.EmployeeDateOfBirthTo))
                    {
                        companies.Add(contextCompany);
                    }
                }
                else
                {
                    if (contextCompany.Employees.Any(employee => searchDto.EmployeesJobTitles.Any(title => employee.JobTitle == title)))
                    {
                        companies.Add(contextCompany);
                    }
                }
            });

            serviceResponse.Data = companies;
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateCompany(long id, CompanyInputDto companyDto)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
            try
            {
                Company company = _context.Companies.Where(c => c.CompanyID == id).First();
                if (company == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "There is no company with that company id.";
                    return serviceResponse;
                }

                company.Name = companyDto.Name;
                company.EstablishmentYear = companyDto.EstablishmentYear;

                await _context.Employees.ForEachAsync(employee =>
                {
                    if (employee.Company == company)
                        _context.Employees.Remove(employee);
                });

                foreach (var dtoEmployee in companyDto.Employees)
                {
                    await _context.Employees.AddAsync(new Employee
                    {
                        FirstName = dtoEmployee.FirstName,
                        LastName = dtoEmployee.LastName,
                        DateOfBirth = dtoEmployee.DateOfBirth,
                        JobTitle = dtoEmployee.JobTitle,
                        Company = company
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Exception: {ex.Message}, InnerException: {ex.InnerException.Message}";
            }

            return serviceResponse;
        }
    }
}
