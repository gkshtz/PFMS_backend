using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class totalmonthlyamounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TotalMonthlyAmounts",
                columns: table => new
                {
                    totalMonthlyAmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    totalExpenceOfMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalIncomeOfMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    totalTransactionAmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalMonthlyAmounts", x => x.totalMonthlyAmountId);
                    table.ForeignKey(
                        name: "FK_TotalMonthlyAmounts_TotalTransactionAmounts_totalTransactionAmountId",
                        column: x => x.totalTransactionAmountId,
                        principalTable: "TotalTransactionAmounts",
                        principalColumn: "totalTransactionAmountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotalMonthlyAmounts_totalTransactionAmountId",
                table: "TotalMonthlyAmounts",
                column: "totalTransactionAmountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalMonthlyAmounts");
        }
    }
}
