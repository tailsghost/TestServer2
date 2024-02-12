using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kurskcartuning.Server_v2.Migrations
{
    /// <inheritdoc />
    public partial class Delete_listOfPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfWorksPrices");

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "ListOfWorks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ListOfWorks");

            migrationBuilder.CreateTable(
                name: "ListOfWorksPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListOfWorksId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfWorksPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListOfWorksPrices_ListOfWorks_ListOfWorksId",
                        column: x => x.ListOfWorksId,
                        principalTable: "ListOfWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListOfWorksPrices_ListOfWorksId",
                table: "ListOfWorksPrices",
                column: "ListOfWorksId");
        }
    }
}
