using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class PageLinkConfiguration : IEntityTypeConfiguration<PageLink>
    {
        public void Configure(EntityTypeBuilder<PageLink> entity)
        {

            entity.HasKey(e => e.LinkID);

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.Controller)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.IconClass).HasMaxLength(128);

            entity.Property(e => e.RoleId).HasMaxLength(128);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasOne(d => d.LinkCategory)
                .WithMany(p => p.PageLink)
                .HasForeignKey(d => d.LinkCategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PageLink_PageLinkCategory");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.PageLink)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_PageLink_PageLink");

        }
    }
}
