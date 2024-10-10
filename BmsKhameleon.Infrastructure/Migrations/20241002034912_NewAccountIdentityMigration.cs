using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmsKhameleon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewAccountIdentityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Accounts");
        }
    }
}
