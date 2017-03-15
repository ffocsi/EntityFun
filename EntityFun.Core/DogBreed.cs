using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Core
{
    [DataContract]
    public enum DogBreed : int
    {
        [EnumMember]
        YorkshireTerrier,
        [EnumMember]
        Labrador,
        [EnumMember]
        Chihuahua
    }
}
