// FILE: Program.cs
using LibraryOOP.Models;
using LibraryOOP.Repositories;
using LibraryOOP.Services;
using System;

namespace LibraryOOP
{
    internal class Program
    {
        // Main method to run the library system
        static void Main(string[] args)
        {
            FileContext fileContext = new FileContext();

            var bookRepo = new BookRepository(fileContext);
            var memberRepo = new MemberRepository(fileContext);
            var recordRepo = new BorrowRecordRepository(fileContext);
            var libraryService = new LibraryService(bookRepo, memberRepo, recordRepo);

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("╔════════════════════════════════════════════════╗");
                Console.WriteLine("║               LIBRARY SYSTEM MENU              ║");
                Console.WriteLine("╚════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  1  Add Book");
                Console.WriteLine("  2  Register Member");
                Console.WriteLine("  3  Borrow Book");
                Console.WriteLine("  4  Return Book");
                Console.WriteLine("  5  View All Books");
                Console.WriteLine("  6  View All Members");
                Console.WriteLine("  7  View Borrow Records");
                Console.WriteLine("  0  Exit");
                Console.ResetColor();

                Console.Write("\nSelect an option: ");
                string input = Console.ReadLine()?? "";

                switch (input)
                {
                    case "1": AddBook(libraryService); break;
                    case "2": RegisterMember(libraryService); break;
                    case "3": BorrowBook(libraryService); break;
                    case "4": ReturnBook(libraryService); break;
                    case "5": ViewBooks(bookRepo); break;
                    case "6": ViewMembers(memberRepo); break;
                    case "7": ViewBorrowRecords(recordRepo); break;
                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n Exiting system. Goodbye!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Invalid option. Press any key to try again...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
        // Method to add a new book
        static void AddBook(ILibraryService service)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("╔════════════════════════════╗");
            Console.WriteLine("║         ADD NEW BOOK       ║");
            Console.WriteLine("╚════════════════════════════╝");
            Console.ResetColor();

            Console.Write(" Book ID (int): ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine(" Invalid Book ID. Must be a positive integer.");
                return;
            }

            Console.Write(" Title: ");
            string title = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine(" Title cannot be empty.");
                return;
            }

            Console.Write(" Author: ");
            string author = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine(" Author name cannot be empty.");
                return;
            }

            var book = new Book { BookId = id, Title = title, Author = author, IsAvailable = true };
            service.AddBook(book);
            Console.WriteLine(" Book added. Press any key to return...");
            Console.ReadKey();
        }
        // Method to register a new member
        static void RegisterMember(ILibraryService service)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔═════════════════════════════╗");
            Console.WriteLine("║       REGISTER MEMBER       ║");
            Console.WriteLine("╚═════════════════════════════╝");
            Console.ResetColor();

            Console.Write(" Member ID (int): ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine(" Invalid Member ID. Must be a positive integer.");
                return;
            }

            Console.Write(" Member Name: ");
            string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine(" Name cannot be empty.");
                return;
            }

            var member = new Member { MemberId = id, MemberName = name };
            service.RegisterMember(member);
            Console.WriteLine(" Member registered. Press any key to return...");
            Console.ReadKey();
        }
        // Method to borrow a book
        static void BorrowBook(ILibraryService service)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║          BORROW BOOK         ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();

            Console.Write(" Book ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId) || bookId <= 0)
            {
                Console.WriteLine(" Invalid Book ID.");
                return;
            }

            Console.Write(" Member ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId) || memberId <= 0)
            {
                Console.WriteLine(" Invalid Member ID.");
                return;
            }

            service.BorrowBook(bookId, memberId);
            Console.WriteLine(" Borrow request processed. Press any key to return...");
            Console.ReadKey();
        }
        // Method to return a borrowed book
        static void ReturnBook(ILibraryService service)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║          RETURN BOOK         ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();

            Console.Write(" Book ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId) || bookId <= 0)
            {
                Console.WriteLine(" Invalid Book ID.");
                return;
            }

            Console.Write(" Member ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId) || memberId <= 0)
            {
                Console.WriteLine(" Invalid Member ID.");
                return;
            }

            service.ReturnBook(bookId, memberId);
            Console.WriteLine(" Return request processed. Press any key to return...");
            Console.ReadKey();
        }
        // Method to view all books
        static void ViewBooks(IBookRepository repo)
        {
            var books = repo.GetAllBooks();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                           BOOK LIST                              ║");
            Console.WriteLine("╠════╦════════════════════╦════════════════════╦═══════════════════╣");
            Console.WriteLine("║ ID ║       Title        ║      Author        ║     Availability  ║");
            Console.WriteLine("╠════╬════════════════════╬════════════════════╬═══════════════════╣");
            Console.ResetColor();

            foreach (var book in books)
            {
                Console.ForegroundColor = book.IsAvailable ? ConsoleColor.Green : ConsoleColor.Red;
                string status = book.IsAvailable ? " Available" : " Borrowed";
                Console.WriteLine($"║ {book.BookId,-2} ║ {Truncate(book.Title, 20),-20} ║ {Truncate(book.Author, 20),-20} ║ {status,-15} ║");
            }
            Console.ResetColor();
            Console.WriteLine("╚════╩════════════════════╩════════════════════╩═══════════════════╝");

            // Utility method to safely truncate text
            string Truncate(string text, int maxLength) =>
                text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";

        }
        // Method to view all members
        static void ViewMembers(IMemberRepository repo)
        {
            var members = repo.GetAllMembers();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("╔═════════════════════════════════════════════╗");
            Console.WriteLine("║                 MEMBER LIST                 ║");
            Console.WriteLine("╠════╦════════════════════════════════════════╣");
            Console.WriteLine("║ ID ║              Member Name               ║");
            Console.WriteLine("╠════╬════════════════════════════════════════╣");
            Console.ResetColor();

            foreach (var member in members)
            {
                Console.WriteLine($"║ {member.MemberId,-2} ║ {Truncate(member.MemberName, 35),-35} ║");
            }

            Console.WriteLine("╚════╩════════════════════════════════════════╝");

            // Helper to truncate long names
            string Truncate(string text, int maxLength) =>
                text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";

        }
        // Method to view all borrow records
        static void ViewBorrowRecords(IBorrowRecordRepository repo)
        {
            var records = repo.GetAllBorrowRecord();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                BORROW RECORDS                                          ║");
            Console.WriteLine("╠════╦════════╦══════════╦════════════════════════════╦══════════════════════════════════╣");
            Console.WriteLine("║ ID ║ BookID ║ MemberID ║       Borrow Date          ║         Return Status            ║");
            Console.WriteLine("╠════╬════════╬══════════╬════════════════════════════╬══════════════════════════════════╣");
            Console.ResetColor();

            foreach (var record in records)
            {
                string borrowDate = record.BorrowDate.ToString("yyyy-MM-dd HH:mm");
                string returnStatus;
                Console.ForegroundColor = ConsoleColor.Gray;

                if (record.ReturnDate == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    returnStatus = " Not Returned";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    returnStatus = $" Returned on {record.ReturnDate.Value:yyyy-MM-dd}";
                }

                Console.WriteLine($"║ {record.BorrowRecordId,-2} ║ {record.BookId,-6} ║ {record.MemberId,-8} ║ {borrowDate,-26} ║ {returnStatus,-30} ║");
            }
            Console.ResetColor();
            Console.WriteLine("╚════╩════════╩══════════╩════════════════════════════╩══════════════════════════════════╝");

        }
    }
}
