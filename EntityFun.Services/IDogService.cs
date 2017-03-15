using EntityFun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    [ServiceContract]
    public interface IDogService
    {
        [OperationContract]
        Task<int> AddDog(Dog dog);
        [OperationContract]
        Task MakeFriend(Dog dog, Dog friend);
    }
}
