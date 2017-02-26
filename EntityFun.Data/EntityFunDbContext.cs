using EntityFun.Core;
using EntityFun.Data.EntityConfig;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Data
{
    public class EntityFunDbContext : DbContext
    {
        public EntityFunDbContext()
            :base (@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Dogs;Integrated Security=True")
        {
            Database
                .SetInitializer<EntityFunDbContext>(new DropCreateDatabaseAlways<EntityFunDbContext>());
        }

        public static EntityFunDbContext Create()
        {
            return new EntityFunDbContext();
        }

        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Human> Humans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add<Dog>(new DogEntityTypeConfiguration());
            modelBuilder.Configurations.Add<Human>(new HumanEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
