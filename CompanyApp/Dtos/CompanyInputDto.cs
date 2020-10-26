using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyApp.Dtos
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
