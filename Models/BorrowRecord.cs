using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.Models
{
    public class BorrowRecord
    {
        public int BorrowRecordId { get; set; } // Unique identifier for the borrow record
        public int BookId { get; set; } // Identifier for the borrowed book
        public int MemberId { get; set; } // Identifier for the member who borrowed the book
        public DateTime BorrowDate { get; set; } // Date when the book was borrowed
        public DateTime? ReturnDate { get; set; } // Date when the book was returned, nullable if not returned yet

    }
}
