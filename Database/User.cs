using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Database
{
    public class User
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Post { get; set; }
        public string StructDivision { get; set; }
        public bool IsAdmin { get; set; }

        public User(string username, string fullname, string post, string structDivision, bool isAdmin)
        {
            Username = username;
            Fullname = fullname;
            Post = post;
            StructDivision = structDivision;
            IsAdmin = isAdmin;
        }
    }
}
