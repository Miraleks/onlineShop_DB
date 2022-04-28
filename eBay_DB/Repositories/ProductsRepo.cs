using Dapper;
using eBay_DB.Models;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

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
            productNew.CreateProductWithUser();
           
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
        public static List<Product> GetAllProducts()
        {
            var query = @"SELECT 
                        p.id,
                        p.name, 
                        p.price,
                        p.customer_id,
                        c.id,
                        c.first_name,
                        c.last_name,
                        c.email,
                        c.address
                        FROM products p
                        JOIN customers c on c.id = p.customer_id
                        ORDER BY p.name";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var list = connection.Query<Product, Customer, Product>(
                    sql: query,
                    map: (Product product, Customer customer) =>
                    {
                        product.Customer = customer;
                        return product;
                    },
                    splitOn: "customer_id"
                    );
                return list.ToList();
            }
        }
        
        public static Product GetProductById(int id)
        {
            var query = @"SELECT 
                        p.id,
                        p.name, 
                        p.price,
                        p.customer_id
                        FROM products p
                        JOIN customers c on c.id = p.customer_id
                        ORDER BY p.name
                        WHERE products.id = @LookUpId";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var list1 = connection.QueryFirstOrDefault(query);
                var list = connection.Query<Product, Customer, Product>(
                    sql: query,
                    param: new
                    {
                        LookUpId = id,
                    },
                    map: (Product product, Customer customer) =>
                    {
                        product.Customer = customer;
                        return product;
                    }
                    );
                return list.ToList().FirstOrDefault<Product>();
            }
           
        }

    }
}
