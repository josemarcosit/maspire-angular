using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace angular_vega.Migrations
{
    /// <inheritdoc />
    public partial class SeedFeatureTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // make sure translation table exists (could be created by another migration)
            migrationBuilder.Sql(@"IF OBJECT_ID('FeatureTranslations','U') IS NULL
BEGIN
    CREATE TABLE [FeatureTranslations](
        [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [FeatureId] int NOT NULL,
        [Language] nvarchar(10) NOT NULL,
        [Name] nvarchar(255) NOT NULL
    );
    CREATE UNIQUE INDEX IX_FeatureTranslations_FeatureId_Language ON FeatureTranslations(FeatureId,Language);
    ALTER TABLE [FeatureTranslations] WITH CHECK ADD CONSTRAINT FK_FeatureTranslations_Features_FeatureId FOREIGN KEY([FeatureId]) REFERENCES [Features]([Id]) ON DELETE CASCADE;
END");

            // seed translations if they don't already exist
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'en-US', 'AWD Traction' FROM Features WHERE Name = 'Tração AWD' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'en-US'
                );");
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'en-US', 'Off-Road Mode' FROM Features WHERE Name = 'Modo Off-Road' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'en-US'
                );");
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'en-US', 'Long Range Battery' FROM Features WHERE Name = 'Bateria de Longa Autonomia' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'en-US'
                );");
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'en-US', 'Adaptive Suspension' FROM Features WHERE Name = 'Suspensão Adaptativa' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'en-US'
                );");
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'en-US', 'Underbody Protection (Skid Plate)' FROM Features WHERE Name = 'Proteção Inferior (Skid Plate)' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'en-US'
                );");

            // additional spanish
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'es', 'Tracción AWD' FROM Features WHERE Name = 'Tração AWD' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'es'
                );");
            migrationBuilder.Sql(@"INSERT INTO FeatureTranslations (FeatureId, Language, Name)
                SELECT Id, 'es', 'Modo todoterreno' FROM Features WHERE Name = 'Modo Off-Road' AND NOT EXISTS(
                    SELECT 1 FROM FeatureTranslations WHERE FeatureId = Features.Id AND Language = 'es'
                );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove only the rows we added in Up
            migrationBuilder.Sql("DELETE FROM FeatureTranslations WHERE Language IN ('en-US','es');");
        }
    }
}
