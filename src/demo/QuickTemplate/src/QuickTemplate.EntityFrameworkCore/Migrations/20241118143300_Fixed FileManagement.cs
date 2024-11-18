using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickTemplate.Migrations
{
    /// <inheritdoc />
    public partial class FixedFileManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileManagementFolderFiles");

            migrationBuilder.DropIndex(
                name: "IX_FileManagementFiles_FileType",
                table: "FileManagementFiles");

            migrationBuilder.DropIndex(
                name: "IX_FileManagementFiles_Hash",
                table: "FileManagementFiles");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "FileManagementFiles");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "FileManagementFiles");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "FileManagementFiles");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "FileManagementFiles");

            migrationBuilder.AddColumn<string>(
                name: "AllowedFileTypes",
                table: "FileManagementFolders",
                type: "varchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "FileManagementFolders",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "MaxFileSize",
                table: "FileManagementFolders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "FileManagementFiles",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldCollation: "ascii_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "FileInfoBaseId",
                table: "FileManagementFiles",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "FileManagementFiles",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

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

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FileInfoBaseId",
                table: "FileManagementFiles",
                column: "FileInfoBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagementFiles_FolderId_Hash",
                table: "FileManagementFiles",
                columns: new[] { "FolderId", "Hash" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_FileManagementFiles_FileManagementFileInfoBases_FileInfoBase~",
                table: "FileManagementFiles",
                column: "FileInfoBaseId",
                principalTable: "FileManagementFileInfoBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileManagementFiles_FileManagementFolders_FolderId",
                table: "FileManagementFiles",
                column: "FolderId",
                principalTable: "FileManagementFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileManagementFiles_FileManagementFileInfoBases_FileInfoBase~",
                table: "FileManagementFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FileManagementFiles_FileManagementFolders_FolderId",
                table: "FileManagementFiles");

            migrationBuilder.DropTable(
                name: "FileManagementFileInfoBases");

            migrationBuilder.DropIndex(
                name: "IX_FileManagementFiles_FileInfoBaseId",
                table: "FileManagementFiles");

            migrationBuilder.DropIndex(
                name: "IX_FileManagementFiles_FolderId_Hash",
                table: "FileManagementFiles");

            migrationBuilder.DropColumn(
                name: "AllowedFileTypes",
                table: "FileManagementFolders");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "FileManagementFolders");

            migrationBuilder.DropColumn(
                name: "MaxFileSize",
                table: "FileManagementFolders");

            migrationBuilder.DropColumn(
                name: "FileInfoBaseId",
                table: "FileManagementFiles");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "FileManagementFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "FileManagementFiles",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "FileManagementFiles",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "FileManagementFiles",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "FileManagementFiles",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "FileManagementFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "FileManagementFolderFiles",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
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
        }
    }
}
