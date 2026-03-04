using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace maspire_angular.Migrations
{
    public partial class SeedFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // FEATURES (OffRoad Elétricos)
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Tração AWD');");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Modo Off-Road');");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Bateria de Longa Autonomia');");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Suspensão Adaptativa');");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Proteção Inferior (Skid Plate)');");

            // =========================
            // BYD
            // =========================
            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Tração AWD'),
                    (SELECT Id FROM Makes WHERE Name = 'BYD')
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Modo Off-Road'),
                    (SELECT Id FROM Makes WHERE Name = 'BYD')
                );
            ");

            // =========================
            // RIVIAN
            // =========================
            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Tração AWD'),
                    (SELECT Id FROM Makes WHERE Name = 'Rivian')
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Suspensão Adaptativa'),
                    (SELECT Id FROM Makes WHERE Name = 'Rivian')
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Proteção Inferior (Skid Plate)'),
                    (SELECT Id FROM Makes WHERE Name = 'Rivian')
                );
            ");

            // =========================
            // TESLA
            // =========================
            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Tração AWD'),
                    (SELECT Id FROM Makes WHERE Name = 'Tesla')
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO FeatureMake (FeaturesId, MakeId)
                VALUES (
                    (SELECT Id FROM Features WHERE Name = 'Bateria de Longa Autonomia'),
                    (SELECT Id FROM Makes WHERE Name = 'Tesla')
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM FeatureMake;");
            migrationBuilder.Sql("DELETE FROM Features WHERE Name IN ('Tração AWD','Modo Off-Road','Bateria de Longa Autonomia','Suspensão Adaptativa','Proteção Inferior (Skid Plate)');");
        }
    }
}