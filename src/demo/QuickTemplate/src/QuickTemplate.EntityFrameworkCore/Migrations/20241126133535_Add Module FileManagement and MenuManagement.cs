using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleFileManagementandMenuManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileManagementFileInfoBases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Hash = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RetentionPolicy = table.Column<int>(type: "int", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementFileInfoBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileManagementResourcePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Permissions = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementResourcePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileManagementResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FileInfoBaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsStatic = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(223)", maxLength: 223, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileManagementResources_FileManagementResources_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FileManagementResources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuManagementMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityVersion = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Icon = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsStatic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Router = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuManagementMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuManagementMenus_MenuManagementMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MenuManagementMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_CreationTime",
                table: "FileManagementFileInfoBases",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_ExpireAt_RetentionPolicy",
                table: "FileManagementFileInfoBases",
                columns: new[] { "ExpireAt", "RetentionPolicy" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_Extension",
                table: "FileManagementFileInfoBases",
                column: "Extension");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_Hash",
                table: "FileManagementFileInfoBases",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementResourcePermissions_ResourceId_Permissions_ProviderName_ProviderKey",
                table: "FileManagementResourcePermissions",
                columns: new[] { "ResourceId", "Permissions", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementResources_Code",
                table: "FileManagementResources",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementResources_CreationTime",
                table: "FileManagementResources",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementResources_Name",
                table: "FileManagementResources",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementResources_ParentId",
                table: "FileManagementResources",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuManagementMenus_Code",
                table: "MenuManagementMenus",
                column: "Code");

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
                name: "FileManagementFileInfoBases");

            migrationBuilder.DropTable(
                name: "FileManagementResourcePermissions");

            migrationBuilder.DropTable(
                name: "FileManagementResources");

            migrationBuilder.DropTable(
                name: "MenuManagementMenus");
        }
    }
}
