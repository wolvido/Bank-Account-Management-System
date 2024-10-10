using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmsKhameleon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MonthlyBalanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyWorkingBalances",
                columns: table => new
                {
                    MonthlyWorkingBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyWorkingBalances", x => x.MonthlyWorkingBalanceId);
                    table.ForeignKey(
                        name: "FK_MonthlyWorkingBalances_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyWorkingBalances_AccountId_Date",
                table: "MonthlyWorkingBalances",
                columns: new[] { "AccountId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyWorkingBalances");
        }
    }
}
