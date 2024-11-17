using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddFileManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileManagementFilePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TargetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProviderName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "ascii_general_ci"),
                    ProviderKey = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "ascii_general_ci"),
                    CanRead = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CanWrite = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CanDelete = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_FileManagementFilePermissions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileManagementFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Filename = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "gbk_chinese_ci"),
                    MimeType = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "ascii_general_ci"),
                    FileType = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "ascii_general_ci"),
                    Size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hash = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "ascii_general_ci"),
                    Path = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "ascii_general_ci"),
                    IsInheritPermissions = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_FileManagementFiles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileManagementFolderPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TargetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProviderName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "ascii_general_ci"),
                    ProviderKey = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "ascii_general_ci"),
                    CanRead = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CanWrite = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CanDelete = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_FileManagementFolderPermissions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileManagementFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "gbk_chinese_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsInheritPermissions = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    StorageQuota = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    UsedStorage = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
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
                    table.PrimaryKey("PK_FileManagementFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileManagementFolders_FileManagementFolders_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FileManagementFolders",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileManagementVirtualPathPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TargetId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProviderName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "ascii_general_ci"),
                    ProviderKey = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "ascii_general_ci"),
                    CanRead = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CanWrite = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CanDelete = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_FileManagementVirtualPathPermissions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileManagementVirtualPaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FolderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Path = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "ascii_general_ci"),
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
                    table.PrimaryKey("PK_FileManagementVirtualPaths", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileManagementFolderFiles",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementFolderFiles", x => new { x.FolderId, x.FileId });
                    table.ForeignKey(
                        name: "FK_FileManagementFolderFiles_FileManagementFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "FileManagementFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileManagementFolderFiles_FileManagementFolders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "FileManagementFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFilePermissions_TargetId_ProviderName_Provider~",
                table: "FileManagementFilePermissions",
                columns: new[] { "TargetId", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_CreationTime",
                table: "FileManagementFiles",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_Filename",
                table: "FileManagementFiles",
                column: "Filename");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FileType",
                table: "FileManagementFiles",
                column: "FileType");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_Hash",
                table: "FileManagementFiles",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFolderFiles_FileId_FolderId",
                table: "FileManagementFolderFiles",
                columns: new[] { "FileId", "FolderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFolderPermissions_TargetId_ProviderName_Provid~",
                table: "FileManagementFolderPermissions",
                columns: new[] { "TargetId", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFolders_CreationTime",
                table: "FileManagementFolders",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFolders_Name",
                table: "FileManagementFolders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFolders_ParentId",
                table: "FileManagementFolders",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementVirtualPathPermissions_TargetId_ProviderName_P~",
                table: "FileManagementVirtualPathPermissions",
                columns: new[] { "TargetId", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementVirtualPaths_FolderId",
                table: "FileManagementVirtualPaths",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementVirtualPaths_Path",
                table: "FileManagementVirtualPaths",
                column: "Path",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileManagementFilePermissions");

            migrationBuilder.DropTable(
                name: "FileManagementFolderFiles");

            migrationBuilder.DropTable(
                name: "FileManagementFolderPermissions");

            migrationBuilder.DropTable(
                name: "FileManagementVirtualPathPermissions");

            migrationBuilder.DropTable(
                name: "FileManagementVirtualPaths");

            migrationBuilder.DropTable(
                name: "FileManagementFiles");

            migrationBuilder.DropTable(
                name: "FileManagementFolders");
        }
    }
}
