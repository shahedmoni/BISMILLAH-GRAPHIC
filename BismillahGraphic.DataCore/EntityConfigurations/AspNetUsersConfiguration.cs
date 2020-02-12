using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class AspNetUsersConfiguration : IEntityTypeConfiguration<AspNetUsers>
    {
        public void Configure(EntityTypeBuilder<AspNetUsers> entity)
        {

            entity.HasIndex(e => e.UserName)
                .HasName("UserNameIndex")
                .IsUnique();

            entity.Property(e => e.Id).HasMaxLength(128);

            entity.Property(e => e.Email).HasMaxLength(256);

            entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(256);

        }
    }
}
