using CVGS.Entities;
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
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Enable cascade delete


            // One-to-one relationship between User and Preference
            builder.Entity<User>()
                .HasOne(u => u.Preferences)
                .WithOne(p => p.User)
                .HasForeignKey<Preference>(p => p.UserId);

            // One-to-one relationship between User and ShippingAddress
            builder.Entity<User>()
                .HasOne(u => u.ShippingAddress)
                .WithOne(s => s.User)
                .HasForeignKey<ShippingAddress>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Enable cascade delete

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Game>().HasData(
            // PC Games
            new Game { GameID = 1, Title = "Minecraft", Description = "A sandbox game with endless possibilities.", Platform = "PC", Category = "Sandbox", LanguageSupport = "English, Spanish", Price = 26.95m, Rating = null, CoverImageURL = "/images/minecraft.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 2, Title = "The Witcher 3: Wild Hunt", Description = "An open-world RPG with a gripping story.", Platform = "PC", Category = "RPG", LanguageSupport = "English, French", Price = 39.99m, Rating = null, CoverImageURL = "/images/witcher3.jpg", DownloadSize = 3500000000 },
            new Game { GameID = 3, Title = "Cyberpunk 2077", Description = "A futuristic RPG with a vast city to explore.", Platform = "PC", Category = "RPG", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = null, CoverImageURL = "/images/cyberpunk2077.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 4, Title = "Half-Life: Alyx", Description = "A VR game set in the Half-Life universe.", Platform = "PC", Category = "Shooter", LanguageSupport = "English, Russian", Price = 59.99m, Rating = null, CoverImageURL = "/images/halflife_alyx.jpg", DownloadSize = 2000000000 },
            new Game { GameID = 5, Title = "Grand Theft Auto V", Description = "A sandbox game with open-world action.", Platform = "PC", Category = "Action", LanguageSupport = "English, French", Price = 29.99m, Rating = null, CoverImageURL = "/images/gtav.jpg", DownloadSize = 6500000000 },

            // Xbox Series S/X Games
            new Game { GameID = 6, Title = "Halo Infinite", Description = "A first-person shooter set in a sci-fi universe.", Platform = "Xbox Series S/X", Category = "Shooter", LanguageSupport = "English, French", Price = 59.99m, Rating = null, CoverImageURL = "/images/halo_infinite.jpg", DownloadSize = 6000000000 },
            new Game { GameID = 7, Title = "Forza Horizon 5", Description = "A racing game with beautiful open-world environments.", Platform = "Xbox Series S/X", Category = "Racing", LanguageSupport = "English, Italian", Price = 49.99m, Rating = null, CoverImageURL = "/images/forza_horizon5.jpg", DownloadSize = 8000000000 },
            new Game { GameID = 8, Title = "FIFA 23", Description = "The latest installment in the FIFA soccer series.", Platform = "Xbox Series S/X", Category = "Sports", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = null, CoverImageURL = "/images/fifa_23.jpg", DownloadSize = 2500000000 },
            new Game { GameID = 9, Title = "The Ascent", Description = "A cyberpunk RPG set in a dystopian world.", Platform = "Xbox Series S/X", Category = "RPG", LanguageSupport = "English, Japanese", Price = 39.99m, Rating = null, CoverImageURL = "/images/the_ascent.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 10, Title = "Psychonauts 2", Description = "A quirky platformer with mind-bending puzzles.", Platform = "Xbox Series S/X", Category = "Platformer", LanguageSupport = "English, French", Price = 59.99m, Rating = null, CoverImageURL = "/images/psychonauts2.jpg", DownloadSize = 4500000000 },

            // Xbox One Games
            new Game { GameID = 11, Title = "FIFA 23", Description = "The latest installment in the FIFA soccer series.", Platform = "Xbox One", Category = "Sports", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = null, CoverImageURL = "/images/fifa_23.jpg", DownloadSize = 2500000000 },
            new Game { GameID = 12, Title = "The Elder Scrolls V: Skyrim", Description = "A fantasy RPG with an open-world to explore.", Platform = "Xbox One", Category = "RPG", LanguageSupport = "English, German", Price = 39.99m, Rating = null, CoverImageURL = "/images/skyrim.jpg", DownloadSize = 1200000000 },
            new Game { GameID = 13, Title = "Minecraft", Description = "A sandbox game with endless possibilities.", Platform = "Xbox One", Category = "Sandbox", LanguageSupport = "English, Spanish", Price = 26.95m, Rating = null, CoverImageURL = "/images/minecraft.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 14, Title = "Call of Duty: Modern Warfare III", Description = "A first-person shooter with intense multiplayer.", Platform = "Xbox One", Category = "Shooter", LanguageSupport = "English, Russian", Price = 59.99m, Rating = null, CoverImageURL = "/images/cod_mw.jpg", DownloadSize = 6000000000 },
            new Game { GameID = 15, Title = "Forza Horizon 4", Description = "A racing game with beautiful open-world environments.", Platform = "Xbox One", Category = "Racing", LanguageSupport = "English, Italian", Price = 49.99m, Rating = null, CoverImageURL = "/images/forza_horizon4.jpg", DownloadSize = 6000000000 },

            // PlayStation 5 Games
            new Game { GameID = 16, Title = "Spider-Man: Miles Morales", Description = "A superhero game with incredible web-slinging mechanics.", Platform = "PlayStation 5", Category = "Action", LanguageSupport = "English, German", Price = 59.99m, Rating = null, CoverImageURL = "/images/spider_man_miles_morales.jpg", DownloadSize = 500000000 },
            new Game { GameID = 17, Title = "Demon's Souls", Description = "A remake of the classic action RPG with amazing visuals.", Platform = "PlayStation 5", Category = "Action/RPG", LanguageSupport = "English, Russian", Price = 59.99m, Rating = null, CoverImageURL = "/images/demons_souls.jpg", DownloadSize = 1000000000 },
            new Game { GameID = 18, Title = "Final Fantasy XVI", Description = "A high fantasy RPG with intense combat.", Platform = "PlayStation 5", Category = "RPG", LanguageSupport = "English, Japanese", Price = 69.99m, Rating = null, CoverImageURL = "/images/final_fantasy_xvi.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 19, Title = "Ratchet & Clank: Rift Apart", Description = "A visually stunning action-adventure platformer.", Platform = "PlayStation 5", Category = "Action", LanguageSupport = "English, French", Price = 69.99m, Rating = null, CoverImageURL = "/images/ratchet_and_clank.jpg", DownloadSize = 4500000000 },
            new Game { GameID = 20, Title = "Returnal", Description = "A roguelike third-person shooter with sci-fi elements.", Platform = "PlayStation 5", Category = "Shooter", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = null, CoverImageURL = "/images/returnal.jpg", DownloadSize = 1500000000 },

            // PlayStation 4 Games
            new Game { GameID = 21, Title = "Uncharted 4: A Thief's End", Description = "A treasure-hunting adventure with stunning visuals.", Platform = "PlayStation 4", Category = "Action/Adventure", LanguageSupport = "English, French", Price = 39.99m, Rating = null, CoverImageURL = "/images/uncharted_4.jpg", DownloadSize = 500000000 },
            new Game { GameID = 22, Title = "The Last of Us Part II", Description = "A gripping action adventure with deep storytelling.", Platform = "PlayStation 4", Category = "Action/Adventure", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = null, CoverImageURL = "/images/last_of_us2.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 23, Title = "Ghost of Tsushima", Description = "An open-world action game set in feudal Japan.", Platform = "PlayStation 4", Category = "Action", LanguageSupport = "English, Russian", Price = 49.99m, Rating = null, CoverImageURL = "/images/ghost_of_tsushima.jpg", DownloadSize = 4500000000 },
            new Game { GameID = 24, Title = "Bloodborne", Description = "A gothic action RPG with intense combat mechanics.", Platform = "PlayStation 4", Category = "RPG", LanguageSupport = "English, French", Price = 39.99m, Rating = null, CoverImageURL = "/images/bloodborne.jpg", DownloadSize = 2500000000 },
            new Game { GameID = 25, Title = "Gran Turismo Sport", Description = "A racing simulation with lifelike graphics.", Platform = "PlayStation 4", Category = "Racing", LanguageSupport = "English, German", Price = 29.99m, Rating = null, CoverImageURL = "/images/gran_turismo_sport.jpg", DownloadSize = 4000000000 },

            // Nintendo Switch Games
            new Game { GameID = 26, Title = "The Legend of Zelda: Breath of the Wild", Description = "An open-world adventure in the land of Hyrule.", Platform = "Nintendo Switch", Category = "Adventure", LanguageSupport = "English, French", Price = 59.99m, Rating = null, CoverImageURL = "/images/zelda_breath_wild.jpg", DownloadSize = 1500000000 },
            new Game { GameID = 27, Title = "Animal Crossing: New Horizons", Description = "A life-simulation game where you build your island paradise.", Platform = "Nintendo Switch", Category = "Simulation", LanguageSupport = "English, Spanish", Price = 59.99m, Rating = null, CoverImageURL = "/images/animal_crossing.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 28, Title = "Super Mario Odyssey", Description = "A 3D Mario platformer with a globe-trotting adventure.", Platform = "Nintendo Switch", Category = "Platformer", LanguageSupport = "English, Japanese", Price = 59.99m, Rating = null, CoverImageURL = "/images/super_mario_odyssey.jpg", DownloadSize = 3000000000 },
            new Game { GameID = 29, Title = "Mario Kart 8 Deluxe", Description = "A fun kart-racing game featuring Nintendo characters.", Platform = "Nintendo Switch", Category = "Racing", LanguageSupport = "English, Italian", Price = 59.99m, Rating = null, CoverImageURL = "/images/mario_kart8.jpg", DownloadSize = 5000000000 },
            new Game { GameID = 30, Title = "Splatoon 3", Description = "A colorful third-person shooter where ink is your weapon.", Platform = "Nintendo Switch", Category = "Shooter", LanguageSupport = "English, French", Price = 59.99m, Rating = null, CoverImageURL = "/images/splatoon3.jpg", DownloadSize = 2500000000 }
        );

            builder.Entity<Event>().HasData(
            new Event
            {
                EventId = 1,
                Name = "Battle Royale Championship",
                Description = "Compete in an intense battle royale tournament and win amazing prizes!",
                Date = new DateOnly(2024, 12, 15),
                Location = "eSports Arena, Los Angeles",
                MaxRegistrations = 200
            },
            new Event
            {
                EventId = 2,
                Name = "Retro Gaming Night",
                Description = "Enjoy classic games from the 80s and 90s on original consoles.",
                Date = new DateOnly(2024, 12, 20),
                Location = "Pixel Café, New York",
                MaxRegistrations = 50
            },
            new Event
            {
                EventId = 3,
                Name = "Indie Game Showcase",
                Description = "Discover the best indie games from up-and-coming developers.",
                Date = new DateOnly(2024, 12, 22),
                Location = "GameDev Convention Center, San Francisco",
                MaxRegistrations = 100
            },
            new Event
            {
                EventId = 4,
                Name = "Cosplay Contest",
                Description = "Show off your best gaming-themed cosplay and win exclusive rewards.",
                Date = new DateOnly(2024, 12, 25),
                Location = "Anime Expo Hall, Chicago",
                MaxRegistrations = 75
            },
            new Event
            {   
                EventId = 5,
                Name = "League of Legends Meetup",
                Description = "Join fellow summoners for a day of friendly matches and discussions.",
                Date = new DateOnly(2025, 1, 5),
                Location = "Riot HQ, Seattle",
                MaxRegistrations = 150
            },
            new Event
            {
                EventId = 6,
                Name = "Minecraft Build-Off",
                Description = "Showcase your creativity in a timed building challenge.",
                Date = new DateOnly(2025, 1, 10),
                Location = "Creative Zone, Houston",
                MaxRegistrations = 100
            },
            new Event
            {
                EventId = 7,
                Name = "VR Experience Day",
                Description = "Immerse yourself in the latest VR games and experiences.",
                Date = new DateOnly(2025, 1, 15),
                Location = "Virtual Arena, Boston",
                MaxRegistrations = 80
            },
            new Event
            {
                EventId = 8,
                Name = "Speedrunning Workshop",
                Description = "Learn the art of speedrunning from professional gamers.",
                Date = new DateOnly(2025, 1, 18),
                Location = "Streamer Studio, Denver",
        MaxRegistrations = 60
            },
            new Event
            {
                EventId = 9,
                Name = "Overwatch 2 Tournament",
                Description = "Team up and compete in this high-octane FPS competition.",
                Date = new DateOnly(2025, 1, 22),
                Location = "Blizzard HQ, Irvine",
                MaxRegistrations = 100
            },
            new Event
            {
                EventId = 10,
                Name = "Pokemon Card Battle",
                Description = "Bring your deck and challenge other trainers to card battles.",
                Date = new DateOnly(2025, 1, 28),
                Location = "Card Haven, Dallas",
                MaxRegistrations = 50
            },
            new Event
            {
                EventId = 11,
                Name = "Dungeons & Dragons Campaign Night",
                Description = "Join a thrilling one-shot campaign hosted by experienced DMs.",
                Date = new DateOnly(2025, 2, 1),
                Location = "Adventure Guild, Portland",
                MaxRegistrations = 40
            },
            new Event
            {
                EventId = 12,
                Name = "Fortnite Duo Challenge",
                Description = "Team up with a friend and aim for Victory Royale!",
                Date = new DateOnly(2025, 2, 5),
                Location = "Battle Grounds, Miami",
                MaxRegistrations = 100
            },
            new Event
            {
                EventId = 13,
                Name = "Elden Ring Lore Discussion",
                Description = "Dive deep into the lore of Elden Ring with fellow fans.",
                Date = new DateOnly(2025, 2, 10),
                Location = "Lore Hall, Philadelphia",
                MaxRegistrations = 70
            },
            new Event
            {
                EventId = 14,
                Name = "Smash Bros. Ultimate Showdown",
                Description = "Compete in a 1v1 Smash Bros. Ultimate tournament.",
                Date = new DateOnly(2025, 2, 14),
                Location = "Nintendo Center, Atlanta",
                MaxRegistrations = 80
            },
            new Event
            {
                EventId = 15,
                Name = "Game Development Seminar",
                Description = "Learn tips and tricks from professional game developers.",
                Date = new DateOnly(2025, 2, 18),
                Location = "Tech Hub, Austin",
                MaxRegistrations = 100
            },
            new Event
            {
                EventId = 16,
                Name = "Board Game Bonanza",
                Description = "Explore a variety of board games with fellow enthusiasts.",
                Date = new DateOnly(2025, 2, 20),
                Location = "Tabletop Tavern, Detroit",
                MaxRegistrations = 50
            },
            new Event
            {
                EventId = 17,
                Name = "Rocket League Championship",
                Description = "Show off your aerial skills in this car soccer tournament.",
                Date = new DateOnly(2025, 2, 25),
                Location = "Arena Dome, Phoenix",
                MaxRegistrations = 120
            },
            new Event
            {
                EventId = 18,
                Name = "Horror Game Marathon",
                Description = "Brace yourself for a night of spooky gaming experiences.",
                Date = new DateOnly(2025, 2, 28),
                Location = "Haunted Hub, Orlando",
                MaxRegistrations = 40
            },
            new Event
            {
                EventId = 19,
                Name = "Street Fighter V Exhibition",
                Description = "Show your skills in an exciting Street Fighter competition.",
                Date = new DateOnly(2025, 3, 1),
                Location = "Arcade Central, San Diego",
                MaxRegistrations = 80
            },
            new Event
            {
                EventId = 20,
                Name = "Cozy Animal Crossing Meet-Up",
                Description = "Relax and share island tips with other Animal Crossing players.",
                Date = new DateOnly(2025, 3, 5),
                Location = "Villager Café, Charlotte",
                MaxRegistrations = 30
            }
        );


            builder.Entity<Review>(b =>
            {

                b.HasOne(r => r.Game)
                .WithMany(g => g.Review)
                .HasForeignKey(r => r.GameId);

                b.HasOne(r => r.User)
                .WithMany(u => u.Review)
                .HasForeignKey(r => r.UserId);

                b.HasData(
                    new Review()
                    {
                        Id = 1,
                        UserId = "05e61254-11dc-44d9-89e7-0e574ce7099a",
                        GameId = 1,
                        Content = "Black  jack",
                        Rating = 1,
                        Approved = true
                    },
                    new Review()
                    {
                        Id = 5,
                        UserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        GameId = 6,
                        Content = "Epic cyberpunky game",
                        Rating = 3,
                        Approved = true
                    },
                    new Review()
                    {
                        Id = 2,
                        UserId = "042f95a1-3247-4e6a-a375-d2165a8bf16c",
                        GameId = 11,
                        Content = "sui",
                        Rating = 5,
                        Approved = false
                    },
                    new Review()
                    {
                        Id = 3,
                        UserId = "88193658-5295-478b-9f7e-534f739a06bc",
                        GameId = 21,
                        Content = "john uncharted",
                        Rating = 3,
                        Approved = true
                    },
                    new Review()
                    {
                        Id = 4,
                        UserId = "bac4f198-0003-438a-a347-2c27bc0e0ffa",
                        GameId = 26,
                        Content = "very open",
                        Rating = 5,
                        Approved = true
                    }
                );
            });

            builder.Entity<Wishlist>(b =>
            {
                b.HasMany(w => w.User)
                .WithMany(u => u.Wishlist);

                b.HasMany(w => w.Game)
                .WithMany(g => g.Wishlist);

                b.HasData(
                    new Wishlist()
                    {
                        WishlistId = 1,
                        UserId = "05e61254-11dc-44d9-89e7-0e574ce7099a",
                        GameId = 1
                    }, new Wishlist()
                    {
                        WishlistId = 2,
                        UserId = "05e61254-11dc-44d9-89e7-0e574ce7099a",
                        GameId = 2
                    }, new Wishlist()
                    {
                        WishlistId = 3,
                        UserId = "05e61254-11dc-44d9-89e7-0e574ce7099a",
                        GameId = 11
                    }, new Wishlist()
                    {
                        WishlistId = 4,
                        UserId = "05e61254-11dc-44d9-89e7-0e574ce7099a",
                        GameId = 25
                    }, new Wishlist()
                    {
                        WishlistId = 5,
                        UserId = "05e61254-11dc-44d9-89e7-0e574ce7099a",
                        GameId = 6
                    },

                    new Wishlist()
                    {
                        WishlistId = 6,
                        UserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        GameId = 3
                    }, new Wishlist()
                    {
                        WishlistId = 7,
                        UserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        GameId = 4
                    }, new Wishlist()
                    {
                        WishlistId = 8,
                        UserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        GameId = 28
                    }, new Wishlist()
                    {
                        WishlistId = 9,
                        UserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        GameId = 13
                    }, new Wishlist()
                    {
                        WishlistId = 10,
                        UserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        GameId = 30
                    },

                    new Wishlist()
                    {
                        WishlistId = 11,
                        UserId = "042f95a1-3247-4e6a-a375-d2165a8bf16c",
                        GameId = 30
                    }, new Wishlist()
                    {
                        WishlistId = 12,
                        UserId = "042f95a1-3247-4e6a-a375-d2165a8bf16c",
                        GameId = 23
                    }, new Wishlist()
                    {
                        WishlistId = 13,
                        UserId = "042f95a1-3247-4e6a-a375-d2165a8bf16c",
                        GameId = 20
                    }, new Wishlist()
                    {
                        WishlistId = 14,
                        UserId = "042f95a1-3247-4e6a-a375-d2165a8bf16c",
                        GameId = 1
                    }
                );
            });

            builder.Entity<Friends>(b =>
            {
                b.HasMany(f => f.UserOne)
                .WithMany();

                b.HasMany(f => f.UserTwo)
                .WithMany();

                b.HasData(
                    new Friends { FriendId = 3, UserOneId = "042f95a1-3247-4e6a-a375-d2165a8bf16c", UserTwoId = "05e61254-11dc-44d9-89e7-0e574ce7099a" },
                    new Friends { FriendId = 1, UserOneId = "042f95a1-3247-4e6a-a375-d2165a8bf16c", UserTwoId = "e091dbd6-3f3f-4177-87f1-c8a7b2674a9f" },
                    new Friends { FriendId = 2, UserOneId = "042f95a1-3247-4e6a-a375-d2165a8bf16c", UserTwoId = "88193658-5295-478b-9f7e-534f739a06bc" }
                    );
            });

            builder.Entity<FriendRequest>(b =>
            {
                b.HasMany(f => f.PrimaryUser)
                .WithMany();

                b.HasMany(f => f.SecondaryUser)
                .WithMany();

                b.HasData(
                    new FriendRequest
                    {
                        FriendRequestId = 1,
                        PrimaryUserId = "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2",
                        SecondaryUserId = "042f95a1-3247-4e6a-a375-d2165a8bf16c"
                    }
                );
            });
        }

        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<FriendRequest> FriendRequest { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }

    }
}
