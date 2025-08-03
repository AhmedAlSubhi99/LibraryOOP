using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.Models
{
    public class Member
    {
        public int MemberId { get; set; } // Unique identifier for the member
        public string MemberName { get; set; } = string.Empty; // Name of the member
    }
}
