using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VODLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Uploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Length = table.Column<TimeSpan>(type: "time", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislike = table.Column<int>(type: "int", nullable: false),
                    VideoOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoRecords_AspNetUsers_VideoOwnerId",
                        column: x => x.VideoOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoRecords_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Music" },
                    { 2, "Sports" },
                    { 3, "Gaming" },
                    { 4, "Entertainment" },
                    { 5, "Education" },
                    { 6, "Science and Technology" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoRecords_CategoryId",
                table: "VideoRecords",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRecords_VideoOwnerId",
                table: "VideoRecords",
                column: "VideoOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoRecords");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
