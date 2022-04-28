using eBay_DB.Repositories;
using System;

namespace eBay_DB.Models
{
    public class Product
    {
        public Product(string name, decimal price, Customer customer)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public Product()
        {
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Customer Customer { get; set; }

        public override string ToString()
        {
            return $"Product: ID={this.Id}, {this.Name}, price: {this.Price}, Owner: {this.Customer}";
        }

        public Product CreateProductWithUser()
        {
            Console.Write("Input name of product: ");
            var Name = Console.ReadLine();
            Console.Write("Input name of product: ");
            var Price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Input id of customer: ");
            var Customer = CustomersRepo.GetCustomerById(Convert.ToInt32(Console.ReadLine()));

            var newProduct = new Product(Name, Price, Customer);

            return newProduct;
        }
    }
}
