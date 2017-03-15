using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Core
{
    [DataContract(IsReference = true)]
    public class Dog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid UId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DogBreed Breed { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public Human Owner { get; set; }

        [DataMember]
        public ICollection<Dog> Friends { get; set; }
    }
}
