using CityInfo.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.api
{
    public class CitiesDataStore
    {
        public static CitiesDataStore current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "Hell hole",
                    PointOfIntrest = new List<PointOfIntrestDto>()
                    {
                        new PointOfIntrestDto()
                        {
                            Id=1,
                            Name = "The Memorial",
                            Description = "Where the twin towers stood before a tragic event took place"
                        },
                        new PointOfIntrestDto()
                        {
                            Id=2,
                            Name = "Trump Tower",
                            Description = "Where the president of America runs business XD"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Runcorn",
                    Description = "Swamp on no return",
                    PointOfIntrest = new List<PointOfIntrestDto>()
                    {
                        new PointOfIntrestDto()
                        {
                            Id=1,
                            Name = "The City",
                            Description = "A place where zombie like heroine adicts get the daily shopping and there dole money"
                        },
                        new PointOfIntrestDto()
                        {
                            Id=2,
                            Name = "The Old Town",
                            Description = "A place heroine addicts go to 'dance' and 'socialise' on there days off (everyday)"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Manchester",
                    Description = "House of horrors",
                    PointOfIntrest = new List<PointOfIntrestDto>()
                    {
                        new PointOfIntrestDto()
                        {
                            Id=1,
                            Name = "The Trafford Center",
                            Description = "A shoppers dream, a foodies delight and a man with a wallets worst nightmare...."
                        },
                        new PointOfIntrestDto()
                        {
                            Id=2,
                            Name = "Curry Mile",
                            Description = "A mile long road of places that will give your digestive track a good seeing too"
                        }
                    }
                }
            };
        }
    }
}
