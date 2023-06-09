using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace angular_vega.Migrations
{
    /// <inheritdoc />
    public partial class SeedFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Feature1');");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Feature2');");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES('Feature3');");

            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(1, (SELECT Id FROM Makes WHERE Name = 'Make1'));;");
            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(2, (SELECT Id FROM Makes WHERE Name = 'Make1'));;");
            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(3, (SELECT Id FROM Makes WHERE Name = 'Make1'));;");

            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(1, (SELECT Id FROM Makes WHERE Name = 'Make2'));;");
            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(2, (SELECT Id FROM Makes WHERE Name = 'Make2'));;");
            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(3, (SELECT Id FROM Makes WHERE Name = 'Make2'));;");

            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(1, (SELECT Id FROM Makes WHERE Name = 'Make3'));;");
            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(2, (SELECT Id FROM Makes WHERE Name = 'Make3'));;");
            migrationBuilder.Sql("INSERT INTO FeatureMake(FeaturesId, MakeId)VALUES(3, (SELECT Id FROM Makes WHERE Name = 'Make3'));;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Features WHERE Name IN ('Feature1','Feature2','Feature3')");
        }
    }
}
