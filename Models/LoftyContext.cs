﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacilityConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyFacilityConfigurations());
            modelBuilder.ApplyConfiguration(new PropertyImageConfigurations());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new GovernorateConfigurations());
            modelBuilder.ApplyConfiguration(new CityConfigurations());
            modelBuilder.DataSeed();
            base.OnModelCreating(modelBuilder);
        }
    }
}