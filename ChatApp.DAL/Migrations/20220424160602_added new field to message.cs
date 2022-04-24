using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.DAL.Migrations
{
    public partial class addednewfieldtomessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeletedOnlyFromMyChat",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOnlyFromMyChat",
                table: "Messages");
        }
    }
}
