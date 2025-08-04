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
                Console.WriteLine("  8  View Returned Books");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  0  Exit");
                Console.ResetColor();

                Console.Write("\nSelect an option: ");
                string input = Console.ReadLine()?? "";

                switch (input)
                {
                    case "1": AddBook(libraryService); break;
                    case "2": RegisterMember(libraryService); break;
                    case "3": BorrowBook(libraryService, bookRepo, memberRepo); break;
                    case "4": ReturnBook(libraryService, recordRepo, bookRepo, memberRepo); break;
                    case "5": ViewBooks(bookRepo); break;
                    case "6": ViewMembers(memberRepo); break;
                    case "7": ViewBorrowRecords(recordRepo, bookRepo, memberRepo); break;
                    case "8": ViewReturnedBooks(recordRepo, bookRepo, memberRepo); break;
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
        static void BorrowBook(ILibraryService service, IBookRepository bookRepo, IMemberRepository memberRepo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║          BORROW BOOK         ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();

            // Show available books
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nAvailable Books:");
            Console.ResetColor();

            var books = bookRepo.GetAllBooks().Where(b => b.IsAvailable).ToList();
            if (books.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No available books to borrow.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.WriteLine("╔════╦════════════════════════╦════════════════════╗");
            Console.WriteLine("║ ID ║         Title          ║       Author      ║");
            Console.WriteLine("╠════╬════════════════════════╬════════════════════╣");

            foreach (var book in books)
            {
                Console.WriteLine($"║ {book.BookId,-2} ║ {Truncate(book.Title, 24),-24} ║ {Truncate(book.Author, 20),-20} ║");
            }
            Console.WriteLine("╚════╩════════════════════════╩════════════════════╝");

            // Show member list
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRegistered Members:");
            Console.ResetColor();

            var members = memberRepo.GetAllMembers();
            if (members.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No members registered.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.WriteLine("╔════╦════════════════════════════════════════╗");
            Console.WriteLine("║ ID ║              Member Name               ║");
            Console.WriteLine("╠════╬════════════════════════════════════════╣");

            foreach (var member in members)
            {
                Console.WriteLine($"║ {member.MemberId,-2} ║ {Truncate(member.MemberName, 40),-40} ║");
            }
            Console.WriteLine("╚════╩════════════════════════════════════════╝");

            // Now ask for input
            Console.Write("\n Book ID: ");
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

            string Truncate(string text, int maxLength) =>
                text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }

        // Method to return a borrowed book
        static void ReturnBook(ILibraryService service, IBorrowRecordRepository recordRepo, IBookRepository bookRepo, IMemberRepository memberRepo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║          RETURN BOOK         ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();

            // Get borrowed books only (ReturnDate == null)
            var borrowedRecords = recordRepo.GetAllBorrowRecord()
                                             .Where(r => r.ReturnDate == null)
                                             .ToList();

            if (borrowedRecords.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No borrowed books found. Nothing to return.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nCurrently Borrowed Books:");
            Console.ResetColor();

            Console.WriteLine("╔════╦════════════╦════════════════════╦══════════════╦══════════════════════════════╗");
            Console.WriteLine("║ ID ║  Book ID   ║     Book Title     ║  Member ID   ║        Member Name           ║");
            Console.WriteLine("╠════╬════════════╬════════════════════╬══════════════╬══════════════════════════════╣");

            foreach (var record in borrowedRecords)
            {
                var book = bookRepo.GetAllBooks().FirstOrDefault(b => b.BookId == record.BookId);
                var member = memberRepo.GetAllMembers().FirstOrDefault(m => m.MemberId == record.MemberId);

                if (book != null && member != null)
                {
                    Console.WriteLine($"║ {record.BorrowRecordId,-2} ║ {book.BookId,-10} ║ {Truncate(book.Title, 20),-20} ║ {member.MemberId,-12} ║ {Truncate(member.MemberName, 28),-28} ║");
                }
            }

            Console.WriteLine("╚════╩════════════╩════════════════════╩══════════════╩══════════════════════════════╝");

            // Input Section
            Console.Write("\n Book ID: ");
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

            string Truncate(string text, int maxLength) =>
                text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
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
        static void ViewBorrowRecords(IBorrowRecordRepository recordRepo, IBookRepository bookRepo, IMemberRepository memberRepo)
        {
            var records = recordRepo.GetAllBorrowRecord();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("╔════════╦════════════╦════════════════════╦════════════╦════════════════════════════╦══════════════════════════════╗");
            Console.WriteLine("║ Rec ID ║  Book ID   ║     Book Title     ║ Member ID  ║        Member Name         ║        Return Status         ║");
            Console.WriteLine("╠════════╬════════════╬════════════════════╬════════════╬════════════════════════════╬══════════════════════════════╣");
            Console.ResetColor();

            foreach (var record in records)
            {
                var book = bookRepo.GetAllBooks().FirstOrDefault(b => b.BookId == record.BookId);
                var member = memberRepo.GetAllMembers().FirstOrDefault(m => m.MemberId == record.MemberId);

                string bookTitle = book != null ? Truncate(book.Title, 20) : "Unknown";
                string memberName = member != null ? Truncate(member.MemberName, 24) : "Unknown";

                string returnStatus;
                if (record.ReturnDate == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    returnStatus = "Not Returned";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    returnStatus = $"Returned on {record.ReturnDate.Value:yyyy-MM-dd}";
                }

                Console.WriteLine($"║ {record.BorrowRecordId,-6} ║ {record.BookId,-10} ║ {bookTitle,-20} ║ {record.MemberId,-10} ║ {memberName,-26} ║ {returnStatus,-28} ║");
            }

            Console.ResetColor();
            Console.WriteLine("╚════════╩════════════╩════════════════════╩════════════╩════════════════════════════╩══════════════════════════════╝");

            // Helper method to truncate long strings
            string Truncate(string text, int maxLength) =>
                text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
        // Method to view all returned books
        static void ViewReturnedBooks(IBorrowRecordRepository borrowRepo, IBookRepository bookRepo, IMemberRepository memberRepo)
        {
            var records = borrowRepo.GetAllBorrowRecord().Where(r => r.ReturnDate != null).ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                               RETURNED BOOK RECORDS                                 ║");
            Console.WriteLine("╠════╦══════════════╦════════════════════╦══════════════╦══════════════════════════════╣");
            Console.WriteLine("║ ID ║   Book ID    ║     Book Title     ║  Member ID   ║        Member Name           ║");
            Console.WriteLine("╠════╬══════════════╬════════════════════╬══════════════╬══════════════════════════════╣");
            Console.ResetColor();

            foreach (var record in records)
            {
                var book = bookRepo.GetAllBooks().FirstOrDefault(b => b.BookId == record.BookId);
                var member = memberRepo.GetAllMembers().FirstOrDefault(m => m.MemberId == record.MemberId);

                if (book != null && member != null)
                {
                    Console.WriteLine($"║ {record.BorrowRecordId,-2} ║ {book.BookId,-12} ║ {Truncate(book.Title, 20),-20} ║ {member.MemberId,-12} ║ {Truncate(member.MemberName, 28),-28} ║");
                }
            }

            Console.WriteLine("╚════╩══════════════╩════════════════════╩══════════════╩══════════════════════════════╝");

            string Truncate(string text, int maxLength) =>
                text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }

    }
}
