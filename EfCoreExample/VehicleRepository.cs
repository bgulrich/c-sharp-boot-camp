using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqTests.Model;
using Microsoft.EntityFrameworkCore;

namespace EfCoreExample.Data
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<IGrouping<Manufacturer, Vehicle>>> GetVehiclesGroupedByManufacturerAsync();

        Task<IEnumerable<Manufacturer>> GetManufacturersOrderedByNameAsync();

        Task AddVehicleAsync(Vehicle vehicle);

        Task AddVehiclesAsync(IEnumerable<Vehicle> vehicles);

        Task ClearAllData();
    }

    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleContext _dbContext;

        public VehicleRepository()
        {
            _dbContext = new VehicleContext();
            _dbContext.Database.Migrate();
        }

        public Task AddVehicleAsync(Vehicle v)
        {
            throw new NotImplementedException();
        }

        public async Task AddVehiclesAsync(IEnumerable<Vehicle> vehicles)
        {
            await _dbContext.AddRangeAsync(vehicles);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearAllData()
        {
            // just delete manufacturers... cascade will take care of vehicles
            foreach (var m in _dbContext.Manufacturers)
            {
                _dbContext.Manufacturers.Remove(m);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturersOrderedByNameAsync()
        {
            return await _dbContext.Manufacturers.OrderBy(m => m.Name)
                                                 .ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<Manufacturer, Vehicle>>> GetVehiclesGroupedByManufacturerAsync()
        {
            return await _dbContext.Vehicles.Include(v => v.Manufacturer)
                                            .GroupBy(v => v.Manufacturer)
                                            .OrderBy(g => g.Key.Name)
                                            .ToListAsync();
        }
    }
}
