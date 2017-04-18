using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.api.Entities;

namespace CityInfo.api.Services
{
    public interface ICityInfoRepository
    {
        /*
         * IQueryable allow the consumer to addon to the query before the query is executed, this leaks data peristance.
         * IEnummrable means that the consumer cannot addon to the query before its executed so data persitance is not leaked. 
         * Both are valid and there is a debate on which is best, I say IEnummrable
         */
        bool CityExists(int cityId);
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool includePointsOfIntrest);
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        bool Save();
    }
}
