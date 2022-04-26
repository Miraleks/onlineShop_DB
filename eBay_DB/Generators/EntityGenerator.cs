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

        public static void GenerateProduct(int count)
        {
            while(count > 0)
            {
                var product = new Product();
                var rnd = new Random();
                product.Name = Faker.Lorem.Words(2).ToString();
                product.Price = (decimal)(rnd.NextDouble()*100);
                product.Customer = CustomersRepo.GetCustomerById(rnd.Next(1, 22));

                ProductsRepo.AddProduct(product);

                count--;
            }
        }

        public static void GenerateOrders(int count)
        {
            var rnd = new Random();
            var productsInOrder = rnd.Next(1, 10);
            var productsId = 0;
            while (count > 0)
            {
                var order = new Order();
                var product = new Product();

                while(productsInOrder > 0)
                {
                    //id ордера - последнее с автоматическим созданием записи и возвратом id созданной записи
                    //случайная выборка из products по id
                    //случайное назначение количества выбранных продуктов
                    //подсчет суммы продуктов и внесение в поле ордера
                }


            }
        }

    }
}
