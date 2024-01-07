using Microsoft.EntityFrameworkCore;

namespace GreenFarmWebAPIProject.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistProduct> WishlistProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product tablosundaki CategoryId ile Category tablosundaki Id arasında FK ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);  

            // OrderProducts tablosundaki OrderId ile Orders tablosundaki Id arasında FK ilişkisi
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade); 

            // OrderProducts tablosundaki ProductId ile Products tablosundaki Id arasında FK ilişkisi
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Orders tablosundaki UserId ile Users tablosundaki Id arasında FK ilişkisi
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wishlist)
                .HasForeignKey<Wishlist>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<WishlistProduct>()
                .HasOne(wp => wp.Wishlist)
                .WithMany(w => w.WishlistProducts)
                .HasForeignKey(wp => wp.WishlistId)
                .OnDelete(DeleteBehavior.Cascade); 

            // WishlistProducts tablosundaki ProductId ile Products tablosundaki Id arasında FK ilişkisi
            modelBuilder.Entity<WishlistProduct>()
                .HasOne(wp => wp.Product)
                .WithMany(p => p.WishlistProducts)
                .HasForeignKey(wp => wp.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
