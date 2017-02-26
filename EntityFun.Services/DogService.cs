using EntityFun.Core;
using EntityFun.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    public class DogService
    {
        /// <summary>
        /// Demonstrates how to add an item in Entity Framework
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        public int AddDog(Dog dog)
        {
            using (var context = EntityFunDbContext.Create())
            {
                context.Dogs.Add(dog);
                context.SaveChanges();
                return dog.Id;
            }
        }

        /// <summary>
        /// Demonstrates a performance way of adding disconnected items to many to many table
        /// </summary>
        /// <param name="dog"></param>
        /// <param name="friend"></param>
        public void MakeFriend(Dog dog, Dog friend)
        {
            using (var context = EntityFunDbContext.Create())
            {
                var friendshipExists = context.Dogs.Any(x => x.Id == dog.Id && x.Friends.Any(y => y.Id == friend.Id));

                if (!friendshipExists)
                {
                    dog.Friends = new List<Dog>();
                    friend.Friends = new List<Dog>();

                    context.Dogs.Attach(dog);
                    context.Dogs.Attach(friend);

                    dog.Friends.Add(friend);
                    friend.Friends.Add(dog);

                    context.SaveChanges();
                }
            }
        }
    }
}
