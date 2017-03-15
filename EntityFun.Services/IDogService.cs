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
        Task<int> AddDogAsync(Dog dog);
        [OperationContract]
        Task MakeFriendAsync(Dog dog, Dog friend);
        [OperationContract]
        int AddDogSync(Dog dog);
        [OperationContract]
        void MakeFriendSync(Dog dog, Dog friend);
    }
}
