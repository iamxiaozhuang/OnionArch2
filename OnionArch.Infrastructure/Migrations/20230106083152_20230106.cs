using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20230106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ProductInventoryDomainEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    AggregateRootId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityJson = table.Column<string>(type: "text", nullable: false),
                    AggregateRootEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    OccurredBy = table.Column<string>(type: "text", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventoryDomainEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInventoryDomainEvents_ProductInventory_AggregateRoot~",
                        column: x => x.AggregateRootEntityId,
                        principalTable: "ProductInventory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventoryDomainEvents_AggregateRootEntityId",
                table: "ProductInventoryDomainEvents",
                column: "AggregateRootEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductInventoryDomainEvents");

            migrationBuilder.DropTable(
                name: "ProductInventory");
        }
    }
}
