using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAirDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddingDashboardData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DashboardData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Projects = table.Column<int>(type: "int", nullable: false),
                    Clients = table.Column<int>(type: "int", nullable: false),
                    Contractors = table.Column<int>(type: "int", nullable: false),
                    Suppliers = table.Column<int>(type: "int", nullable: false),
                    Images = table.Column<int>(type: "int", nullable: false),
                    Pdfs = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardData");
        }
    }
}
