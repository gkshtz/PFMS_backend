using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "transactionType",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "transactionType",
                table: "TransactionCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"),
                column: "transactionType",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"),
                column: "transactionType",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0f1a7152-3c07-4ddc-a370-06ad886995d4"),
                column: "transactionType",
                value: "Income");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("55460ca7-9ea9-4576-b067-18c72d925456"),
                column: "transactionType",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("92ca68b2-e05b-40cf-981b-5abfea29a8c2"),
                column: "transactionType",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("a2d7aa11-24f2-44c2-85a1-787c95afc34d"),
                column: "transactionType",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("bf295b65-d41b-4684-96d5-0a674aa96eb6"),
                column: "transactionType",
                value: "Income");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("c7b57c96-7034-44de-89aa-2b45323d82cd"),
                column: "transactionType",
                value: "Expense");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("da49303b-843a-4b6e-acf8-4caf75043afb"),
                column: "transactionType",
                value: "Income");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "transactionType",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "transactionType",
                table: "TransactionCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"),
                column: "transactionType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"),
                column: "transactionType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("0f1a7152-3c07-4ddc-a370-06ad886995d4"),
                column: "transactionType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("55460ca7-9ea9-4576-b067-18c72d925456"),
                column: "transactionType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("92ca68b2-e05b-40cf-981b-5abfea29a8c2"),
                column: "transactionType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("a2d7aa11-24f2-44c2-85a1-787c95afc34d"),
                column: "transactionType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("bf295b65-d41b-4684-96d5-0a674aa96eb6"),
                column: "transactionType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("c7b57c96-7034-44de-89aa-2b45323d82cd"),
                column: "transactionType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "categoryId",
                keyValue: new Guid("da49303b-843a-4b6e-acf8-4caf75043afb"),
                column: "transactionType",
                value: 1);
        }
    }
}
