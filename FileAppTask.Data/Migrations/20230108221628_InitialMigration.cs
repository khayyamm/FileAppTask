using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileAppTask.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "FileShares",
                columns: table => new
                {
                    FileShareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DownloadAvailableUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileShares", x => x.FileShareId);
                    table.ForeignKey(
                        name: "FK_FileShares_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileShares_FileId",
                table: "FileShares",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileShares");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
