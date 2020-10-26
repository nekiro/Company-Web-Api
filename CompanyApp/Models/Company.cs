using System.Collections.Generic;

namespace CompanyApp.Models
{
    public class Company
    {
        public long CompanyID { get; set; }
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
