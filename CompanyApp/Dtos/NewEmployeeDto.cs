using CompanyApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyApp.Dtos
{
    public class NewEmployeeDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public JobTitle? JobTitle { get; set; }
    }
}
