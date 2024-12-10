using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Data.Migrations.Application
{
    /// <inheritdoc />
    public partial class dataPipelineMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Pipelines_DataPipelineId",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_DataPipelineId",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pipelines",
                schema: "app",
                table: "Pipelines");

            migrationBuilder.DropColumn(
                name: "DataPipelineId",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.RenameTable(
                name: "Pipelines",
                schema: "app",
                newName: "DataPipelines",
                newSchema: "app");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "app",
                table: "Workspaces",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Workspaces",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "app",
                table: "Workspaces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                schema: "app",
                table: "Workspaces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAy",
                schema: "app",
                table: "Workspaces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataPipelines",
                schema: "app",
                table: "DataPipelines",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DataPipelineBaseEntity",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataSource_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSource_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSource_IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DataSource_DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataTransformation_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataTransformation_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionOrder = table.Column<int>(type: "int", nullable: true),
                    DataTransformation_IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DataTransformation_DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPipelineBaseEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataPipelineBaseEntity_DataPipelines_DataPipelineId",
                        column: x => x.DataPipelineId,
                        principalSchema: "app",
                        principalTable: "DataPipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataPipelineBaseEntity_DataPipelines_DataSource_DataPipelineId",
                        column: x => x.DataSource_DataPipelineId,
                        principalSchema: "app",
                        principalTable: "DataPipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataPipelineBaseEntity_DataPipelines_DataTransformation_DataPipelineId",
                        column: x => x.DataTransformation_DataPipelineId,
                        principalSchema: "app",
                        principalTable: "DataPipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_DataPipelineBaseEntity_DestinationId",
                        column: x => x.DestinationId,
                        principalSchema: "app",
                        principalTable: "DataPipelineBaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_DataPipelineBaseEntity_SourceId",
                        column: x => x.SourceId,
                        principalSchema: "app",
                        principalTable: "DataPipelineBaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_DataPipelines_DataPipelineId",
                        column: x => x.DataPipelineId,
                        principalSchema: "app",
                        principalTable: "DataPipelines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataColumn",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false),
                    DataSourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataColumn_DataPipelineBaseEntity_DataSourceId",
                        column: x => x.DataSourceId,
                        principalSchema: "app",
                        principalTable: "DataPipelineBaseEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_DataPipelineId",
                schema: "app",
                table: "Connections",
                column: "DataPipelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_DestinationId",
                schema: "app",
                table: "Connections",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SourceId",
                schema: "app",
                table: "Connections",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataColumn_DataSourceId",
                schema: "app",
                table: "DataColumn",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPipelineBaseEntity_DataPipelineId",
                schema: "app",
                table: "DataPipelineBaseEntity",
                column: "DataPipelineId",
                unique: true,
                filter: "[DataPipelineId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DataPipelineBaseEntity_DataSource_DataPipelineId",
                schema: "app",
                table: "DataPipelineBaseEntity",
                column: "DataSource_DataPipelineId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPipelineBaseEntity_DataTransformation_DataPipelineId",
                schema: "app",
                table: "DataPipelineBaseEntity",
                column: "DataTransformation_DataPipelineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DataColumn",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DataPipelineBaseEntity",
                schema: "app");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataPipelines",
                schema: "app",
                table: "DataPipelines");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "LastModifiedAy",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.RenameTable(
                name: "DataPipelines",
                schema: "app",
                newName: "Pipelines",
                newSchema: "app");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "app",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "DataPipelineId",
                schema: "app",
                table: "Workspaces",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pipelines",
                schema: "app",
                table: "Pipelines",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_DataPipelineId",
                schema: "app",
                table: "Workspaces",
                column: "DataPipelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Pipelines_DataPipelineId",
                schema: "app",
                table: "Workspaces",
                column: "DataPipelineId",
                principalSchema: "app",
                principalTable: "Pipelines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
