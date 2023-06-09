using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace angular_vega.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES('Make1');");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES('Make2');");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES('Make3');");


            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make1-ModelA', 1);");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make1-ModelB', 1);");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make1-ModelC', 1);");

            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make2-ModelA', 2);");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make2-ModelB', 2);");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make2-ModelC', 2);");

            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make3-ModelA', 3);");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make3-ModelB', 3);");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES('Make3-ModelC', 3);");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Makes");
            migrationBuilder.Sql("DELETE FROM Models");
        }
    }
}
