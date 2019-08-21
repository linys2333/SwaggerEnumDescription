using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(TestEnum testEnum)
        {
            return new string[] { "value1", "value2" };
        }
    }
}
