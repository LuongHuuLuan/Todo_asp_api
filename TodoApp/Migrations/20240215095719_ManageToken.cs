using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class ManageToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutstandingTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Jti = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutstandingTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutstandingTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BlacklistedTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BlacklistedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlacklistedTokens_OutstandingTokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "OutstandingTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_TokenId",
                table: "BlacklistedTokens",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingTokens_AccountId",
                table: "OutstandingTokens",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedTokens");

            migrationBuilder.DropTable(
                name: "OutstandingTokens");
        }
    }
}
