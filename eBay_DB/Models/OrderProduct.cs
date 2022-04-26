namespace eBay_DB.Models
{
    public class OrderProduct
    {
        public long id { get; set; }
        public Order order { get; set; }
        public Product product { get; set; }
        public int value { get; set; }

    }
}
