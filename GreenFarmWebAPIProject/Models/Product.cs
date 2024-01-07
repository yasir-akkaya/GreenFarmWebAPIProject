namespace GreenFarmWebAPIProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Image_2 { get; set; }
        public string Image_3 { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<WishlistProduct> WishlistProducts { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
    }
}
