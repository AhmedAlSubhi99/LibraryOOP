using LibraryOOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks(); // Method to get all books
        Book? GetBookById(int bookId); // Method to get a book by ID
        void AddBook(Book book); // Method to add a new book
        void UpdateBook(Book book); // Method to update an existing book
        void DeleteBook(int bookId); // Method to delete a book by ID
    }
}
