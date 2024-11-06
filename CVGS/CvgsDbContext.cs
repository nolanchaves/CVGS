using CVGS.Entities;
using CVGS.Entities.CVGS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CVGS
{
    public class CvgsDbContext : IdentityDbContext<User>
    {
        public CvgsDbContext(DbContextOptions<CvgsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One-to-one relationship between User and Address
            builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            // One-to-one relationship between User and Preference
            builder.Entity<User>()
                .HasOne(u => u.Preferences)
                .WithOne(p => p.User)
                .HasForeignKey<Preference>(p => p.UserId);

            // One-to-one relationship between User and ShippingAddress
            builder.Entity<User>()
                .HasOne(u => u.ShippingAddress)
                .WithOne(s => s.User)
                .HasForeignKey<ShippingAddress>(s => s.UserId);

            builder.Entity<Game>().HasData(
            // PC Games
            new Game { GameID = 1, Title = "Minecraft", Description = "A sandbox game with endless possibilities.", Platform = "PC", Category = "Sandbox", LanguageSupport = "English, Spanish", Price = 26.95m, Rating = 9.1f, CoverImageURL = "/images/minecraft.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 2, Title = "The Witcher 3: Wild Hunt", Description = "An open-world RPG with a gripping story.", Platform = "PC", Category = "RPG", LanguageSupport = "English, French", Price = 39.99m, Rating = 9.4f, CoverImageURL = "/images/witcher3.jpg", DownloadSize = 3500000000 },
            new Game { GameID = 3, Title = "Cyberpunk 2077", Description = "A futuristic RPG with a vast city to explore.", Platform = "PC", Category = "RPG", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = 8.5f, CoverImageURL = "/images/cyberpunk2077.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 4, Title = "Half-Life: Alyx", Description = "A VR game set in the Half-Life universe.", Platform = "PC", Category = "Shooter", LanguageSupport = "English, Russian", Price = 59.99m, Rating = 9.0f, CoverImageURL = "/images/halflife_alyx.jpg", DownloadSize = 2000000000 },
            new Game { GameID = 5, Title = "Grand Theft Auto V", Description = "A sandbox game with open-world action.", Platform = "PC", Category = "Action", LanguageSupport = "English, French", Price = 29.99m, Rating = 9.3f, CoverImageURL = "/images/gtav.jpg", DownloadSize = 6500000000 },

            // Xbox Series S/X Games
            new Game { GameID = 6, Title = "Halo Infinite", Description = "A first-person shooter set in a sci-fi universe.", Platform = "Xbox Series S/X", Category = "Shooter", LanguageSupport = "English, French", Price = 59.99m, Rating = 9.0f, CoverImageURL = "/images/halo_infinite.jpg", DownloadSize = 6000000000 },
            new Game { GameID = 7, Title = "Forza Horizon 5", Description = "A racing game with beautiful open-world environments.", Platform = "Xbox Series S/X", Category = "Racing", LanguageSupport = "English, Italian", Price = 49.99m, Rating = 9.3f, CoverImageURL = "/images/forza_horizon5.jpg", DownloadSize = 8000000000 },
            new Game { GameID = 8, Title = "FIFA 23", Description = "The latest installment in the FIFA soccer series.", Platform = "Xbox Series S/X", Category = "Sports", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = 8.7f, CoverImageURL = "/images/fifa_23.jpg", DownloadSize = 2500000000 },
            new Game { GameID = 9, Title = "The Ascent", Description = "A cyberpunk RPG set in a dystopian world.", Platform = "Xbox Series S/X", Category = "RPG", LanguageSupport = "English, Japanese", Price = 39.99m, Rating = 8.0f, CoverImageURL = "/images/the_ascent.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 10, Title = "Psychonauts 2", Description = "A quirky platformer with mind-bending puzzles.", Platform = "Xbox Series S/X", Category = "Platformer", LanguageSupport = "English, French", Price = 59.99m, Rating = 8.9f, CoverImageURL = "/images/psychonauts2.jpg", DownloadSize = 4500000000 },

            // Xbox One Games
            new Game { GameID = 11, Title = "FIFA 23", Description = "The latest installment in the FIFA soccer series.", Platform = "Xbox One", Category = "Sports", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = 8.7f, CoverImageURL = "/images/fifa_23.jpg", DownloadSize = 2500000000 },
            new Game { GameID = 12, Title = "The Elder Scrolls V: Skyrim", Description = "A fantasy RPG with an open-world to explore.", Platform = "Xbox One", Category = "RPG", LanguageSupport = "English, German", Price = 39.99m, Rating = 9.6f, CoverImageURL = "/images/skyrim.jpg", DownloadSize = 1200000000 },
            new Game { GameID = 13, Title = "Minecraft", Description = "A sandbox game with endless possibilities.", Platform = "Xbox One", Category = "Sandbox", LanguageSupport = "English, Spanish", Price = 26.95m, Rating = 9.1f, CoverImageURL = "/images/minecraft.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 14, Title = "Call of Duty: Modern Warfare III", Description = "A first-person shooter with intense multiplayer.", Platform = "Xbox One", Category = "Shooter", LanguageSupport = "English, Russian", Price = 59.99m, Rating = 8.8f, CoverImageURL = "/images/cod_mw.jpg", DownloadSize = 6000000000 },
            new Game { GameID = 15, Title = "Forza Horizon 4", Description = "A racing game with beautiful open-world environments.", Platform = "Xbox One", Category = "Racing", LanguageSupport = "English, Italian", Price = 49.99m, Rating = 9.1f, CoverImageURL = "/images/forza_horizon4.jpg", DownloadSize = 6000000000 },

            // PlayStation 5 Games
            new Game { GameID = 16, Title = "Spider-Man: Miles Morales", Description = "A superhero game with incredible web-slinging mechanics.", Platform = "PlayStation 5", Category = "Action", LanguageSupport = "English, German", Price = 59.99m, Rating = 9.3f, CoverImageURL = "/images/spider_man_miles_morales.jpg", DownloadSize = 500000000 },
            new Game { GameID = 17, Title = "Demon's Souls", Description = "A remake of the classic action RPG with amazing visuals.", Platform = "PlayStation 5", Category = "Action/RPG", LanguageSupport = "English, Russian", Price = 59.99m, Rating = 9.5f, CoverImageURL = "/images/demons_souls.jpg", DownloadSize = 1000000000 },
            new Game { GameID = 18, Title = "Final Fantasy XVI", Description = "A high fantasy RPG with intense combat.", Platform = "PlayStation 5", Category = "RPG", LanguageSupport = "English, Japanese", Price = 69.99m, Rating = 9.1f, CoverImageURL = "/images/final_fantasy_xvi.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 19, Title = "Ratchet & Clank: Rift Apart", Description = "A visually stunning action-adventure platformer.", Platform = "PlayStation 5", Category = "Action", LanguageSupport = "English, French", Price = 69.99m, Rating = 9.0f, CoverImageURL = "/images/ratchet_and_clank.jpg", DownloadSize = 4500000000 },
            new Game { GameID = 20, Title = "Returnal", Description = "A roguelike third-person shooter with sci-fi elements.", Platform = "PlayStation 5", Category = "Shooter", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = 8.8f, CoverImageURL = "/images/returnal.jpg", DownloadSize = 1500000000 },

            // PlayStation 4 Games
            new Game { GameID = 21, Title = "Uncharted 4: A Thief's End", Description = "A treasure-hunting adventure with stunning visuals.", Platform = "PlayStation 4", Category = "Action/Adventure", LanguageSupport = "English, French", Price = 39.99m, Rating = 9.0f, CoverImageURL = "/images/uncharted_4.jpg", DownloadSize = 500000000 },
            new Game { GameID = 22, Title = "The Last of Us Part II", Description = "A gripping action adventure with deep storytelling.", Platform = "PlayStation 4", Category = "Action/Adventure", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = 9.5f, CoverImageURL = "/images/last_of_us2.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 23, Title = "Ghost of Tsushima", Description = "An open-world action game set in feudal Japan.", Platform = "PlayStation 4", Category = "Action", LanguageSupport = "English, Russian", Price = 49.99m, Rating = 9.2f, CoverImageURL = "/images/ghost_of_tsushima.jpg", DownloadSize = 4500000000 },
            new Game { GameID = 24, Title = "Bloodborne", Description = "A gothic action RPG with intense combat mechanics.", Platform = "PlayStation 4", Category = "RPG", LanguageSupport = "English, French", Price = 39.99m, Rating = 9.1f, CoverImageURL = "/images/bloodborne.jpg", DownloadSize = 2500000000 },
            new Game { GameID = 25, Title = "Gran Turismo Sport", Description = "A racing simulation with lifelike graphics.", Platform = "PlayStation 4", Category = "Racing", LanguageSupport = "English, German", Price = 29.99m, Rating = 8.5f, CoverImageURL = "/images/gran_turismo_sport.jpg", DownloadSize = 4000000000 },

            // Nintendo Switch Games
            new Game { GameID = 26, Title = "The Legend of Zelda: Breath of the Wild", Description = "An open-world adventure in the land of Hyrule.", Platform = "Nintendo Switch", Category = "Adventure", LanguageSupport = "English, French", Price = 59.99m, Rating = 9.7f, CoverImageURL = "/images/zelda_breath_wild.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 27, Title = "Animal Crossing: New Horizons", Description = "A life-simulation game where you build your island paradise.", Platform = "Nintendo Switch", Category = "Simulation", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = 9.3f, CoverImageURL = "/images/animal_crossing.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 28, Title = "Super Mario Odyssey", Description = "A 3D Mario platformer with a globe-trotting adventure.", Platform = "Nintendo Switch", Category = "Platformer", LanguageSupport = "English, Japanese", Price = 59.99m, Rating = 9.5f, CoverImageURL = "/images/super_mario_odyssey.jpg", DownloadSize = 3000000000 },
            new Game { GameID = 29, Title = "Mario Kart 8 Deluxe", Description = "A fun kart-racing game featuring Nintendo characters.", Platform = "Nintendo Switch", Category = "Racing", LanguageSupport = "English, Italian", Price = 59.99m, Rating = 9.2f, CoverImageURL = "/images/mario_kart8.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 30, Title = "Splatoon 3", Description = "A colorful third-person shooter where ink is your weapon.", Platform = "Nintendo Switch", Category = "Shooter", LanguageSupport = "English, French", Price = 59.99m, Rating = 8.8f, CoverImageURL = "/images/splatoon3.jpg", DownloadSize = 2500000000 }
        );
        }


        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Game> Games { get; set; }

    }
}
