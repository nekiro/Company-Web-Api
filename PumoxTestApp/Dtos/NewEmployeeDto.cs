using Newtonsoft.Json.Linq;
using PumoxTestApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PumoxTestApp.Dtos
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
