using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrationtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionCategories",
                columns: table => new
                {
                    categoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    categoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transactionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategories", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "TotalTransactionAmounts",
                columns: table => new
                {
                    totalTransactionAmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    totalExpence = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    lastTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalTransactionAmounts", x => x.totalTransactionAmountId);
                    table.ForeignKey(
                        name: "FK_TotalTransactionAmounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    transactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transactionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transactionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    transactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    transactionCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transactionType = table.Column<int>(type: "int", nullable: false),
                    totalTransactionAmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.transactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_TotalTransactionAmounts_totalTransactionAmountId",
                        column: x => x.totalTransactionAmountId,
                        principalTable: "TotalTransactionAmounts",
                        principalColumn: "totalTransactionAmountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionCategories_transactionCategoryId",
                        column: x => x.transactionCategoryId,
                        principalTable: "TransactionCategories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotalTransactionAmounts_UserId",
                table: "TotalTransactionAmounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_totalTransactionAmountId",
                table: "Transactions",
                column: "totalTransactionAmountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_transactionCategoryId",
                table: "Transactions",
                column: "transactionCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TotalTransactionAmounts");

            migrationBuilder.DropTable(
                name: "TransactionCategories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
