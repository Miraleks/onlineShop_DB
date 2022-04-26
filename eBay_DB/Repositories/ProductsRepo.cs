using Dapper;
using eBay_DB.Models;
using Npgsql;
using System;

namespace eBay_DB.Repositories
{
    /// <summary>
    /// name, 
    /// price, 
    /// customer_id
    /// </summary>
    class ProductsRepo
    {

        public static void AddProduct()
        {
            Product productNew = new Product();


            Console.Write("Input name of product: ");
            productNew.Name = Console.ReadLine();
            Console.Write("Input name of product: ");
            productNew.Price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Input id of customer: ");
            productNew.Customer = CustomersRepo.GetCustomerById(Convert.ToInt32(Console.ReadLine()));

            InsertProductsWithParams(productNew);
        }

        public static void AddProduct(Product product)
        {
            InsertProductsWithParams(product);

        }

        private static void InsertProductsWithParams(Product product)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        INSERT INTO products(name, price, customer_id) 
                        VALUES (:name, :price, :customer_id);
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "name", value: product.Name));
            parameters.Add(value: new NpgsqlParameter(parameterName: "price", value: product.Price));
            parameters.Add(value: new NpgsqlParameter(parameterName: "customer_id", value: product.Customer.Id));
            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();
        }

        public static Product GetProductById(int id)
        {
            var query = @"SELECT 
                        id, first_name, last_name, email, address
                        FROM products
                        WHERE products.id=@LookUpId";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var product = connection.QueryFirstOrDefault<Product>(
                    sql: query,
                    param: new
                    {
                        LookUpId = id,
                    });
                return product;
            }
        }

    }
}
