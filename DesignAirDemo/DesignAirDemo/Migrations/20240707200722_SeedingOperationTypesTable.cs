using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAirDemo.Migrations
{
    /// <inheritdoc />
    public partial class SeedingOperationTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                           table: "OperationTypes",
                           columns: new[] { "Name", "Sign" },
                           values: new object[] { "تحصيلات", true }
                       );

            migrationBuilder.InsertData(
                table: "OperationTypes",
                columns: new[] { "Name", "Sign" },
                values: new object[] { "نثريات", false }
            );

            migrationBuilder.InsertData(
                table: "OperationTypes",
                columns: new[] { "Name", "Sign" },
                values: new object[] { "مدفوعات", false }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELTE FROM [OperationTypes]");
        }
    }
}
