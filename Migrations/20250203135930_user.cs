using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "F/1ej0Vaxgmy9OfJoZ0egsdYcMRZPHrts7GO6bmTmadk964gfyu0wT+tcx0oFVtZM6QZ+jBjE5mROtT6yMozlQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "jSl3cQnIE647KMsck4rAU9BJMUMnz5cwJPYiGJVc6ZhFBwNWLWTFJ/ccqYVAIpjakN2U1SXLzrisKOMd/CCHnA==");
        }
    }
}
