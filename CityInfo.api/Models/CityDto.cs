using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfIntrest { get
            {
                return PointOfIntrest.Count;
            }
        }

        public ICollection<PointOfIntrestDto> PointOfIntrest { get; set; } = new List<PointOfIntrestDto>();
    }
}
