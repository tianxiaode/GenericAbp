using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuManagementMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: false, collation: "ascii_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EntityVersion = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "gbk_chinese_ci"),
                    Icon = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "ascii_general_ci"),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    IsStatic = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Router = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "ascii_general_ci"),
                    GroupName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "ascii_general_ci"),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuManagementMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuManagementMenus_MenuManagementMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MenuManagementMenus",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MenuManagementMenus_Code",
                table: "MenuManagementMenus",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_MenuManagementMenus_GroupName_Order",
                table: "MenuManagementMenus",
                columns: new[] { "GroupName", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_MenuManagementMenus_Name",
                table: "MenuManagementMenus",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MenuManagementMenus_ParentId",
                table: "MenuManagementMenus",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuManagementMenus");
        }
    }
}
