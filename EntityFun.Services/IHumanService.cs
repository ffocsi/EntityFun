using EntityFun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    public interface IHumanService
    {
        Task<int> AddHuman(Human human);
        Task AdoptDog(Human human, Dog dog);
    }
}
