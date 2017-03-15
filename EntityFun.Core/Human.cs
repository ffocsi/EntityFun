using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Core
{
    [DataContract(IsReference = true)]
    public class Human
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid UId { get; set; }

        [DataMember]
        public string Forename { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public ICollection<Dog> Dogs { get; set; }
    }
}
