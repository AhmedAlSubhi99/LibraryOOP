using LibraryOOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.Repositories
{
    public interface IMemberRepository
    {
        List<Member> GetAllMembers();// Method to get all members
        Member? GetMemberById(int memberId); // Method to get a member by ID
        void AddMember(Member member); // Method to add a new member
        void UpdateMember(Member member); // Method to update an existing member
        void DeleteMember(int memberId); // Method to delete a member by ID
    }
}
