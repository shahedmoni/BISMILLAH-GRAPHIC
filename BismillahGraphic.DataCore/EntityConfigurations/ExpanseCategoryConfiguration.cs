using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class ExpanseCategoryConfiguration : IEntityTypeConfiguration<ExpanseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpanseCategory> entity)
        {

            entity.ToTable("Expanse_Category");

            entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.TotalExpanse)
                .HasColumnName("Total_Expanse")
                .HasDefaultValueSql("((0))");

        }
    }
}
