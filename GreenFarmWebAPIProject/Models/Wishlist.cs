namespace GreenFarmWebAPIProject.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<WishlistProduct> WishlistProducts { get; set; }
    }
}
