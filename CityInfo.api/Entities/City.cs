using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.api.Entities
{
    public class City
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        // Should initialise lists to empty lists to avoid null ref exception
        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
                = new List<PointOfInterest>();
    }
}
