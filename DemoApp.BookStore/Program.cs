using BookStore.DataAccess;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                db.Database.EnsureCreated();

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
        static void AddAuthorMenu(BookStoreContext db)
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
        static void DeleteAuthorMenu(BookStoreContext db)
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
        static void AddBookMenu(BookStoreContext db)
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
                            Console.Write("Введите тип книги (учебник, справочник): ");
                            string type = Console.ReadLine();

                            book = new ScienceBook() { Name = name, Type = type };
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
        static void DeleteBookMenu(BookStoreContext db)
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
        static void PrintAuthorsList(BookStoreContext db)
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
        static void PrintBooksList(BookStoreContext db)
        {
            var books = db.Books;

            foreach (var u in books)
            {
                Console.WriteLine($"{u.Id})\t{u.Name}");
            }
        }

        /// <summary>
        /// Вывод списка авторов и их книг
        /// </summary>
        /// <param name="db"></param>
        static void PrintAutorInfo(BookStoreContext db)
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
