using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class PageLinkAssignConfiguration : IEntityTypeConfiguration<PageLinkAssign>
    {
        public void Configure(EntityTypeBuilder<PageLinkAssign> entity)
        {

            entity.HasKey(e => e.LinkAssignID);

            entity.HasOne(d => d.PageLink)
                .WithMany(p => p.PageLinkAssign)
                .HasForeignKey(d => d.LinkID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PageLinkAssign_PageLink");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.PageLinkAssign)
                .HasForeignKey(d => d.RegistrationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PageLinkAssign_Registration");

        }
    }
}
