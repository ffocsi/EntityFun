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
    public class HumanEntityTypeConfiguration : EntityTypeConfiguration<Human>
    {
        public HumanEntityTypeConfiguration()
        {
            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(x => x.Id);
            HasMany(x => x.Dogs);

            ToTable("Humans");
        }
    }
}
