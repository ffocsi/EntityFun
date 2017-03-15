using EntityFun.Core;
using EntityFun.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class DogService : IDogService
    {
        /// <summary>
        /// Demonstrates how to add an item in Entity Framework
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        public async Task<int> AddDog(Dog dog)
        {
            using (var context = EntityFunDbContext.Create())
            {
                context.Dogs.Add(dog);
                await context.SaveChangesAsync();
                return await Task.FromResult(dog.Id);
            }
        }

        /// <summary>
        /// Demonstrates a performance way of adding disconnected items to many to many table
        /// </summary>
        /// <param name="dog"></param>
        /// <param name="friend"></param>
        public async Task MakeFriend(Dog dog, Dog friend)
        {
            using (var context = EntityFunDbContext.Create())
            {
                var friendshipExists = context.Dogs.Any(x => x.Id == dog.Id && x.Friends.Any(y => y.Id == friend.Id));
                var dogsExist = context.Dogs.Count(x => x.Id == dog.Id || x.Id == friend.Id) == 2;

                if (!friendshipExists && dogsExist)
                {
                    dog.Friends = new List<Dog>();
                    friend.Friends = new List<Dog>();

                    context.Dogs.Attach(dog);
                    context.Dogs.Attach(friend);

                    dog.Friends.Add(friend);
                    friend.Friends.Add(dog);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
