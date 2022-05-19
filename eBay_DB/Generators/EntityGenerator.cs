using eBay_DB.Data;
using eBay_DB.Models;
using eBay_DB.Repositories;
using Npgsql;
using System;

namespace eBay_DB.Generators
{
    class EntityGenerator
    {
        public static void GenerateCustomer(int count)
        {
            
            while (count > 0)
            {
                var customer = new Customer();
                customer.First_Name = Faker.Name.First();
                customer.Last_Name = Faker.Name.Last();
                customer.Email = Faker.Internet.Email();
                customer.Address = Faker.Address.City() + ", " + Faker.Address.StreetAddress(false);

                CustomersRepo.AddNewCustomer(customer);

                count--;

            }
        }

        public static void AddProduct(int count)
        {
            while(count > 0)
            {                
                ProductsRepo.AddProduct(GenerateProduct());
                count--;
            }
        }

        public static void GenerateOrders(int count)
        {
            var rnd = new Random();

            for (int i = 0; i < count; i++)          
            {

                OrdersRepo.CreateOrder();
                var orderId = OrdersRepo.LastOrderId();
                var productsInOrder = rnd.Next(1, 10);

                for (int j = 0; j < productsInOrder; j++)
                {
                    var productId = rnd.Next(1, ProductsRepo.GetCountRecords());
                    OrdersRepo.AddProductInOrder(productId, rnd.Next(1, 20), orderId);
                }
            }
            Console.WriteLine($"Создано ордеров: {count}");
        }

        static Product GenerateProduct()
        {
            var rnd = new Random();
            var rndName = Faker.Lorem.Words(2);
            string result = "";
            foreach (var item in rndName)
            {
                result = result + item + " ";
            }
            var Name = result;
            var Price = (decimal)(rnd.NextDouble() * 100);
            var Customer = CustomersRepo.GetCustomerById(rnd.Next(1, 22));
            var product = new Product(Name, Price, Customer);

            return product;
        }

    }
}
