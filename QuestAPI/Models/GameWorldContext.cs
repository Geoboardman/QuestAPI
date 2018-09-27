using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuestAPI.Models
{
    public partial class GameWorldContext : DbContext
    {
        public GameWorldContext()
        {
        }

        public GameWorldContext(DbContextOptions<GameWorldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MapObjects> MapObjects { get; set; }
        public virtual DbSet<Monsters> Monsters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=184.168.194.62;Database=SanctuaryRPG;User ID=Administrator;Password=!Theman315;");
                //optionsBuilder.UseSqlServer("Server=sanctuaryrpg.cduqxrqhyqqv.us-west-2.rds.amazonaws.com;Database=GameWorld;User ID=Admin;Password=Passwordx;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MapObjects>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Attributes).HasColumnName("attributes");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Lon)
                    .HasColumnName("lon")
                    .HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasColumnName("region")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(200);

                entity.Property(e => e.TimeStamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Monsters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Attributes).HasColumnName("attributes");
            });
        }
    }
}