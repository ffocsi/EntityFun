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
        /// <summary>
        /// Demonstrates how to add an item in Entity Framework
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        public int AddHuman(Human human)
        {
            using (var context = EntityFunDbContext.Create())
            {
                context.Humans.Add(human);
                context.SaveChanges();
                return human.Id;
            }
        }

        /// <summary>
        /// Demonstrates a performant way of changing a navigation property disconnected
        /// </summary>
        /// <param name="human"></param>
        /// <param name="dog"></param>
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
