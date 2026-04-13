using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Documents.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "documents");

            migrationBuilder.CreateTable(
                name: "documents",
                schema: "documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    descriptive_no = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    kind = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    integrated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revision = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "integration_checkpoints",
                schema: "documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_processed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_integration_checkpoints", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "document_items",
                schema: "documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    article_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    tax_rate = table.Column<decimal>(type: "numeric(9,4)", nullable: false),
                    net_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_document_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_document_items_documents_document_id",
                        column: x => x.document_id,
                        principalSchema: "documents",
                        principalTable: "documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_document_items_document_id",
                schema: "documents",
                table: "document_items",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_descriptive_no",
                schema: "documents",
                table: "documents",
                column: "descriptive_no");

            migrationBuilder.CreateIndex(
                name: "ix_documents_integrated_at_utc",
                schema: "documents",
                table: "documents",
                column: "integrated_at_utc");

            migrationBuilder.CreateIndex(
                name: "ix_documents_kind",
                schema: "documents",
                table: "documents",
                column: "kind");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document_items",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "integration_checkpoints",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "documents",
                schema: "documents");
        }
    }
}
