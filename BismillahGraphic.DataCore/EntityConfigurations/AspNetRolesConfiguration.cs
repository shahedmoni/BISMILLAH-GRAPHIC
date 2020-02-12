using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class AspNetRolesConfiguration : IEntityTypeConfiguration<AspNetRoles>
    {
        public void Configure(EntityTypeBuilder<AspNetRoles> entity)
        {

            entity.HasIndex(e => e.Name)
                .HasName("RoleNameIndex")
                .IsUnique();

            entity.Property(e => e.Id).HasMaxLength(128);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

        }
    }
}
