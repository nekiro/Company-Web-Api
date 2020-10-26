using CompanyApp.Models;
using System;
using System.Collections.Generic;

namespace CompanyApp.Dtos
{
    public class SearchCompanyDto
    {
        public string Keyword { get; set; }
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public ICollection<JobTitle> EmployeesJobTitles { get; set; }
    }
}
