using LibraryOOP.Models;
using System.Collections.Generic;
using System.IO;
using System;

namespace LibraryOOP
{
    public class FileContext
    {
        private const string BooksFile = "books.txt";
        private const string MembersFile = "members.txt";
        private const string RecordsFile = "borrow_records.txt";

        // ========== BOOKS ==========
        // Load and save books from/to a file
        public List<Book> LoadBooks()
        {
            var books = new List<Book>();
            if (!File.Exists(BooksFile)) return books;

            foreach (var line in File.ReadAllLines(BooksFile))
            {
                var parts = line.Split('|');
                if (parts.Length == 4)
                {
                    books.Add(new Book
                    {
                        BookId = int.Parse(parts[0]),
                        Title = parts[1],
                        Author = parts[2],
                        IsAvailable = bool.Parse(parts[3])
                    });
                }
            }

            return books;
        }

        public void SaveBooks(List<Book> books)
        {
            using var writer = new StreamWriter(BooksFile);
            foreach (var book in books)
            {
                writer.WriteLine($"{book.BookId}|{book.Title}|{book.Author}|{book.IsAvailable}");
            }
        }

        // ========== MEMBERS ==========
        // Load and save members from/to a file
        public List<Member> LoadMembers()
        {
            var members = new List<Member>();
            if (!File.Exists(MembersFile)) return members;

            foreach (var line in File.ReadAllLines(MembersFile))
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    members.Add(new Member
                    {
                        MemberId = int.Parse(parts[0]),
                        MemberName = parts[1]
                    });
                }
            }

            return members;
        }

        public void SaveMembers(List<Member> members)
        {
            using var writer = new StreamWriter(MembersFile);
            foreach (var member in members)
            {
                writer.WriteLine($"{member.MemberId}|{member.MemberName}");
            }
        }

        // ========== BORROW RECORDS ==========
        // Load and save borrow records from/to a file
        public List<BorrowRecord> LoadBorrowRecords()
        {
            var records = new List<BorrowRecord>();
            if (!File.Exists(RecordsFile)) return records;

            foreach (var line in File.ReadAllLines(RecordsFile))
            {
                var parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    records.Add(new BorrowRecord
                    {
                        BorrowRecordId = int.Parse(parts[0]),
                        BookId = int.Parse(parts[1]),
                        MemberId = int.Parse(parts[2]),
                        BorrowDate = DateTime.Parse(parts[3]),
                        ReturnDate = parts.Length > 4 && !string.IsNullOrEmpty(parts[4]) ? DateTime.Parse(parts[4]) : (DateTime?)null
                    });
                }
            }

            return records;
        }

        public void SaveBorrowRecords(List<BorrowRecord> records)
        {
            using var writer = new StreamWriter(RecordsFile);
            foreach (var record in records)
            {
                writer.WriteLine($"{record.BorrowRecordId}|{record.BookId}|{record.MemberId}|{record.BorrowDate}|{record.ReturnDate}");
            }
        }
    }
}
