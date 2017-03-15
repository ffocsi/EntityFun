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
    public interface IHumanService
    {
        [OperationContract]
        Task<int> AddHumanAsync(Human human);
        [OperationContract]
        Task AdoptDogAsync(Human human, Dog dog);
        [OperationContract]
        int AddHumanSync(Human human);
        [OperationContract]
        void AdoptDogSync(Human human, Dog dog);
    }
}
