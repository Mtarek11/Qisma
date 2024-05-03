using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LoftyContext(DbContextOptions<LoftyContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyFacility> PropertyFacilities { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<City> Cities {  get; set; }
        public DbSet<BuyTracker> BuyTrackers {  get; set; }
        public DbSet<Order> Orders {  get; set; }
        public DbSet<PropertyUnitPrice> PropertyUnitPrices {  get; set; }
        public DbSet<PropertyRentalYield> PropertyRentalYields {  get; set; }
        public DbSet<AboutQisma> AboutQisma {  get; set; }
        public DbSet<PropertyStatus> PropertyStatuses {  get; set; }
        public DbSet<FAQ> FAQs {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacilityConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyFacilityConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyImageConfigurations());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new GovernorateConfigurations());
            modelBuilder.ApplyConfiguration(new CityConfigurations());
            modelBuilder.ApplyConfiguration(new BuyTrackerConfigurations());
            modelBuilder.ApplyConfiguration(new OrderConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyUnitPriceConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyRentalYieldConfigurations());
            modelBuilder.ApplyConfiguration(new AboutQismaConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyStatusConfigurations());
            modelBuilder.ApplyConfiguration(new FAQConfigurations());
            modelBuilder.DataSeed();
            base.OnModelCreating(modelBuilder);
        }
    }
}
