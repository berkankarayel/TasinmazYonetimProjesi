using Microsoft.EntityFrameworkCore;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Il> Iller { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }
        public DbSet<Mahalle> Mahalleler { get; set; }
        public DbSet<Tasinmaz> Tasinmazlar { get; set; }
        public DbSet<Log> Loglar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<Tasinmaz>()
                .HasOne(t => t.Kullanici)
                .WithMany()
                .HasForeignKey(t => t.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
