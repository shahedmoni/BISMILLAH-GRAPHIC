using Microsoft.EntityFrameworkCore;

namespace BismillahGraphic.DataCore
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Expanse> Expanse { get; set; }
        public virtual DbSet<ExpanseCategory> ExpanseCategory { get; set; }
        public virtual DbSet<Institution> Institution { get; set; }
        public virtual DbSet<PageLink> PageLink { get; set; }
        public virtual DbSet<PageLinkAssign> PageLinkAssign { get; set; }
        public virtual DbSet<PageLinkCategory> PageLinkCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<Selling> Selling { get; set; }
        public virtual DbSet<SellingList> SellingList { get; set; }
        public virtual DbSet<SellingPaymentRecord> SellingPaymentRecord { get; set; }
        public virtual DbSet<SmsBalance> SmsBalance { get; set; }
        public virtual DbSet<SmsRechargeRecord> SmsRechargeRecord { get; set; }
        public virtual DbSet<SmsSendRecord> SmsSendRecord { get; set; }
        public virtual DbSet<UserLoginInfo> UserLoginInfo { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=.;Database=BismillahGraphic;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AspNetRolesConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserLoginsConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new AspNetUsersConfiguration());

            modelBuilder.ApplyConfiguration(new ExpanseConfiguration());
            modelBuilder.ApplyConfiguration(new ExpanseCategoryConfiguration());

            modelBuilder.ApplyConfiguration(new InstitutionConfiguration());
            modelBuilder.ApplyConfiguration(new PageLinkConfiguration());
            modelBuilder.ApplyConfiguration(new PageLinkAssignConfiguration());
            modelBuilder.ApplyConfiguration(new PageLinkCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new SellingConfiguration());
            modelBuilder.ApplyConfiguration(new SellingListConfiguration());
            modelBuilder.ApplyConfiguration(new SellingPaymentRecordConfiguration());
            modelBuilder.ApplyConfiguration(new SmsBalanceConfiguration());
            modelBuilder.ApplyConfiguration(new SmsRechargeRecordConfiguration());
            modelBuilder.ApplyConfiguration(new SmsSendRecordConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginInfoConfiguration());
            modelBuilder.ApplyConfiguration(new VendorConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
