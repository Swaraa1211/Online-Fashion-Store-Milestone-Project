using Microsoft.EntityFrameworkCore;
using FashionStoreAPI.Models;
using System.Collections.Generic;

namespace FashionStoreAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<ColorModel>? Colors { get; set; }
        public DbSet<SizeModel>? Size { get; set; }
    }
}
