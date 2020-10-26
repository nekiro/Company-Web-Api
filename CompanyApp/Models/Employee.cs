using System;

namespace CompanyApp.Models
{
    public enum JobTitle
    {
        Administrator = 1,
        Developer = 2,
        Architect = 3,
        Manager = 4
    }

    public class Employee
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public JobTitle JobTitle { get; set; }

        public Company Company { get; set; }
    }
}
