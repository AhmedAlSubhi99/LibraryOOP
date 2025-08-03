using LibraryOOP.Models;
using LibraryOOP.Repositories;
using System;
using System.Linq;

namespace LibraryOOP.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepo; // Repository for managing books
        private readonly IMemberRepository _memberRepo; // Repository for managing members
        private readonly IBorrowRecordRepository _recordRepo; // Repository for managing borrow records

        // Constructor to initialize the repositories
        public LibraryService(IBookRepository bookRepo, IMemberRepository memberRepo, IBorrowRecordRepository recordRepo)
        {
            _bookRepo = bookRepo;
            _memberRepo = memberRepo;
            _recordRepo = recordRepo;
        }
        // Method to add a new book to the library
        public void AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                Console.WriteLine(" Book Title and Author cannot be empty.");
                return;
            }

            if (_bookRepo.GetBookById(book.BookId) != null)
            {
                Console.WriteLine(" A book with this ID already exists.");
                return;
            }

            _bookRepo.AddBook(book);
            Console.WriteLine(" Book added successfully.");
        }
        // Method to update an existing book in the library
        public void RegisterMember(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.MemberName))
            {
                Console.WriteLine(" Member name cannot be empty.");
                return;
            }

            if (_memberRepo.GetMemberById(member.MemberId) != null)
            {
                Console.WriteLine(" A member with this ID already exists.");
                return;
            }

            _memberRepo.AddMember(member);
            Console.WriteLine(" Member registered.");
        }
        // Method to borrow a book from the library
        public void BorrowBook(int bookId, int memberId)
        {
            var book = _bookRepo.GetBookById(bookId);
            if (book == null)
            {
                Console.WriteLine(" Book not found.");
                return;
            }

            if (!book.IsAvailable)
            {
                Console.WriteLine(" Book is currently not available.");
                return;
            }

            var member = _memberRepo.GetMemberById(memberId);
            if (member == null)
            {
                Console.WriteLine(" Member not found.");
                return;
            }

            var alreadyBorrowed = _recordRepo.GetBorrowRecordByBookId(bookId)
                .Any(r => r.MemberId == memberId && r.ReturnDate == null);
            if (alreadyBorrowed)
            {
                Console.WriteLine(" This member already borrowed this book.");
                return;
            }

            var borrowRecord = new BorrowRecord
            {
                BorrowRecordId = GenerateBorrowRecordId(),
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };

            book.IsAvailable = false;

            _bookRepo.UpdateBook(book);
            _recordRepo.AddBorrowRecord(borrowRecord);

            Console.WriteLine(" Book borrowed successfully.");
        }
        // Method to return a borrowed book to the library
        public void ReturnBook(int bookId, int memberId)
        {
            var book = _bookRepo.GetBookById(bookId);
            if (book == null)
            {
                Console.WriteLine(" Book not found.");
                return;
            }

            var borrowRecord = _recordRepo.GetBorrowRecordByBookId(bookId)
                .FirstOrDefault(r => r.MemberId == memberId && r.ReturnDate == null);

            if (borrowRecord == null)
            {
                Console.WriteLine(" No active borrow record found for this member and book.");
                return;
            }

            borrowRecord.ReturnDate = DateTime.Now;
            book.IsAvailable = true;

            _bookRepo.UpdateBook(book);
            _recordRepo.UpdateBorrowRecord(borrowRecord);

            Console.WriteLine(" Book returned successfully.");
        }
        // Method to view all books in the library
        private int GenerateBorrowRecordId()
        {
            var records = _recordRepo.GetAllBorrowRecord();
            return records.Any() ? records.Max(r => r.BorrowRecordId) + 1 : 1;
        }
    }
}
