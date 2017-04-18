using CityInfo.api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CityInfo.api.Services;

namespace CityInfo.api.Controllers
{
    [Route("api/cities")]
    public class PointsOfIntrestController : Controller
    {
        private ILogger<PointsOfIntrestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfIntrestController(
            ILogger<PointsOfIntrestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository
            )
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfIntrest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest");
                    return NotFound();
                }
                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);
                var pointsOfInterestForCityResults = Mapper.Map<IEnumerable<PointOfIntrestDto>>(pointsOfInterestForCity);

                return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult getPointOfIntrest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointsOfIntrest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointsOfIntrest == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map<PointOfIntrestDto>(pointsOfIntrest);

            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfIntrestCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            // Should not do this (was covered because microsoft use it) but better way is to use "fluent validation"
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be diffrent from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // Demo Purpose will be improved later
            var maxPointOfIntrest = CitiesDataStore.current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfIntrest = new PointOfIntrestDto()
            {
                Id = maxPointOfIntrest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfIntrest);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                id = finalPointOfIntrest.Id
            }, finalPointOfIntrest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfIntrestUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            // Should not do this (was covered because microsoft use it) but better way is to use "fluent validation"
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be diffrent from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfIntrestUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfIntrestUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            // When applying patch document, pass in the ModelState so any errors that occur when applying, can be added to the ModelState. THis will only validate the JsonDocument
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be diffrent from the name.");
            }

            // This will now validate the PointOfIntrestUpdateDto Model and add any errors found to the ModelState
            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of interest deleted.",
                $"Point of intrest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            return NoContent();
        }
    }
}
