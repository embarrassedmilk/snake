using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace temp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Log.Logger.Information("test");
            return new string[] { "value1", "value2" };
        }
    }
}
