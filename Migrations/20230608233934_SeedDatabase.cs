using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace angular_vega.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // MAKES
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES('BYD');");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES('Rivian');");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES('Tesla');");

            // =========================
            // BYD MODELS
            // =========================
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Tang EV AWD', (SELECT Id FROM Makes WHERE Name = 'BYD'));");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Yuan Plus', (SELECT Id FROM Makes WHERE Name = 'BYD'));");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Song Plus EV', (SELECT Id FROM Makes WHERE Name = 'BYD'));");

            // =========================
            // RIVIAN MODELS
            // =========================
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('R1T Adventure', (SELECT Id FROM Makes WHERE Name = 'Rivian'));");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('R1S Adventure', (SELECT Id FROM Makes WHERE Name = 'Rivian'));");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('R1S Quad-Motor', (SELECT Id FROM Makes WHERE Name = 'Rivian'));");

            // =========================
            // TESLA MODELS
            // =========================
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Cybertruck AWD', (SELECT Id FROM Makes WHERE Name = 'Tesla'));");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Cybertruck Tri-Motor', (SELECT Id FROM Makes WHERE Name = 'Tesla'));");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Model X Plaid AWD', (SELECT Id FROM Makes WHERE Name = 'Tesla'));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Models;");
            migrationBuilder.Sql("DELETE FROM Makes WHERE Name IN ('BYD','Rivian','Tesla');");
        }
    }
}