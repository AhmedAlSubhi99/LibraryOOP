using LibraryOOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.Repositories
{
    public interface IBorrowRecordRepository
    {
        List<BorrowRecord> GetAllBorrowRecord(); // Method to get all borrow records
        BorrowRecord? GetBorrowRecordById(int recordId); // Method to get a borrow record by ID
        void AddBorrowRecord(BorrowRecord record); // Method to add a new borrow record
        void UpdateBorrowRecord(BorrowRecord record); // Method to update an existing borrow record
        List<BorrowRecord> GetBorrowRecordByMemberId(int memberId); // Method to get borrow records by member ID
        List<BorrowRecord> GetBorrowRecordByBookId(int bookId); // Method to get borrow records by book ID
    }
}
