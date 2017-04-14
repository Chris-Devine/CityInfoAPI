using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.api.Entities
{
    public class PointOfInterest
    {
        // Key annotation tells the EF Framework to make this field a primary key
        [Key]
        // Allows you to specifically tell EF to create an ID on add. Can also use none or computed, computed will create an id on add or update.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /*
         * Here we can just use the public City City {get;set;} property to map the points of intrest baack to the 
         * parent city calss by convention, as EF should automatically create a FK of CityId. 
         * Although convention says this will happen its best practice to state a property of CityID and place 
         * an annotation on the navigation property that tells EF that the CityId property should be used as the
         * forigenkey of the navigation property.
         */

        [ForeignKey("CityId")]
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
