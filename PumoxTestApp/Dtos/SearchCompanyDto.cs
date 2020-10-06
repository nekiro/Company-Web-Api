using Newtonsoft.Json.Linq;
using PumoxTestApp.Models;
using System;
using System.Collections.Generic;

namespace PumoxTestApp.Dtos
{
    public class SearchCompanyDto
    {
        public string Keyword;
        public DateTime? EmployeeDateOfBirthFrom;
        public DateTime? EmployeeDateOfBirthTo;
        public List<JobTitle> EmployeesJobTitles = new List<JobTitle>();

        public void ParseJsonData(JObject obj)
        {
            Keyword = obj.Value<string>("Keyword");
            EmployeeDateOfBirthFrom = obj.Value<DateTime?>("EmployeeDateOfBirthFrom");
            EmployeeDateOfBirthTo = obj.Value<DateTime?>("EmployeeDateOfBirthTo");

            if (obj.ContainsKey("EmployeesJobTitles"))
            {
                foreach (var titleToken in obj.Value<JArray>("EmployeesJobTitles"))
                {
                    EmployeesJobTitles.Add(Enum.Parse<JobTitle>(titleToken.Value<string>()));
                }
            }
        }
    }
}
