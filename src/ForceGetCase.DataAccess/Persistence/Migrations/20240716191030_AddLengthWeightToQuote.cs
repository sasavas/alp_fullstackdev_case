using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForceGetCase.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLengthWeightToQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lenght",
                table: "Quotes",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Quotes",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lenght",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Quotes");
        }
    }
}
