using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Core
{
    public class Dog
    {
        public int Id { get; set; }
        public Guid UId { get; set; }
        public string Name { get; set; }
        public DogBreed Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Human Owner { get; set; }
        public ICollection<Dog> Friends { get; set; }
    }
}
