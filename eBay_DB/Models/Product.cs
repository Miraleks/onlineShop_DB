namespace eBay_DB.Models
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Customer Customer { get; set; }

        public override string ToString()
        {
            return $"Product: ID={this.Id}, {this.Name}, price: {this.Price}, Owner: {this.Customer}";
        }
    }
}
