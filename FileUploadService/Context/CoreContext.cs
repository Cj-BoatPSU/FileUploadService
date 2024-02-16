using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FileUploadService.Models.DBModels;

namespace FileUploadService.Context
{
    public partial class CoreContext : DbContext
    {
        public CoreContext()
        {
        }

        public CoreContext(DbContextOptions<CoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<FileTable> FileTables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\Local;Initial Catalog=B1_XBgSbO;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDt).HasColumnName("Created_DT");

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.UpdatedDt).HasColumnName("Updated_DT");

                entity.Property(e => e.Username).HasMaxLength(200);
            });

            modelBuilder.Entity<FileTable>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("File_Table");

                entity.Property(e => e.CreatedDt).HasColumnName("Created_DT");

                entity.Property(e => e.ExtensionType)
                    .HasMaxLength(20)
                    .HasColumnName("Extension_Type");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Path).HasMaxLength(500);

                entity.Property(e => e.Reference).HasMaxLength(100);

                entity.Property(e => e.UpdatedDt).HasColumnName("Updated_DT");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
