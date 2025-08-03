using LibraryOOP.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryOOP.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly FileContext _fileContext;

        // Constructor to initialize the file context for loading and saving books
        public BookRepository(FileContext fileContext)
        {
            _fileContext = fileContext;
        }
        // Method to load and save books from/to a file
        public List<Book> GetAllBooks()
        {
            return _fileContext.LoadBooks();
        }
        // Method to get a book by ID
        public Book? GetBookById(int bookId)
        {
            return GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
        }
        // Method to add a new book
        public void AddBook(Book book)
        {
            var books = GetAllBooks();
            books.Add(book);
            _fileContext.SaveBooks(books);
        }
        // Method to update an existing book
        public void UpdateBook(Book book)
        {
            var books = GetAllBooks();
            var index = books.FindIndex(b => b.BookId == book.BookId);
            if (index != -1)
            {
                books[index] = book;
                _fileContext.SaveBooks(books);
            }
        }
        // Method to delete a book by ID
        public void DeleteBook(int bookId)
        {
            var books = GetAllBooks();
            var bookToRemove = books.FirstOrDefault(b => b.BookId == bookId);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                _fileContext.SaveBooks(books);
            }
        }
    }

}
