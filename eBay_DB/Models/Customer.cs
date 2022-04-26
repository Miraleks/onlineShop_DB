namespace eBay_DB.Models
{
    public class Customer
    {
        public long Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return $"Customer: ID={this.Id}, {this.First_Name} {this.Last_Name}, e-mail: {this.Email}";
        }




    }
}