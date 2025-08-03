using LibraryOOP.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryOOP.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly FileContext _fileContext;

        // Method to load and save members from/to a file
        public MemberRepository(FileContext fileContext)
        {
            _fileContext = fileContext;
        }
        //  Method to get all members
        public List<Member> GetAllMembers()
        {
            return _fileContext.LoadMembers();
        }
        // Method to get a member by ID
        public Member? GetMemberById(int memberId)
        {
            return GetAllMembers().FirstOrDefault(m => m.MemberId == memberId);
        }
        // Method to add a new member
        public void AddMember(Member member)
        {
            var members = GetAllMembers();
            members.Add(member);
            _fileContext.SaveMembers(members);
        }
        // Method to update an existing member
        public void UpdateMember(Member member)
        {
            var members = GetAllMembers();
            var index = members.FindIndex(m => m.MemberId == member.MemberId);
            if (index != -1)
            {
                members[index] = member;
                _fileContext.SaveMembers(members);
            }
        }
        // Method to delete a member by ID
        public void DeleteMember(int memberId)
        {
            var members = GetAllMembers();
            var toRemove = members.FirstOrDefault(m => m.MemberId == memberId);
            if (toRemove != null)
            {
                members.Remove(toRemove);
                _fileContext.SaveMembers(members);
            }
        }
    }
}
