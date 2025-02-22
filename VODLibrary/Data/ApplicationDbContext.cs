﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VODLibrary.Data.Models;

namespace VODLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<VideoRecord> VideoRecords { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Reply> Replies { get; set; }

        private Category[] categories;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasKey(c => c.Id);

            builder.Entity<VideoRecord>()
                .HasKey(v => v.Id);

            builder.Entity<Comment>()
                .HasKey(c => c.Id);

            builder.Entity<Reply>()
                .HasKey(r => r.Id);

            builder.Entity<VideoRecord>()
                .HasOne(v => v.Category)
                .WithMany(c => c.Videos)
                .HasForeignKey(v => v.CategoryId);

            builder.Entity<VideoRecord>()
                .HasOne(v => v.VideoOwner)
                .WithMany()
                .HasForeignKey(v => v.VideoOwnerId);

            builder.Entity<VideoRecord>()
                .HasMany(v => v.Comments)
                .WithOne(c => c.VideoRecord)
                .HasForeignKey(c => c.VideoRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasMany(c => c.Replies)
                .WithOne(r => r.Comment)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);


            SeedCategories();

            builder.Entity<Category>()
                .HasData(categories);

        }

        private void SeedCategories()
        {
            categories = new Category[]
            {
                new Category { Id = 1, Name = "Music"} ,
                new Category { Id = 2, Name = "Sports"},
                new Category { Id = 3, Name = "Gaming"},
                new Category { Id = 4, Name = "Entertainment"},
                new Category { Id = 5, Name = "Education"},
                new Category { Id = 6, Name = "Science and Technology"}


            };
        }
    }
}
