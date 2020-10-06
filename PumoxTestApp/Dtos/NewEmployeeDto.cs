using Newtonsoft.Json.Linq;
using PumoxTestApp.Models;
using System;
using System.Collections.Generic;

namespace PumoxTestApp.Dtos
{
    public class NewEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public JobTitle JobTitle { get; set; }

        public bool ParseJsonData(JToken token)
        {
            IDictionary<string, JToken> tokenDict = (IDictionary<string, JToken>)token;
            if (!tokenDict.ContainsKey("FirstName") || !tokenDict.ContainsKey("LastName") || !tokenDict.ContainsKey("DateOfBirth")
                || !tokenDict.ContainsKey("JobTitle"))
            {
                return false;
            }

            FirstName = token.Value<string>("FirstName");
            LastName = token.Value<string>("LastName");
            DateOfBirth = token.Value<DateTime>("DateOfBirth");
            JobTitle = Enum.Parse<JobTitle>(token.Value<string>("JobTitle"));
            return true;
        }
    }
}
