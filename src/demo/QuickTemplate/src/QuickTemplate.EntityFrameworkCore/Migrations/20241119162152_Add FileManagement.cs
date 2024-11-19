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
                name: "FileManagementFileInfoBases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MimeType = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Hash = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagementFileInfoBases", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                    IsStatic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowedFileTypes = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StorageQuota = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    UsedStorage = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    MaxFileSize = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
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
                name: "FileManagementFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Filename = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "gbk_chinese_ci"),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hash = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsInheritPermissions = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FolderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileInfoBaseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
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
                    table.ForeignKey(
                        name: "FK_FileManagementFiles_FileManagementFileInfoBases_FileInfoBase~",
                        column: x => x.FileInfoBaseId,
                        principalTable: "FileManagementFileInfoBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileManagementFiles_FileManagementFolders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "FileManagementFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_CreationTime",
                table: "FileManagementFileInfoBases",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_FileType",
                table: "FileManagementFileInfoBases",
                column: "FileType");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFileInfoBases_Hash",
                table: "FileManagementFileInfoBases",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFilePermissions_TargetId_ProviderName_Provider~",
                table: "FileManagementFilePermissions",
                columns: new[] { "TargetId", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_CreationTime",
                table: "FileManagementFiles",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FileInfoBaseId",
                table: "FileManagementFiles",
                column: "FileInfoBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FolderId_FileInfoBaseId",
                table: "FileManagementFiles",
                columns: new[] { "FolderId", "FileInfoBaseId" });

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FolderId_Filename",
                table: "FileManagementFiles",
                columns: new[] { "FolderId", "Filename" });

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
                name: "FileManagementFiles");

            migrationBuilder.DropTable(
                name: "FileManagementFolderPermissions");

            migrationBuilder.DropTable(
                name: "FileManagementVirtualPathPermissions");

            migrationBuilder.DropTable(
                name: "FileManagementVirtualPaths");

            migrationBuilder.DropTable(
                name: "FileManagementFileInfoBases");

            migrationBuilder.DropTable(
                name: "FileManagementFolders");
        }
    }
}
