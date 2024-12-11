using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Data.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddDataPipelineIdToWorkspaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "DataPipelineId",
                schema: "app",
                table: "Workspaces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataFlowId",
                schema: "app",
                table: "DataPipelines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_DataPipelineId",
                schema: "app",
                table: "Workspaces",
                column: "DataPipelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_DataPipelines_DataPipelineId",
                schema: "app",
                table: "Workspaces",
                column: "DataPipelineId",
                principalSchema: "app",
                principalTable: "DataPipelines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_DataPipelines_DataPipelineId",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_DataPipelineId",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "DataPipelineId",
                schema: "app",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "DataFlowId",
                schema: "app",
                table: "DataPipelines");

            migrationBuilder.CreateTable(
                name: "DataPipelineBaseEntity",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSource_DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataSource_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSource_IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DataSource_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataTransformation_DataPipelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataTransformation_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionOrder = table.Column<int>(type: "int", nullable: true),
                    DataTransformation_IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DataTransformation_Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    DataSourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}
