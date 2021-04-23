using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Text;

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

        [HttpGet]
        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes("GetString");
        }

        [HttpDelete]
        public string DeleteString()
        {
            return "DeleteString";
        }

        [HttpPost]
        public string PostString()
        {
            return "PostString";
        }

        [HttpPut]
        public string PutString()
        {
            return "PutString";
        }

        [HttpPost]
        public string PostNullString()
        {
            return null;
        }

        [HttpPut]
        public string PutNullString()
        {
            return null;
        }

        [HttpPost]
        public IActionResult PostArrayString()
        {
            return new JsonResult(new string[] { "string1", "string2" });
        }

        [HttpPut]
        public IActionResult PutArrayString()
        {
            return new JsonResult(new string[] { "string1", "string2" });
        }

        [HttpPost]
        public IActionResult PostEmptyArrayString()
        {
            return new JsonResult(new string[0]);
        }

        [HttpPut]
        public IActionResult PutEmptyArrayString()
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

        [HttpPut]
        public string PutReturnHeaderValue()
        {
            return Request.Headers["test_header"];
        }
    }
}
