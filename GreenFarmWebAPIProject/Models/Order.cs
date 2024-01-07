namespace GreenFarmWebAPIProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
