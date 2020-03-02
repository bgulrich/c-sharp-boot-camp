using System;
using System.Collections.Generic;
using System.Text;
using LinqTests.Model;
using Microsoft.EntityFrameworkCore;

namespace EfCoreExample.Data
{
    public class VehicleContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var manufacturerEntity = modelBuilder.Entity<Manufacturer>();
            manufacturerEntity.ToTable("Manufacturers");
            manufacturerEntity.HasKey(m => m.Id);

            var vehicleEntity = modelBuilder.Entity<Vehicle>();
            vehicleEntity.ToTable("Vehicles");
            vehicleEntity.HasOne(v => v.Manufacturer).WithMany(m => m.Vehicles).HasForeignKey(v => v.ManufacturerId);

            vehicleEntity.OwnsOne(v => v.Engine).OwnsOne(e => e.Fuel);
            vehicleEntity.OwnsOne(v => v.Drivetrain);
            vehicleEntity.OwnsOne(v => v.FuelEconomy);

            var drivetrainEntity = modelBuilder.Entity<Drivetrain>();
            drivetrainEntity.OwnsOne(dt => dt.Transmission);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Data Source=vehicles.db;");
        }
    }
}
