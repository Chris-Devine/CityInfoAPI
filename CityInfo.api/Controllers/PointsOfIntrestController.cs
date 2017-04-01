using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.api.Controllers
{
    [Route("api/cities")]
    public class PointsOfIntrestController: Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfIntrest(int cityId)
        {
            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointOfIntrest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}")]
        public IActionResult getPointOfIntrest(int cityId, int id)
        {
            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointsOfIntrest = city.PointOfIntrest.FirstOrDefault(p => p.Id == id);
            if (pointsOfIntrest == null)
            {
                return NotFound();
            }

            return Ok(pointsOfIntrest);
        }
    }
}
