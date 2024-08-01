using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAirDemo.Migrations
{
    /// <inheritdoc />
    public partial class updateDashboeardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Operations",
                table: "DashboardData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operations",
                table: "DashboardData");
        }
    }
}
