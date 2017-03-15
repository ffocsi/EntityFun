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
        Task<int> AddHuman(Human human);
        [OperationContract]
        Task AdoptDog(Human human, Dog dog);
    }
}
