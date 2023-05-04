using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FashionStore.Models;

namespace FashionStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FashionStore.Models.ColorModel>? ColorModel { get; set; }
    }
    //public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<FashioUser>
    //{
    //    public void Configure(EntityTypeBuilder<FashioUser> builder)
    //    {
    //        builder.Property(x => x.Name).HasMaxLength(100);
    //        //throw new NotImplementedException();
    //    }
    //}
}