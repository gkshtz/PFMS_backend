using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PFMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class dataseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionCategories",
                columns: new[] { "categoryId", "categoryName", "transactionType" },
                values: new object[,]
                {
                    { new Guid("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"), "Travel", 0 },
                    { new Guid("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"), "Shopping", 0 },
                    { new Guid("0f1a7152-3c07-4ddc-a370-06ad886995d4"), "Others", 1 },
                    { new Guid("55460ca7-9ea9-4576-b067-18c72d925456"), "Rent and Bills", 0 },
                    { new Guid("92ca68b2-e05b-40cf-981b-5abfea29a8c2"), "Others", 0 },
                    { new Guid("a2d7aa11-24f2-44c2-85a1-787c95afc34d"), "Food", 0 },
                    { new Guid("bf295b65-d41b-4684-96d5-0a674aa96eb6"), "Salary", 1 },
                    { new Guid("c7b57c96-7034-44de-89aa-2b45323d82cd"), "Home Necessities", 0 },
                    { new Guid("da49303b-843a-4b6e-acf8-4caf75043afb"), "Sale", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0f1a7152-3c07-4ddc-a370-06ad886995d4"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("55460ca7-9ea9-4576-b067-18c72d925456"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("92ca68b2-e05b-40cf-981b-5abfea29a8c2"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("a2d7aa11-24f2-44c2-85a1-787c95afc34d"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("bf295b65-d41b-4684-96d5-0a674aa96eb6"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("c7b57c96-7034-44de-89aa-2b45323d82cd"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("da49303b-843a-4b6e-acf8-4caf75043afb"));
        }
    }
}
