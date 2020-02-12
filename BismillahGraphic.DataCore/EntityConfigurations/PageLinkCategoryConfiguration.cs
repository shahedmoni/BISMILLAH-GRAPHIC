using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class PageLinkCategoryConfiguration : IEntityTypeConfiguration<PageLinkCategory>
    {
        public void Configure(EntityTypeBuilder<PageLinkCategory> entity)
        {

            entity.HasKey(e => e.LinkCategoryID);
            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.IconClass).HasMaxLength(128);


        }
    }
}
