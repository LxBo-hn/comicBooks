    
using Microsoft.EntityFrameworkCore;
using System;

namespace ComicRental.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình ComicBook
            modelBuilder.Entity<ComicBook>(entity => 
            {
                // Khóa chính
                entity.HasKey(e => e.ComicBookID);

                // Cấu hình cột
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                entity.Property(e => e.PricePerDay)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                // Index
                entity.HasIndex(e => e.Title);
            });

            // Cấu hình Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.RegistrationDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                // Index
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });

            // Cấu hình Rental
            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.RentalID);

                entity.Property(e => e.RentalDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.ReturnDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                // Relationship với Customer (1-n)
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.CustomerID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index
                entity.HasIndex(e => e.RentalDate);
                entity.HasIndex(e => e.CustomerID);
            });

            // Cấu hình RentalDetail
            modelBuilder.Entity<RentalDetail>(entity =>
            {
                entity.HasKey(e => e.RentalDetailID);

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.PricePerDay)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                // Relationship với Rental (1-n)
                entity.HasOne(d => d.Rental)
                    .WithMany(p => p.RentalDetails)
                    .HasForeignKey(d => d.RentalID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship với ComicBook (1-n)
                entity.HasOne(d => d.ComicBook)
                    .WithMany()
                    .HasForeignKey(d => d.ComicBookID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index
                entity.HasIndex(e => new { e.RentalID, e.ComicBookID });
            });

            // Seed data mẫu (nếu cần)
            modelBuilder.Entity<ComicBook>().HasData(
                new ComicBook { ComicBookID = 1, Title = "Doraemon", Author = "Fujiko F. Fujio", PricePerDay = 5000 },
                new ComicBook { ComicBookID = 2, Title = "Conan", Author = "Gosho Aoyama", PricePerDay = 6000 }
            );
        }
    }
}

