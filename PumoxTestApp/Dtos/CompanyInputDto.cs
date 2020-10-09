using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace PumoxTestApp.Dtos
{
    public class CompanyInputDto
    {
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public List<NewEmployeeDto> Employees { get; set; } = new List<NewEmployeeDto>();

        public (bool Success, string Error) ParseJsonData(JObject obj)
        {
            try
            {
                if (!obj.ContainsKey("Name") || !obj.ContainsKey("EstablishmentYear"))
                {
                    return (false, "Required property is missing. Required properties: Name:<string>, EstablishmentYear:<integer>");
                }

                Name = obj.Value<string>("Name");
                EstablishmentYear = obj.Value<int>("EstablishmentYear");

                if (obj.ContainsKey("Employees"))
                {
                    foreach (var jsonEmployee in obj.Value<JArray>("Employees"))
                    {
                        NewEmployeeDto employeeDto = new NewEmployeeDto();
                        if (!employeeDto.ParseJsonData(jsonEmployee))
                        {
                            continue;
                        }

                        Employees.Add(employeeDto);
                    }
                }

                return (true, "");
            }
            catch (FormatException)
            {
                //thrown when obj.Value<type> fails
                return (false, "Data type is invalid. Data types: Name:<string>, EstablishmentYear:<integer>");
            }
            catch (InvalidCastException)
            {
                //thrown when obj.Value<type> fails and the value is null
                return (false, "Data type is invalid. Data types: Name:<string>, EstablishmentYear:<integer>");
            }
        }
    }
}
