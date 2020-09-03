using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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

        [HttpGet]
        public string GetQueryString(string query)
        {
            return query;
        }

        [HttpGet]
        public string GetQueryInt(int query)
        {
            return query.ToString(CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetQueryDouble(double query)
        {
            return query.ToString(CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetQueryDateTime(DateTime query)
        {
            return query.ToString(CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetQueryDateTimeOffset(DateTimeOffset query)
        {
            return query.ToString(CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetQueryTimeSpan(TimeSpan query)
        {
            return query.ToString("G", CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetQueryFloat(float query)
        {
            return query.ToString(CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetQueryDecimal(decimal query)
        {
            return query.ToString(CultureInfo.InvariantCulture);
        }

        [HttpGet]
        public string GetReturnHeaderValue()
        {
            return Request.Headers["test_header"];
        }

        [HttpPost]
        public string PostReturnHeaderValue()
        {
            return Request.Headers["test_header"];
        }
    }
}
