using LibraryOOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.Services
{
    public interface ILibraryService
    {
        void BorrowBook(int bookId, int memberId); // Method to borrow a book
        void ReturnBook(int bookId, int memberId); // Method to return a borrowed book
        void RegisterMember(Member member); // Method to register a new member
        void AddBook(Book book); // Method to add a new book to the library
    }
}
