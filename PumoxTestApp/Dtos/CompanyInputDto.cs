using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PumoxTestApp.Dtos
{
    public class CompanyInputDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int? EstablishmentYear { get; set; }

        public ICollection<NewEmployeeDto> Employees { get; set; }
    }
}
