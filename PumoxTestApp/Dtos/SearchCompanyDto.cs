using PumoxTestApp.Models;
using System;
using System.Collections.Generic;

namespace PumoxTestApp.Dtos
{
    public class SearchCompanyDto
    {
        public string Keyword { get; set; }
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public ICollection<JobTitle> EmployeesJobTitles { get; set; }
    }
}
