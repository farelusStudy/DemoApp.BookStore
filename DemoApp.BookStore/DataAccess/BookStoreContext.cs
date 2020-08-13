using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Sources;

namespace BookStore.DataAccess
{
    class BookStoreContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Magazine> Magazines { get; set; }
        public virtual DbSet<ScienceBook> ScienceBooks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<AuthorBook> AuthorBooks { get; set; }

        public BookStoreContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=BookStore.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(e => e.FirstName)
                        .HasMaxLength(20);

                entity.Property(e => e.SurnameName)
                        .HasMaxLength(20);

                entity.Ignore(e => e.FullName);
            });


            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
            });

            modelBuilder.Entity<AuthorBook>(entity =>
            {
                entity.HasKey(k => new { k.AuthorId, k.BookId });

                entity.HasOne(ab => ab.Author)
                        .WithMany(b => b.AuthorBooks)
                        .HasForeignKey(ab => ab.AuthorId);

                entity.HasOne(ab => ab.Book)
                        .WithMany(a => a.AuthorBooks)
                        .HasForeignKey(ab => ab.BookId);
            });

            modelBuilder.Entity<Magazine>(entity =>
            {
                entity.Property(m => m.ReaderType);
            });

            modelBuilder.Entity<ScienceBook>(entity =>
            {
                entity.Property(sb => sb.Type);
            });
        }
    }
}
