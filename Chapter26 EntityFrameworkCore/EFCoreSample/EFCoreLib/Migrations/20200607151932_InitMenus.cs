using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreLib.Migrations
{
    public partial class InitMenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mc");

            migrationBuilder.CreateTable(
                name: "MenuCards",
                schema: "mc",
                columns: table => new
                {
                    MenuCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCards", x => x.MenuCardId);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "mc",
                columns: table => new
                {
                    MenuId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(maxLength: 120, nullable: true),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    MenuCardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_Menus_MenuCards_MenuCardId",
                        column: x => x.MenuCardId,
                        principalSchema: "mc",
                        principalTable: "MenuCards",
                        principalColumn: "MenuCardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuCardId",
                schema: "mc",
                table: "Menus",
                column: "MenuCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus",
                schema: "mc");

            migrationBuilder.DropTable(
                name: "MenuCards",
                schema: "mc");
        }
    }
}
