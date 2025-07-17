using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDoctorRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullPhoneNumber",
                table: "DoctorJoinRequests");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "DoctorJoinRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "DoctorJoinRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FullPhoneNumber",
                table: "DoctorJoinRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 14, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1595));

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 5, 28, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1636));

            migrationBuilder.UpdateData(
                table: "CustomerReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 7, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1641));

            migrationBuilder.UpdateData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2025, 4, 28, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1673));

            migrationBuilder.UpdateData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmissionDate",
                value: new DateTime(2025, 5, 28, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1678));
        }
    }
}
