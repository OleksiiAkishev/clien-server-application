using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UniversityMgmtSystemServerApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TimeTables",
                columns: new[] { "TimeTableId", "TimeTableName" },
                values: new object[] { 1, "1st semester" });

            migrationBuilder.InsertData(
                table: "Days",
                columns: new[] { "DayId", "DayNum", "TimeTableId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "ClassRoomId", "ClassroomName", "DayId" },
                values: new object[,]
                {
                    { 1, "A101", 1 },
                    { 2, "A102", 1 },
                    { 3, "A101", 2 },
                    { 4, "A102", 2 },
                    { 5, "A101", 3 },
                    { 6, "A102", 3 },
                    { 7, "A101", 4 },
                    { 8, "A102", 4 },
                    { 9, "A101", 5 },
                    { 10, "A102", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ClassRooms",
                keyColumn: "ClassRoomId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TimeTables",
                keyColumn: "TimeTableId",
                keyValue: 1);
        }
    }
}
