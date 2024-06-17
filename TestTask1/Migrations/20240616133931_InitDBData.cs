using Microsoft.EntityFrameworkCore.Migrations;

namespace TestTask1.Migrations
{
    public partial class InitDBData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              table: "Accounts",
              columns: new[] { "Id", "Name"},
              values: new object[,]
              {
                    {1000, "Account1"},
                    {1001, "Account2"},
                    {1002, "Account3"},
                    {1003, "Account4"},
              });

            migrationBuilder.InsertData(
              table: "Contacts",
              columns: new[] { "Id", "FirstName", "LastName", "Email", "AccountId" },
              values: new object[,]
              {
                    {2000, "John", "Smith", "john.smith@gmail.com", 1001},
                    {2001, "Emily", "Johnson", "emily.johnson@gmail.com", 1002},
                    {2002, "Michael", "Johnson", "michael.Johnson@ukr.net", 1002},
                    {2003, "Sarah", "Davis", "sarah.davis@mail.com", 1000},
                    {2004, "David", "Wilson", "david.wilson@gmail.com", 1003},
                    {2005, "Laura", "Martinez", "laura.martinez@gmail.com", 1000},
              });


            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "Name", "Description", "AccountId"},
                values: new object[,]
                {
                    {"Incident1", "Incident1 description", 1001},
                    {"Incident2", "Incident2 description", 1000},
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValues: new object[] { 2000, 2001, 2002, 2003, 2004, 2005 });

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "Name",
                keyValues: new object[] { "Incident1", "Incident2" });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValues: new object[] { 1000, 1001, 1002, 1003 });
        }
    }
}
