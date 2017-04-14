using Microsoft.AspNetCore.Mvc;
using CityInfo.api.Entities;

namespace CityInfo.api.Controllers
{
    public class DummyController: Controller
    {
        private CityInfoContext _ctx;
        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
