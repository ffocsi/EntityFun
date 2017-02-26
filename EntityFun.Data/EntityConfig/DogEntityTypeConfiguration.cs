using EntityFun.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFun.Data.EntityConfig
{
    public class DogEntityTypeConfiguration : EntityTypeConfiguration<Dog>
    {
        public DogEntityTypeConfiguration()
        {
            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.Id);
            HasOptional(x => x.Owner);
            HasMany(x => x.Friends)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("LeftDog_Id");
                    x.MapRightKey("RightDog_Id");
                    x.ToTable("DogFriendships");
                });
        }
    }
}
