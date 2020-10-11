using System.Collections.Generic;

namespace PumoxTestApp.Models
{
    public class Company
    {
        public long CompanyID { get; set; }
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
