using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EfCoreExample.Migrations
{
    public partial class Base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelYear = table.Column<int>(nullable: false),
                    Make = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    Engine_Aspiration = table.Column<int>(nullable: false),
                    Engine_DisplacementLiters = table.Column<float>(nullable: false),
                    Engine_Cylinders = table.Column<int>(nullable: false),
                    Engine_Fuel_FuelType = table.Column<int>(nullable: false),
                    Engine_Fuel_FuelGrade = table.Column<int>(nullable: true),
                    Drivetrain_Type = table.Column<int>(nullable: false),
                    Drivetrain_LockupTorqueConverter = table.Column<bool>(nullable: false),
                    Drivetrain_Transmission_Type = table.Column<int>(nullable: false),
                    Drivetrain_Transmission_Gears = table.Column<int>(nullable: false),
                    FuelEconomy_City = table.Column<int>(nullable: false),
                    FuelEconomy_Highway = table.Column<int>(nullable: false),
                    FuelEconomy_Combined = table.Column<int>(nullable: false),
                    ManufacturerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ManufacturerId",
                table: "Vehicles",
                column: "ManufacturerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
