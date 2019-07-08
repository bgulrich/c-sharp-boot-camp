using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using LinqTests.Model;

namespace LinqTests.Data
{
    public static class DataLoader
    {
        public static IEnumerable<Vehicle> LoadVehiclesFromExcel()
        {
            using (var workbook = new XLWorkbook("vehicles.xlsx"))
            {
                var worksheet = workbook.Worksheet("Vehicles");

                // get manufacturers
                var manufacturerColumn = worksheet.Column(2);

                var manufacturerMap = manufacturerColumn.Cells().Skip(1)
                                                        .Select(c => c.Value as string)
                                                        .Distinct()
                                                        .Select(s => new Manufacturer
                                                        {
                                                            Name = s
                                                        })
                                                        .ToDictionary(m => m.Name);

                // skip header line
                return worksheet.Rows().Skip(1)
                    .Select(r => new Vehicle
                    {
                        Manufacturer = manufacturerMap[r.Cell(2).GetString()],
                        ModelYear = r.Cell(1).GetValue<int>(),
                        Make = r.Cell(3).GetString(),
                        Model = r.Cell(4).GetString(),
                        ReleaseDate = r.Cell(25).GetDateTime(),
                        Engine = new Engine
                        {
                            Aspiration = r.Cell(11).GetEnum<Aspiration>(),
                            DisplacementLiters = r.Cell(5).GetValue<float>(),
                            Cylinders = r.Cell(6).GetValue<int>(),
                            Fuel = r.Cell(18).GetFuelInfo()
                        },
                        Drivetrain = new Drivetrain
                        {
                            Type = r.Cell(16).GetDrivetrainType(),
                            LockupTorqueConverter = r.Cell(15).GetYesNo(),
                            Transmission = new Transmission
                            {
                                Type = r.Cell(7).GetEnum<TransmissionType>(),
                                Gears = r.Cell(14).GetValue<int>()
                             }
                        },
                        FuelEconomy = new FuelEconomy
                        {
                            City = r.Cell(8).GetValue<int>(),
                            Highway = r.Cell(9).GetValue<int>(),
                            Combined = r.Cell(10).GetValue<int>()
                        }
                    });
            }
        }

        public static TEnum GetEnum<TEnum>(this IXLCell cell) where TEnum : struct, Enum
        {
            var value = cell.GetString();
            return Enum.Parse<TEnum>(value);
        }

        public static bool GetYesNo(this IXLCell cell)
        {
            var value = cell.GetString();

            switch (value)
            {
                case "Y": return true;
                case "N": return false;
            }
            throw new Exception();
        }

        public static DrivetrainType GetDrivetrainType(this IXLCell cell)
        {
            var value = cell.GetString();
            switch (value)
            {
                case "A": return DrivetrainType.AllWheelDrive;
                case "F": return DrivetrainType.FrontWheelDrive;
                case "R": return DrivetrainType.RearWheelDrive;
                case "4": return DrivetrainType.FourWheelDrive;
                case "P": return DrivetrainType.PartTimeFourWheelDrive;
            }

            throw new Exception();
        }

        public static FuelInfo GetFuelInfo(this IXLCell cell)
        {
            var value = cell.GetString();

            switch (value.First())
            {
                case 'D':
                {
                    switch (value)
                    {
                        case "DU": return new FuelInfo{ FuelType = FuelType.Diesel };
                        default: throw new Exception();
                    }
                }
                case 'G':
                {
                    switch(value)
                    {
                        case "G" :  return new FuelInfo{ FuelType = FuelType.Gasoline, FuelGrade = FuelGrade.Regular };
                        case "GM":  return new FuelInfo{ FuelType = FuelType.Gasoline, FuelGrade = FuelGrade.MidGradeRecommended };
                        case "GP":  return new FuelInfo{ FuelType = FuelType.Gasoline, FuelGrade = FuelGrade.PremiumRecommended };
                        case "GPR": return new FuelInfo{ FuelType = FuelType.Gasoline, FuelGrade = FuelGrade.PremiumRequired };
                        default: throw new Exception();
                    }
                }
            }

            throw new Exception();
        }
    }
}
