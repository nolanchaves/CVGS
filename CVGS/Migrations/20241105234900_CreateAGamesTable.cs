using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class CreateAGamesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageSupport = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    CoverImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DownloadSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameID);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameID", "Category", "CoverImageURL", "Description", "DownloadSize", "LanguageSupport", "Platform", "Price", "Rating", "Title" },
                values: new object[,]
                {
                    { 1, "Sandbox", "/images/minecraft.jpg", "A sandbox game with endless possibilities.", 1500000000L, "English, Spanish", "PC", 26.95m, 9.1000003814697266, "Minecraft" },
                    { 2, "RPG", "/images/witcher3.jpg", "An open-world RPG with a gripping story.", 3500000000L, "English, French", "PC", 39.99m, 9.3999996185302734, "The Witcher 3: Wild Hunt" },
                    { 3, "RPG", "/images/cyberpunk2077.jpg", "A futuristic RPG with a vast city to explore.", 5000000000L, "English, Spanish", "PC", 59.99m, 8.5, "Cyberpunk 2077" },
                    { 4, "Shooter", "/images/halflife_alyx.jpg", "A VR game set in the Half-Life universe.", 2000000000L, "English, Russian", "PC", 59.99m, 9.0, "Half-Life: Alyx" },
                    { 5, "Action", "/images/gtav.jpg", "A sandbox game with open-world action.", 6500000000L, "English, French", "PC", 29.99m, 9.3000001907348633, "Grand Theft Auto V" },
                    { 6, "Shooter", "/images/halo_infinite.jpg", "A first-person shooter set in a sci-fi universe.", 6000000000L, "English, French", "Xbox Series S/X", 59.99m, 9.0, "Halo Infinite" },
                    { 7, "Racing", "/images/forza_horizon5.jpg", "A racing game with beautiful open-world environments.", 8000000000L, "English, Italian", "Xbox Series S/X", 49.99m, 9.3000001907348633, "Forza Horizon 5" },
                    { 8, "Sports", "/images/fifa_23.jpg", "The latest installment in the FIFA soccer series.", 2500000000L, "English, Spanish", "Xbox Series S/X", 59.99m, 8.6999998092651367, "FIFA 23" },
                    { 9, "RPG", "/images/the_ascent.jpg", "A cyberpunk RPG set in a dystopian world.", 5000000000L, "English, Japanese", "Xbox Series S/X", 39.99m, 8.0, "The Ascent" },
                    { 10, "Platformer", "/images/psychonauts2.jpg", "A quirky platformer with mind-bending puzzles.", 4500000000L, "English, French", "Xbox Series S/X", 59.99m, 8.8999996185302734, "Psychonauts 2" },
                    { 11, "Sports", "/images/fifa_23.jpg", "The latest installment in the FIFA soccer series.", 2500000000L, "English, Spanish", "Xbox One", 59.99m, 8.6999998092651367, "FIFA 23" },
                    { 12, "RPG", "/images/skyrim.jpg", "A fantasy RPG with an open-world to explore.", 1200000000L, "English, German", "Xbox One", 39.99m, 9.6000003814697266, "The Elder Scrolls V: Skyrim" },
                    { 13, "Sandbox", "/images/minecraft.jpg", "A sandbox game with endless possibilities.", 1500000000L, "English, Spanish", "Xbox One", 26.95m, 9.1000003814697266, "Minecraft" },
                    { 14, "Shooter", "/images/cod_mw.jpg", "A first-person shooter with intense multiplayer.", 6000000000L, "English, Russian", "Xbox One", 59.99m, 8.8000001907348633, "Call of Duty: Modern Warfare" },
                    { 15, "Racing", "/images/forza_horizon4.jpg", "A racing game with beautiful open-world environments.", 6000000000L, "English, Italian", "Xbox One", 49.99m, 9.1000003814697266, "Forza Horizon 4" },
                    { 16, "Action", "/images/spider_man_miles_morales.jpg", "A superhero game with incredible web-slinging mechanics.", 500000000L, "English, German", "PlayStation 5", 59.99m, 9.3000001907348633, "Spider-Man: Miles Morales" },
                    { 17, "Action/RPG", "/images/demons_souls.jpg", "A remake of the classic action RPG with amazing visuals.", 1000000000L, "English, Russian", "PlayStation 5", 59.99m, 9.5, "Demon's Souls" },
                    { 18, "RPG", "/images/final_fantasy_xvi.jpg", "A high fantasy RPG with intense combat.", 1500000000L, "English, Japanese", "PlayStation 5", 69.99m, 9.1000003814697266, "Final Fantasy XVI" },
                    { 19, "Action", "/images/ratchet_and_clank.jpg", "A visually stunning action-adventure platformer.", 4500000000L, "English, French", "PlayStation 5", 69.99m, 9.0, "Ratchet & Clank: Rift Apart" },
                    { 20, "Shooter", "/images/returnal.jpg", "A roguelike third-person shooter with sci-fi elements.", 1500000000L, "English, Spanish", "PlayStation 5", 59.99m, 8.8000001907348633, "Returnal" },
                    { 21, "Action/Adventure", "/images/uncharted_4.jpg", "A treasure-hunting adventure with stunning visuals.", 500000000L, "English, French", "PlayStation 4", 39.99m, 9.0, "Uncharted 4: A Thief's End" },
                    { 22, "Action/Adventure", "/images/last_of_us2.jpg", "A gripping action adventure with deep storytelling.", 5000000000L, "English, Spanish", "PlayStation 4", 59.99m, 9.5, "The Last of Us Part II" },
                    { 23, "Action", "/images/ghost_of_tsushima.jpg", "An open-world action game set in feudal Japan.", 4500000000L, "English, Russian", "PlayStation 4", 49.99m, 9.1999998092651367, "Ghost of Tsushima" },
                    { 24, "RPG", "/images/bloodborne.jpg", "A gothic action RPG with intense combat mechanics.", 2500000000L, "English, French", "PlayStation 4", 39.99m, 9.1000003814697266, "Bloodborne" },
                    { 25, "Racing", "/images/gran_turismo_sport.jpg", "A racing simulation with lifelike graphics.", 4000000000L, "English, German", "PlayStation 4", 29.99m, 8.5, "Gran Turismo Sport" },
                    { 26, "Adventure", "/images/zelda_breath_wild.jpg", "An open-world adventure in the land of Hyrule.", 1500000000L, "English, French", "Nintendo Switch", 59.99m, 9.6999998092651367, "The Legend of Zelda: Breath of the Wild" },
                    { 27, "Simulation", "/images/animal_crossing.jpg", "A life-simulation game where you build your island paradise.", 5000000000L, "English, Spanish", "Nintendo Switch", 59.99m, 9.3000001907348633, "Animal Crossing: New Horizons" },
                    { 28, "Platformer", "/images/super_mario_odyssey.jpg", "A 3D Mario platformer with a globe-trotting adventure.", 3000000000L, "English, Japanese", "Nintendo Switch", 59.99m, 9.5, "Super Mario Odyssey" },
                    { 29, "Racing", "/images/mario_kart8.jpg", "A fun kart-racing game featuring Nintendo characters.", 5000000000L, "English, Italian", "Nintendo Switch", 59.99m, 9.1999998092651367, "Mario Kart 8 Deluxe" },
                    { 30, "Shooter", "/images/splatoon3.jpg", "A colorful third-person shooter where ink is your weapon.", 2500000000L, "English, French", "Nintendo Switch", 59.99m, 8.8000001907348633, "Splatoon 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
