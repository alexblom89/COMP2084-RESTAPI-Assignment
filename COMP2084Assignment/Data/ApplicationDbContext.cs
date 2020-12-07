using COMP2084Assignment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace COMP2084Assignment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Define many-to-many relationship between Game and Genre models.
            //Following this tutorial: https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration 
            //However this may be out of date.
            builder.Entity<GameGenre>()
                .HasKey(gg => new { gg.GameId, gg.GenreId });
            builder.Entity<GameGenre>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameId)
                .HasConstraintName("FK_GameGenre_GameId");
            builder.Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(c => c.GameGenres)
                .HasForeignKey(gg => gg.GenreId)
                .HasConstraintName("FK_GameGenre_GenreId");

            //Define many-to-many relationship between Game and Platform models.
            builder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });
            builder.Entity<GamePlatform>()
                .HasOne(gp => gp.Game)
                .WithMany(g => g.GamePlatforms)
                .HasForeignKey(gp => gp.GameId)
                .HasConstraintName("FK_GamePlatform_GameId");
            builder.Entity<GamePlatform>()
                .HasOne(gp => gp.Platform)
                .WithMany(p => p.GamePlatforms)
                .HasForeignKey(gp => gp.PlatformId)
                .HasConstraintName("FK_GamePlatform_PlatformId");
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
