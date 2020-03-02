using System;
using System.Collections.Generic;
using System.Text;

namespace LinqTests.Model
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public int ModelYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Engine Engine { get; set; }
        public Drivetrain Drivetrain { get; set; }
        public FuelEconomy FuelEconomy { get; set; } 

        // Foreign Keys
        public Guid ManufacturerId { get; set; }

        // Foreign Key Items (navigation property)
        public Manufacturer Manufacturer { get; set; }
    }

    public class Engine
    {
        public Aspiration Aspiration { get; set; }
        public float DisplacementLiters { get; set; }
        public int Cylinders { get; set; }
        //public int IntakeValvesPerCylinder { get; set; }
        //public int ExhaustValvesPerCylinder { get; set; }
        //public bool CylinderDeactivation { get; set; }
        //public bool VariableValveTiming { get; set; }
        //public bool VariableValveLift { get; set; }

        public FuelInfo Fuel { get; set; }
    }

    public enum Aspiration
    {
        NaturallyAspirated,
        Supercharged,
        Turbocharged,
        TurbochargedAndSupercharged,
        Other
    }

    public class FuelInfo
    {
        public FuelType FuelType { get; set; }
        public FuelGrade? FuelGrade { get; set; }
    }

    public enum FuelType
    {
        Gasoline,
        Diesel
    }

    public enum FuelGrade
    {
        Regular,
        MidGradeRecommended,
        PremiumRecommended,
        PremiumRequired
    }

    public enum DrivetrainType
    {
        RearWheelDrive,
        FrontWheelDrive,
        AllWheelDrive,
        FourWheelDrive,
        PartTimeFourWheelDrive
    }

    public class Drivetrain
    {
        public DrivetrainType Type { get; set; }
        public bool LockupTorqueConverter { get; set; }
        public Transmission Transmission { get; set; }
    }

    public enum TransmissionType
    {
        Manual,
        Automatic
    }

    public class Transmission
    {
        public TransmissionType Type { get; set; }
        public int Gears { get; set; }
    }

    public class FuelEconomy
    {
        public int City { get; set; }
        public int Highway { get; set; }
        public int Combined { get; set; }
    }
}
