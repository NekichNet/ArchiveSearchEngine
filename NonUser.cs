using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine
{
    public class NonUser
    {
        public NonUser(int id, string fullname, string post, string structDivision)
        {
            Id = id;
            Fullname = fullname;
            Post = post;
            StructDivision = structDivision;
        }

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Post { get; set; }
        public string StructDivision { get; set; }
    }
}
