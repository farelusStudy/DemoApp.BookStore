using EFCoreDemoApp.DataAccess;
using EFCoreDemoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCoreDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DemoAppContext db = new DemoAppContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Init(db);

                db.Authors.Remove(db.Authors.Find(1));
                db.SaveChanges();
                PrintUsersTable(db);
                PrintBooksTable(db);
                PrintAutorInfo(db);

                //db.Users.FromSqlRaw("INSERT INTO Users (firstname, surnamename, discriminator) VALUES('Name1', 'Surname1', 'User')");
                //db.SaveChanges();
            }

            Console.ReadKey();
        }

        static void AddAuthorMenu(DemoAppContext db)
        {
            Console.WriteLine("Введите имя автора");
            string fname = Console.ReadLine();

            Console.WriteLine("Введите фамилию автора");
            string sname = Console.ReadLine();

            var author = db.Authors.Where(a => a.FirstName == fname && a.SurnameName == sname).FirstOrDefault();

            if (author == null)
            {
                db.Authors.Add(new Author() { FirstName = fname, SurnameName = sname });
            }
            else
            {
                Console.WriteLine("Автор уже существует!");
            }
        }

        static void DeleteAuthorMenu(DemoAppContext db)
        {
            Console.WriteLine("Введите имя автора");
            string fname = Console.ReadLine();

            Console.WriteLine("Введите фамилию автора");
            string sname = Console.ReadLine();

            var author = db.Authors.Where(a => a.FirstName == fname && a.SurnameName == sname).FirstOrDefault();

            if (author != null)
            {
                db.Authors.Remove(db.Authors.Find(author.Id));
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Автор не существует!");
            }
        }

        /// <summary>
        /// Заполенение базы данных и настройка связей
        /// </summary>
        /// <param name="db"></param>
        static void Init(DemoAppContext db)
        {
            User reader = new User() { FirstName = "Рафаэл", SurnameName = "Гафаров" };

            Author author1 = new Author() { FirstName = "Иван", SurnameName = "Иванов" };
            Author author2 = new Author() { FirstName = "Андрей", SurnameName = "Попов" };
            Author author3 = new Author() { FirstName = "Елена", SurnameName = "Наумова" };

            Book book1 = new Book() { Name = "Book1" };
            Book book2 = new Book() { Name = "Book2" };
            Book book3 = new Book() { Name = "Book3" };
            ScienceBook mkb1 = new ScienceBook() { Name = "МКБ1", Genre = "Справочник" };
            ScienceBook mkb2 = new ScienceBook() { Name = "МКБ2", Genre = "Справочник" };
            ScienceBook mkb3 = new ScienceBook() { Name = "МКБ3", Genre = "Справочник" };
            Magazine magazine1 = new Magazine() { Name = "Чистый дом1", ReaderType = "Для домохозяек" };
            Magazine magazine2 = new Magazine() { Name = "Чистый дом2", ReaderType = "Для домохозяек" };
            Magazine magazine3 = new Magazine() { Name = "Чистый дом3", ReaderType = "Для домохозяек" };


            db.AuthorBooks.Add(new AuthorBook() { Author = author1, Book = book1 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author1, Book = book2 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author1, Book = book3 });

            db.AuthorBooks.Add(new AuthorBook() { Author = author2, Book = mkb1 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author2, Book = mkb2 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author2, Book = mkb3 });

            db.AuthorBooks.Add(new AuthorBook() { Author = author3, Book = magazine1 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author3, Book = magazine2 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author3, Book = magazine3 });

            db.Add(author1);
            db.Authors.Add(author2);
            db.Authors.Add(author3);
            db.Users.Add(reader);


            db.Books.Add(book1);
            db.Books.Add(book2);
            db.Books.Add(book3);

            db.ScienceBooks.Add(mkb1);
            db.ScienceBooks.Add(mkb2);
            db.ScienceBooks.Add(mkb3);

            db.Magazines.Add(magazine1);
            db.Magazines.Add(magazine2);
            db.Magazines.Add(magazine3);

            db.SaveChanges();
        }

        /// <summary>
        /// Выводит список авторов и их книг
        /// </summary>
        /// <param name="db"></param>
        static void PrintAutorInfo(DemoAppContext db)
        {
            var authors = db.Authors.Include(a => a.AuthorBooks);
            Console.WriteLine();
            Console.WriteLine("Authors Info");
            Console.WriteLine($"Id\t|\t Full Name \t|\t Discriminator");
            Console.WriteLine("______________________________________________");
            foreach (var a in authors)
            {
                Console.WriteLine($"{a.Id}\t \t{a.FullName}\t \t{a.Discriminator}");
                Console.WriteLine("Books:");
                foreach (var b in a.AuthorBooks.Select(a => a.Book))
                {
                    Console.WriteLine($"\t{b.Name}\t \t{b.Discriminator}");
                }
                Console.WriteLine("______________________________________________");
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Выводит список пользователей
        /// </summary>
        /// <param name="db"></param>
        static void PrintUsersTable(DemoAppContext db)
        {
            var users = db.Users;
            Console.WriteLine("Users Table");
            Console.WriteLine($"Id\t|\t Full Name \t|\t Discriminator");
            Console.WriteLine("______________________________________________");
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Id}\t|\t{u.FullName}\t|\t{u.Discriminator}");
            }
        }

        /// <summary>
        /// Выводит список книг
        /// </summary>
        /// <param name="db"></param>
        static void PrintBooksTable(DemoAppContext db)
        {
            var books = db.Books;
            Console.WriteLine("Books Table");
            Console.WriteLine($"Id\t|\tBook Name\t|\tAuthor Full Name \t|\t Discriminator");
            Console.WriteLine("____________________________________________________________________");
            foreach (var b in books)
            {
                Console.WriteLine($"{b.Id}\t|\t{b.Name}\t|\t\t|\t{b.Discriminator}");
            }
        }
    }
}
