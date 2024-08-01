using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAirDemo.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDashboardData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                           table: "DashboardData",
                           columns: new[] { "Projects" , "Clients" ,  "Contractors" , "Suppliers" , "Images" , "Pdfs"},
                           values: new object[] { 0 , 0 , 0 , 0 ,0 , 0 }
                       );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELTE FROM [DashboardData]");
        }
    }
}
