using System;
using System.Collections.Generic;
using System.Text;
using LinqTests.Model;
using Microsoft.EntityFrameworkCore;

namespace LinqTests.Data
{
    public class VehicleContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manufacturer>().ToTable("Manufacturers");

            var vehicleEntity = modelBuilder.Entity<Vehicle>();
            vehicleEntity.ToTable("Vehicles");

            vehicleEntity.OwnsOne(v => v.Engine);
            vehicleEntity.OwnsOne(v => v.Drivetrain);
            vehicleEntity.OwnsOne(v => v.FuelEconomy);

            var drivetrainEntity = modelBuilder.Entity<Drivetrain>();
            drivetrainEntity.OwnsOne(dt => dt.Transmission);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("vehicles.db");
        }
    }
}
