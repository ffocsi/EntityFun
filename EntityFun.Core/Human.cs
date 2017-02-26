using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Core
{
    public class Human
    {
        public int Id { get; set; }
        public Guid UId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Dog> Dogs { get; set; }
    }
}
