using Microsoft.EntityFrameworkCore.Migrations;
using Persistence.Configurations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StratgyPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccount_AccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount");

            migrationBuilder.DropColumn(
                name: "BalanceCeiling",
                table: "BankAccount");

            migrationBuilder.DropColumn(
                name: "OverdraftLimit",
                table: "BankAccount");

            migrationBuilder.RenameTable(
                name: "BankAccount",
                newName: "BankAccounts");

            migrationBuilder.AddColumn<AccountBehaviorData>(
                name: "AccountBehavior",
                table: "BankAccounts",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "AccountBehavior",
                table: "BankAccounts");

            migrationBuilder.RenameTable(
                name: "BankAccounts",
                newName: "BankAccount");

            migrationBuilder.AddColumn<float>(
                name: "BalanceCeiling",
                table: "BankAccount",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "OverdraftLimit",
                table: "BankAccount",
                type: "real",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccount_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "BankAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
