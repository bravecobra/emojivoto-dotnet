using EmojiSvc.Persistence.Impl;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmojiSvc.Migrations
{
    public partial class Populate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Emojies;");
            foreach (var emoji in EmojiDefinitions.Top100Emojis)
            {
                migrationBuilder.InsertData("Emojies", columns: new[] { "Shortcode", "Unicode" }, values: new object[]
                {
                    emoji,
                    EmojiDefinitions.CodeMap[emoji]
                });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Emojies;");
        }
    }
}
