using EntityFun.Core;
using EntityFun.Data;
using EntityFun.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceModelEx;

namespace EntityFun.Terminal
{
    public class Program
    {
        private static readonly Random _random = new Random();
        private static readonly Stopwatch _stopWatch = new Stopwatch();

        private static readonly string[] _forenames = {
            "Bob", "Frank", "Sarah", "Zoidberg"
        };

        private static readonly string[] _surnames = {
            "Zappa", "Jones", "Murphy", "Moss", "Williams", "Lewis"
        };

        private static readonly string[] _dognames =
        {
            "Fido", "Bonny", "Zoidberg", "Cujo"
        };

        private static void DoEverythingSync()
        {
            var useWcf = true;
            IHumanService humanService;
            IDogService dogService;
            if (useWcf)
            {
                humanService = InProcFactory.CreateInstance<HumanService, IHumanService>();
                dogService = InProcFactory.CreateInstance<DogService, IDogService>();
            }
            else
            {
                humanService = new HumanService();
                dogService = new DogService();
            }

            var humanRecord = new List<Human>();
            var dogRecord = new List<Dog>();

            for (int i = 0; i < 1000; i++)
            {
                var newHuman = new Human
                {
                    DateOfBirth = DateTime.Today.AddYears(_random.Next(20, 50) * -1),
                    Forename = _forenames[_random.Next(0, _forenames.Length)],
                    Surname = _surnames[_random.Next(0, _surnames.Length)]
                };

                newHuman.Id = humanService.AddHumanSync(newHuman);
                humanRecord.Add(newHuman);
            }

            for (int i = 0; i < 1000; i++)
            {
                var newDog = new Dog
                {
                    Breed = (DogBreed)_random.Next(1, 3),
                    DateOfBirth = DateTime.Today.AddYears(_random.Next(3, 12) * -1),
                    Name = _dognames[_random.Next(0, _dognames.Length)]
                };

                newDog.Id = dogService.AddDogSync(newDog);
                dogRecord.Add(newDog);
            }

            for (int i = 0; i < 1000; i++)
            {
                var firstFriendId = _random.Next(0, 999);
                dogService.MakeFriendSync(dogRecord[i], dogRecord[firstFriendId]);

                var secondFriendId = _random.Next(0, 999);
                while (secondFriendId == firstFriendId)
                {
                    secondFriendId = _random.Next(0, 999);
                }
                dogService.MakeFriendSync(dogRecord[i], dogRecord[secondFriendId]);
            }

            for (int i = 0; i < 1000; i++)
            {
                humanService.AdoptDogSync(humanRecord[i], dogRecord[i]);
            }

            using (var context = EntityFunDbContext.Create())
            {
                var dogCount = context.Dogs.Count();
                Console.WriteLine("There are {0} dogs in the system", dogCount);

                var humanCount = context.Humans.Count();
                Console.WriteLine("There are {0} humans in the system", humanCount);

                var dogFriends = context.Dogs.SelectMany(x => x.Friends).Count();
                Console.WriteLine(dogFriends);

                var dogsWithManyFriends = context.Dogs.Count(x => x.Friends.Count > 2);
                Console.WriteLine("{0} dogs have more than 2 friends", dogsWithManyFriends);

                Console.WriteLine("{0} dogs have 0 friends", context.Dogs.Count(x => x.Friends.Count == 0));
            }
        }

        private static async Task DoEverything()
        {
            var useWcf = true;
            IHumanService humanService;
            IDogService dogService;
            if (useWcf)
            {
                humanService = InProcFactory.CreateInstance<HumanService, IHumanService>();
                dogService = InProcFactory.CreateInstance<DogService, IDogService>();
            }
            else
            {
                humanService = new HumanService();
                dogService = new DogService();
            }

            var humanRecord = new List<Human>();
            var dogRecord = new List<Dog>();

            var numberOfIterations = 100;

            var humanAddTasks = Enumerable.Range(0, numberOfIterations)
                .Select(async i =>
                {
                    var newHuman = new Human
                    {
                        DateOfBirth = DateTime.Today.AddYears(_random.Next(20, 50) * -1),
                        Forename = _forenames[_random.Next(0, _forenames.Length)],
                        Surname = _surnames[_random.Next(0, _surnames.Length)]
                    };

                    newHuman.Id = await humanService.AddHumanAsync(newHuman);
                    humanRecord.Add(newHuman);
                });
            await Task.WhenAll(humanAddTasks);

            var dogAddTasks = Enumerable.Range(0, numberOfIterations)
                .Select(async i =>
                {
                    var newDog = new Dog
                    {
                        Breed = (DogBreed)_random.Next(1, 3),
                        DateOfBirth = DateTime.Today.AddYears(_random.Next(3, 12) * -1),
                        Name = _dognames[_random.Next(0, _dognames.Length)]
                    };

                    newDog.Id = await dogService.AddDogAsync(newDog);
                    dogRecord.Add(newDog);
                });
            await Task.WhenAll(dogAddTasks);
            
            var dogFriendTasks = Enumerable.Range(0, numberOfIterations)
                .Select(async i =>
                {
                    var firstFriendId = _random.Next(0, numberOfIterations - 1);
                    await dogService.MakeFriendAsync(dogRecord[i], dogRecord[firstFriendId]);

                    var secondFriendId = _random.Next(0, numberOfIterations - 1);
                    while (secondFriendId == firstFriendId)
                    {
                        secondFriendId = _random.Next(0, numberOfIterations - 1);
                    }

                    await dogService.MakeFriendAsync(new Dog { Id = i }, new Dog { Id = secondFriendId });
                });
            await Task.WhenAll(dogFriendTasks);

            var adoptDogTasks = Enumerable.Range(0, numberOfIterations)
                .Select(async i =>
                {
                    await humanService.AdoptDogAsync(humanRecord[i], dogRecord[i]);
                });
            await Task.WhenAll(adoptDogTasks);

            using (var context = EntityFunDbContext.Create())
            {
                var dogCount = context.Dogs.Count();
                Console.WriteLine("There are {0} dogs in the system", dogCount);

                var humanCount = context.Humans.Count();
                Console.WriteLine("There are {0} humans in the system", humanCount);

                var dogFriends = context.Dogs.SelectMany(x => x.Friends).Count();
                Console.WriteLine("There are {0} dog friend combos", dogFriends);

                var dogsWithManyFriends = context.Dogs.Count(x => x.Friends.Count > 2);
                Console.WriteLine("{0} dogs have more than 2 friends", dogsWithManyFriends);

                Console.WriteLine("{0} dogs have 0 friends", context.Dogs.Count(x => x.Friends.Count == 0));
            }
        }

        public static void Main(string[] args)
        {
            _stopWatch.Start();
            DoEverything().Wait();
            //DoEverythingSync();
            _stopWatch.Stop();
            var elapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
            var elapsedTicks = _stopWatch.ElapsedTicks;
            Console.WriteLine("Everything took {0} milliseconds and {1} ticks!", elapsedMilliseconds, elapsedTicks);

            _stopWatch.Restart();
            using (var context = EntityFunDbContext.Create())
            {
                var allDogs = context.Dogs
                    .Include(x => x.Friends)
                    .Include(x => x.Owner)
                    .ToList();
                foreach (var dog in allDogs)
                {
                    Debug.WriteLine("{0} is a great dog, his owner is {1} {2} and his best friend is {3}", dog.Name, dog.Owner.Forename, dog.Owner.Surname, dog.Friends.First().Name);
                }
            }
            _stopWatch.Stop();
            elapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
            elapsedTicks = _stopWatch.ElapsedTicks;
            Console.WriteLine("All stuff took {0} milliseconds and {1} ticks!", elapsedMilliseconds, elapsedTicks);

            _stopWatch.Restart();
            using (var context = EntityFunDbContext.Create())
            {
                var allDogs = context.Dogs
                    .Select(x => new
                    {
                        x.Name,
                        Owner = new
                        {
                            Forename = x.Owner.Forename,
                            Surname = x.Owner.Surname
                        },
                        Friends = x.Friends.Select(y => new
                        {
                            y.Name
                        })
                    })
                    .ToList()
                    .Select(x => new Dog
                    {
                        Name = x.Name,
                        Owner = new Human
                        {
                            Forename = x.Owner.Forename,
                            Surname = x.Owner.Surname
                        },
                        Friends = x.Friends.Select(y => new Dog
                        {
                            Name = y.Name
                        }).ToList()
                    });
                foreach (var dog in allDogs)
                {
                    Debug.WriteLine("{0} is a great dog, his owner is {1} {2} and his best friend is {3}", dog.Name, dog.Owner.Forename, dog.Owner.Surname, dog.Friends.First().Name);
                }
            }
            _stopWatch.Stop();
            elapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
            elapsedTicks = _stopWatch.ElapsedTicks;
            Console.WriteLine("Limited stuff took {0} milliseconds and {1} ticks!", elapsedMilliseconds, elapsedTicks);
            var dogService = new DogService();
            var dog1 = new Dog { Id = 1 };
            var dog2 = new Dog { Id = _random.Next(400, 800) };

            dogService.MakeFriendAsync(dog1, dog2).Wait();
            using (var context = EntityFunDbContext.Create())
            {
                var test = context.Dogs
                    .Include(x => x.Friends)
                    .First();

                foreach (var friend in test.Friends)
                {
                    Console.WriteLine("{0} - {1}", friend.Id, friend.Name);
                }
            }

            var newDog = dogService.AddDogAsync(new Dog { Name = "Brenford", DateOfBirth = DateTime.Now.AddYears(-2) });
            Console.ReadKey();
        }
    }
}
