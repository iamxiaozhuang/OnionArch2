using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20230201 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityChangedAuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeType = table.Column<string>(type: "text", nullable: false),
                    OriginalValues = table.Column<string>(type: "text", nullable: false),
                    CurrentValues = table.Column<string>(type: "text", nullable: false),
                    OccurredBy = table.Column<string>(type: "text", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChangedAuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductCode = table.Column<string>(type: "text", nullable: false),
                    InventoryAmount = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangedAuditLog_Id_TenantId",
                table: "EntityChangedAuditLog",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangedAuditLog_TenantId",
                table: "EntityChangedAuditLog",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventory_Id_TenantId",
                table: "ProductInventory",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventory_TenantId",
                table: "ProductInventory",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityChangedAuditLog");

            migrationBuilder.DropTable(
                name: "ProductInventory");
        }
    }
}
