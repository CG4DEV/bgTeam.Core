using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.bgTeam.Web
{
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string GetString()
        {
            return "GetString";
        }

        [HttpPost]
        public string PostString()
        {
            return "PostString";
        }

        [HttpPost]
        public string PostNullString()
        {
            return null;
        }

        [HttpPost]
        public IActionResult PostArrayString()
        {
            return new JsonResult(new string[] { "string1", "string2" });
        }

        [HttpPost]
        public IActionResult PostEmptyArrayString()
        {
            return new JsonResult(new string[0]);
        }
    }
}
