using EFCoreDemoApp.DataAccess;
using EFCoreDemoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace EFCoreDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DemoAppContext db = new DemoAppContext())
            {
                if (!File.Exists("DemoAppDb.db"))
                {
                    db.Database.EnsureCreated();
                    Init(db);
                }


                PrintAutorInfo(db);

                int menu = -1;
                while (menu != 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("1) Добавить книгу");
                    Console.WriteLine("2) Удалить книгу");
                    Console.WriteLine("3) Добавить автора");
                    Console.WriteLine("4) Удалить автора");
                    Console.WriteLine("5) Вывод информации об авторах и их книгах");
                    Console.WriteLine("0) Выход");
                    Console.Write("Выберите пункт меню: ");
                    Int32.TryParse(Console.ReadLine(), out menu);

                    switch (menu)
                    {
                        case 1:
                            AddBookMenu(db);
                            break;
                        case 2:
                            DeleteBookMenu(db);
                            break;
                        case 3:
                            AddAuthorMenu(db);
                            break;
                        case 4:
                            DeleteAuthorMenu(db);
                            break;
                        case 5:
                            Console.WriteLine();
                            PrintAutorInfo(db);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Интерактивное меню для добавления автора
        /// </summary>
        /// <param name="db"></param>
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
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Автор уже существует!");
            }
        }

        /// <summary>
        /// Интерактивное меню для удаления автора
        /// </summary>
        /// <param name="db"></param>
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
        /// Интерактивное меню для добавления книги
        /// </summary>
        /// <param name="db"></param>
        static void AddBookMenu(DemoAppContext db)
        {
            Console.WriteLine("1) Обычная книга");
            Console.WriteLine("2) Научная книга");
            Console.WriteLine("3) Журнал");
            Console.Write("Выберите тип книги, который хотите добавить: ");

            byte menuChoice;
            if (Byte.TryParse(Console.ReadLine(), out menuChoice))
            {
                Console.WriteLine();
                PrintAuthorsList(db);
                Console.Write("Введите идентификатор его автора: ");
                Console.WriteLine();

                int authorId;
                if (Int32.TryParse(Console.ReadLine(), out authorId))
                {
                    var author = db.Authors.Find(authorId);

                    if (author != null)
                    {
                        Console.Write("Введите название: ");

                        string name = Console.ReadLine();

                        Book book = null;

                        if (menuChoice == 1)
                        {
                            book = new Book() { Name = name };
                        }
                        else if (menuChoice == 2)
                        {
                            Console.Write("Введите жанр книги: ");
                            string genre = Console.ReadLine();

                            book = new ScienceBook() { Name = name, Genre = genre };
                        }
                        else if (menuChoice == 3)
                        {
                            Console.Write("Для кого предназначен журнал? ");
                            string reader = Console.ReadLine();

                            book = new Magazine() { Name = name, ReaderType = reader };
                        }
                        db.Books.Add(book);
                        author.AuthorBooks.Add(new AuthorBook() { Book = book });
                        db.SaveChanges();

                    }
                    else
                    {
                        Console.WriteLine("Автора с таким идентификатором нет в базе");
                    }
                }
                else
                {
                    Console.WriteLine("Ожидалось целое число");
                }
            }
        }

        /// <summary>
        /// Интерактивное меню для удаления книги
        /// </summary>
        /// <param name="db"></param>
        static void DeleteBookMenu(DemoAppContext db)
        {
            PrintBooksList(db);
            Console.Write("Введите индентификатор удаляемой книги: ");
            int bookId;
            if (Int32.TryParse(Console.ReadLine(), out bookId))
            {
                var book = db.Books.Find(bookId);

                if (book != null)
                {
                    db.Books.Remove(book);
                    db.SaveChanges();
                    Console.WriteLine($"Кинга {book.Name} удалена из базы");
                }
                else
                {
                    Console.WriteLine("Книги с таким идентификатором нет в базе");
                }
            }
        }

        /// <summary>
        /// Выводит список авторов с их идентификаторами
        /// </summary>
        /// <param name="db"></param>
        static void PrintAuthorsList(DemoAppContext db)
        {
            var authors = db.Authors;

            foreach (var u in authors)
            {
                Console.WriteLine($"{u.Id})\t{u.FullName}");
            }
        }

        /// <summary>
        /// Выводит список книг с их идентификаторами
        /// </summary>
        /// <param name="db"></param>
        static void PrintBooksList(DemoAppContext db)
        {
            var books = db.Books;

            foreach (var u in books)
            {
                Console.WriteLine($"{u.Id})\t{u.Name}");
            }
        }

        /// <summary>
        /// Заполенение базы данных и связей
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

            //author1.AuthorBooks.Add(new AuthorBook() { BookId = 7, AuthorId = 1 });
            //author1.AuthorBooks.Add(new AuthorBook() { BookId = 8, AuthorId = 1 });
            //author1.AuthorBooks.Add(new AuthorBook() { BookId = 9, AuthorId = 1 });

            author1.AuthorBooks.Add(new AuthorBook() { Book = book1 });
            author1.AuthorBooks.Add(new AuthorBook() { Book = book2 });
            author1.AuthorBooks.Add(new AuthorBook() { Book = book3 });


            //db.AuthorBooks.Add(new AuthorBook() { Author = author1, Book = book1 });
            //db.AuthorBooks.Add(new AuthorBook() { Author = author1, Book = book2 });
            //db.AuthorBooks.Add(new AuthorBook() { Author = author1, Book = book3 });

            db.AuthorBooks.Add(new AuthorBook() { Author = author2, Book = mkb1 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author2, Book = mkb2 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author2, Book = mkb3 });

            db.AuthorBooks.Add(new AuthorBook() { Author = author3, Book = magazine1 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author3, Book = magazine2 });
            db.AuthorBooks.Add(new AuthorBook() { Author = author3, Book = magazine3 });

            db.Authors.Add(author1);
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
            var authors = db.Authors.Include(a => a.AuthorBooks).ThenInclude(ab => ab.Book);
            Console.WriteLine();
            Console.WriteLine("Authors Info");
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
    }
}
