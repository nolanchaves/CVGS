using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class SeedGameEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Date", "Description", "Location", "MaxRegistrations", "Name" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 12, 15), "Compete in an intense battle royale tournament and win amazing prizes!", "eSports Arena, Los Angeles", 200, "Battle Royale Championship" },
                    { 2, new DateOnly(2024, 12, 20), "Enjoy classic games from the 80s and 90s on original consoles.", "Pixel Café, New York", 50, "Retro Gaming Night" },
                    { 3, new DateOnly(2024, 12, 22), "Discover the best indie games from up-and-coming developers.", "GameDev Convention Center, San Francisco", 100, "Indie Game Showcase" },
                    { 4, new DateOnly(2024, 12, 25), "Show off your best gaming-themed cosplay and win exclusive rewards.", "Anime Expo Hall, Chicago", 75, "Cosplay Contest" },
                    { 5, new DateOnly(2025, 1, 5), "Join fellow summoners for a day of friendly matches and discussions.", "Riot HQ, Seattle", 150, "League of Legends Meetup" },
                    { 6, new DateOnly(2025, 1, 10), "Showcase your creativity in a timed building challenge.", "Creative Zone, Houston", 100, "Minecraft Build-Off" },
                    { 7, new DateOnly(2025, 1, 15), "Immerse yourself in the latest VR games and experiences.", "Virtual Arena, Boston", 80, "VR Experience Day" },
                    { 8, new DateOnly(2025, 1, 18), "Learn the art of speedrunning from professional gamers.", "Streamer Studio, Denver", 60, "Speedrunning Workshop" },
                    { 9, new DateOnly(2025, 1, 22), "Team up and compete in this high-octane FPS competition.", "Blizzard HQ, Irvine", 100, "Overwatch 2 Tournament" },
                    { 10, new DateOnly(2025, 1, 28), "Bring your deck and challenge other trainers to card battles.", "Card Haven, Dallas", 50, "Pokemon Card Battle" },
                    { 11, new DateOnly(2025, 2, 1), "Join a thrilling one-shot campaign hosted by experienced DMs.", "Adventure Guild, Portland", 40, "Dungeons & Dragons Campaign Night" },
                    { 12, new DateOnly(2025, 2, 5), "Team up with a friend and aim for Victory Royale!", "Battle Grounds, Miami", 100, "Fortnite Duo Challenge" },
                    { 13, new DateOnly(2025, 2, 10), "Dive deep into the lore of Elden Ring with fellow fans.", "Lore Hall, Philadelphia", 70, "Elden Ring Lore Discussion" },
                    { 14, new DateOnly(2025, 2, 14), "Compete in a 1v1 Smash Bros. Ultimate tournament.", "Nintendo Center, Atlanta", 80, "Smash Bros. Ultimate Showdown" },
                    { 15, new DateOnly(2025, 2, 18), "Learn tips and tricks from professional game developers.", "Tech Hub, Austin", 100, "Game Development Seminar" },
                    { 16, new DateOnly(2025, 2, 20), "Explore a variety of board games with fellow enthusiasts.", "Tabletop Tavern, Detroit", 50, "Board Game Bonanza" },
                    { 17, new DateOnly(2025, 2, 25), "Show off your aerial skills in this car soccer tournament.", "Arena Dome, Phoenix", 120, "Rocket League Championship" },
                    { 18, new DateOnly(2025, 2, 28), "Brace yourself for a night of spooky gaming experiences.", "Haunted Hub, Orlando", 40, "Horror Game Marathon" },
                    { 19, new DateOnly(2025, 3, 1), "Show your skills in an exciting Street Fighter competition.", "Arcade Central, San Diego", 80, "Street Fighter V Exhibition" },
                    { 20, new DateOnly(2025, 3, 5), "Relax and share island tips with other Animal Crossing players.", "Villager Café, Charlotte", 30, "Cozy Animal Crossing Meet-Up" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 20);
        }
    }
}
