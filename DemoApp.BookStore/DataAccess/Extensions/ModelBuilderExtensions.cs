using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.BookStore.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Инициализация авторов и их книг в бд, настройка связей между ними.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Author author1 = new Author() { Id = 1, FirstName = "Иван", SurnameName = "Иванов" };
            Author author2 = new Author() { Id = 2, FirstName = "Андрей", SurnameName = "Попов" };
            Author author3 = new Author() { Id = 3, FirstName = "Елена", SurnameName = "Наумова" };

            Book book1 = new Book() { Id = 1, Name = "Book1" };
            Book book2 = new Book() { Id = 2, Name = "Book2" };
            Book book3 = new Book() { Id = 3, Name = "Book3" };

            ScienceBook mkb1 = new ScienceBook() { Id = 4, Name = "МКБ1", Type = "Справочник" };
            ScienceBook mkb2 = new ScienceBook() { Id = 5, Name = "МКБ2", Type = "Справочник" };
            ScienceBook mkb3 = new ScienceBook() { Id = 6, Name = "МКБ3", Type = "Справочник" };

            Magazine magazine1 = new Magazine() { Id = 7, Name = "Чистый дом1", ReaderType = "Для домохозяек" };
            Magazine magazine2 = new Magazine() { Id = 8, Name = "Чистый дом2", ReaderType = "Для домохозяек" };
            Magazine magazine3 = new Magazine() { Id = 9, Name = "Чистый дом3", ReaderType = "Для домохозяек" };

            modelBuilder.Entity<Author>().HasData(
                author1,
                author2,
                author3
                );

            modelBuilder.Entity<Book>().HasData(
                book1,
                book2,
                book3
                );
            modelBuilder.Entity<ScienceBook>().HasData(
                mkb1,
                mkb2,
                mkb3
                );
            modelBuilder.Entity<Magazine>().HasData(
                magazine1,
                magazine2,
                magazine3
                );

            modelBuilder.Entity<AuthorBook>().HasData(
                new AuthorBook() { AuthorId = 1, BookId = 1 },
                new AuthorBook() { AuthorId = 1, BookId = 2 },
                new AuthorBook() { AuthorId = 1, BookId = 3 },
                new AuthorBook() { AuthorId = 2, BookId = 4 },
                new AuthorBook() { AuthorId = 2, BookId = 5 },
                new AuthorBook() { AuthorId = 2, BookId = 6 },
                new AuthorBook() { AuthorId = 3, BookId = 7 },
                new AuthorBook() { AuthorId = 3, BookId = 8 },
                new AuthorBook() { AuthorId = 3, BookId = 9 }
                );
        }
    }
}
