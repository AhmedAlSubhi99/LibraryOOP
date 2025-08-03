using LibraryOOP.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryOOP.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly FileContext _fileContext;

        // Constructor to initialize the file context for loading and saving borrow records
        public BorrowRecordRepository(FileContext fileContext)
        {
            _fileContext = fileContext;
        }
        // Method to get all borrow records
        public List<BorrowRecord> GetAllBorrowRecord()
        {
            return _fileContext.LoadBorrowRecords();
        }
        // Method to get a borrow record by ID
        public BorrowRecord? GetBorrowRecordById(int recordId)
        {
            return GetAllBorrowRecord().FirstOrDefault(r => r.BorrowRecordId == recordId);
        }
        // Method to add a new borrow record
        public void AddBorrowRecord(BorrowRecord record)
        {
            var records = GetAllBorrowRecord();
            records.Add(record);
            _fileContext.SaveBorrowRecords(records);
        }
        // Method to update an existing borrow record
        public void UpdateBorrowRecord(BorrowRecord record)
        {
            var records = GetAllBorrowRecord();
            var index = records.FindIndex(r => r.BorrowRecordId == record.BorrowRecordId);
            if (index != -1)
            {
                records[index] = record;
                _fileContext.SaveBorrowRecords(records);
            }
        }
        // Method to get borrow records by member ID
        public List<BorrowRecord> GetBorrowRecordByMemberId(int memberId)
        {
            return GetAllBorrowRecord().Where(r => r.MemberId == memberId).ToList();
        }
        // Method to get borrow records by book ID
        public List<BorrowRecord> GetBorrowRecordByBookId(int bookId)
        {
            return GetAllBorrowRecord().Where(r => r.BookId == bookId).ToList();
        }
    }
}
