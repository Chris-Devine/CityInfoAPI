using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.api.Entities
{
    public class CityInfoContext: DbContext
    {

        // By adding this constructor it will allow us to pass in the connection string in the startup configuration of services.
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
           : base(options)
        {
            // Will create the DB if it does not exsist 
            Database.EnsureCreated();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        // We can declare the connection string for the DbContext here by overriding the OnConfiguring method
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
