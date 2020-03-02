using System;
using System.Linq;
using System.Threading.Tasks;
using EfCoreExample.Data;
using LinqTests.Data;

namespace EfCoreExample
{
    class Program
    {
        private static IVehicleRepository _vehicleRepository;

        static async Task Main(string[] args)
        {
            Console.Write("Initializing repository...");

            _vehicleRepository = new VehicleRepository();

            Console.WriteLine("done.");

            while(!await LoopAsync())
            {
                ;
            }
        }

        // returns true if it's time to quit
        private static async Task<bool> LoopAsync()
        {
            Console.WriteLine($"\r\nWhat would you like to do?");
            Console.WriteLine("  (S)eed database");
            Console.WriteLine("  (D)elete all data from database");
            Console.WriteLine("  List all (M)anufacturers");
            Console.WriteLine("  List all (V)ehicles by manufacturer");
            Console.WriteLine("  (Q)uit\r\n");

            var key = Console.ReadKey().KeyChar;

            Console.WriteLine();

            switch(key)
            {
                case 'q': return true;
                case 's': await SeedDatabaseAsync(); return false;
                case 'd': await ClearDatabaseAsync(); return false;
                case 'm': await ListManufacturersAsync(); return false;
                case 'v': await ListVehiclesByManufacturerAsync(); return false;
                default: Console.WriteLine("Invalid input... please try again"); return false;
            }
        }

        private static async Task ListVehiclesByManufacturerAsync()
        {
            foreach (var group in await _vehicleRepository.GetVehiclesGroupedByManufacturerAsync())
            {
                Console.WriteLine($"\r\n{group.Key.Name}:");

                foreach(var v in group)
                {
                    Console.WriteLine($"  {v.ModelYear} {v.Make} {v.Model}");
                }
            }
        }

        private static async Task ListManufacturersAsync()
        {
            foreach (var m in await _vehicleRepository.GetManufacturersOrderedByNameAsync())
            {
                Console.WriteLine(m.Name);
            }
        }

        private static async Task SeedDatabaseAsync()
        {
            Console.Write("Seeding database from file...");

            var vehicles = DataLoader.LoadVehiclesFromExcel();
            await _vehicleRepository.AddVehiclesAsync(vehicles);

            Console.WriteLine($"done.  {vehicles.Count()} vehicles from {vehicles.Select(v => v.Manufacturer).Distinct().Count()} manufacturers added.");
        }

        private static async Task ClearDatabaseAsync()
        {
            Console.Write("Removing all vehicles and manufacturers...");

            await _vehicleRepository.ClearAllData();

            Console.WriteLine("done.");
        }
    }
}
