using EntityFun.Core;
using EntityFun.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class HumanService : IHumanService
    {
        /// <summary>
        /// Demonstrates how to add an item in Entity Framework
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        public async Task<int> AddHumanAsync(Human human)
        {
            using (var context = EntityFunDbContext.Create())
            {
                context.Humans.Add(human);
                await context.SaveChangesAsync();
                return await Task.FromResult(human.Id);
            }
        }

        /// <summary>
        /// Demonstrates a performant way of changing a navigation property disconnected
        /// </summary>
        /// <param name="human"></param>
        /// <param name="dog"></param>
        public async Task AdoptDogAsync(Human human, Dog dog)
        {
            using (var context = EntityFunDbContext.Create())
            {
                try
                {
                    context.Humans.Attach(human);
                    context.Dogs.Attach(dog);

                    dog.Owner = human;

                    await context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }
                
            }
        }
    }
}
