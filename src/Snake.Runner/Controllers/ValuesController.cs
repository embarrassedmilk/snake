using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace temp.Controllers
{
    public interface IValuesRepository
    {
        IEnumerable<string> Get();
    }

    public class ValuesRepository : IValuesRepository
    {
        public IEnumerable<string> Get() => new string[] { "1", "2" };
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IValuesRepository _valuesRepository;

        public ValuesController(IValuesRepository valuesRepository)
        {
            _valuesRepository = valuesRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.Logger.Information("test");

            return Ok(_valuesRepository.Get());
        }
    }
}
