using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhatsAppUserSessions",
                columns: table => new
                {
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentFormType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FormData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppUserSessions", x => x.PhoneNumber);
                });

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 29, 17, 14, 28, 707, DateTimeKind.Local).AddTicks(1109));

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 13, 17, 14, 28, 707, DateTimeKind.Local).AddTicks(1151));

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 22, 17, 14, 28, 707, DateTimeKind.Local).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2025, 5, 13, 17, 14, 28, 707, DateTimeKind.Local).AddTicks(1185));

            migrationBuilder.UpdateData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmissionDate",
                value: new DateTime(2025, 6, 13, 17, 14, 28, 707, DateTimeKind.Local).AddTicks(1191));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhatsAppUserSessions");

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 15, 1, 38, 9, 673, DateTimeKind.Local).AddTicks(5981));

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 5, 29, 1, 38, 9, 673, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 8, 1, 38, 9, 673, DateTimeKind.Local).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2025, 4, 29, 1, 38, 9, 673, DateTimeKind.Local).AddTicks(6064));

            migrationBuilder.UpdateData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmissionDate",
                value: new DateTime(2025, 5, 29, 1, 38, 9, 673, DateTimeKind.Local).AddTicks(6068));
        }
    }
}
