using EntityFun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    public interface IDogService
    {
        Task<int> AddDog(Dog dog);
        Task MakeFriend(Dog dog, Dog friend);
    }
}
