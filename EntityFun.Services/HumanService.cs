using EntityFun.Core;
using EntityFun.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    public class HumanService
    {
        public int AddHuman(Human human)
        {
            using (var context = EntityFunDbContext.Create())
            {
                context.Humans.Add(human);
                context.SaveChanges();
                return human.Id;
            }
        }

        public void AdoptDog(Human human, Dog dog)
        {
            using (var context = EntityFunDbContext.Create())
            {
                context.Humans.Attach(human);
                context.Dogs.Attach(dog);

                dog.Owner = human;

                context.SaveChanges();
            }
        }
    }
}
