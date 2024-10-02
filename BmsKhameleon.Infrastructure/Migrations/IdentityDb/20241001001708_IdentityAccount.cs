using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmsKhameleon.Infrastructure.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class IdentityAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BankBranch = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    InitialBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateEnrolled = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visibility = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MonthlyWorkingBalance",
                columns: table => new
                {
                    MonthlyWorkingBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyWorkingBalance", x => x.MonthlyWorkingBalanceId);
                    table.ForeignKey(
                        name: "FK_MonthlyWorkingBalance_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TransactionMedium = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    CashTransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Payee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChequeBankName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ApplicationUserId",
                table: "Account",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyWorkingBalance_AccountId",
                table: "MonthlyWorkingBalance",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyWorkingBalance");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
