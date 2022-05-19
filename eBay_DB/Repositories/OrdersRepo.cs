using Dapper;
using eBay_DB.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;


namespace eBay_DB.Repositories
{
    class OrdersRepo
    {
        public static void CreateOrder()
        {
            var createDate = DateTime.Today;
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        INSERT INTO orders(created_at, sum) 
                        VALUES (:created_at, :sum)";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "created_at", value: createDate));
            parameters.Add(value: new NpgsqlParameter(parameterName: "sum", value: 0));

            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();
        }
        public static List<Order> GetOrders()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var query = @"SELECT 
                        *
                        from orders";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var list = connection.Query<Order>(query);
                    
                return list.ToList();
            }

        }

        public static Order GetOrderById(long id)
        {
            var query = @"SELECT 
                        *
                        FROM orders
                        WHERE orders.id=@LookUpId";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var order = connection.QueryFirstOrDefault<Order>(
                    sql: query,
                    param: new
                    {
                        LookUpId = id,
                    });


                return order;
            }
        }

        internal static void AddProductInOrderDialog()
        {
            OrdersRepo.CreateOrder();
            var orderId = LastOrderId();
            int countOfProductsInOrder = 0;


            while (true)
            {
                Console.WriteLine("Введите код продукта или \"выход\" для выхода.");
                var input = Console.ReadLine();

                if (input == "выход")
                {
                    Console.WriteLine(countOfProductsInOrder > 0 ? $"Добавлено в ордер {countOfProductsInOrder} продуктов" : "Продукты не добавлены в ордер");
                    break;
                }
                else
                {
                    countOfProductsInOrder++; 
                    var productId = Convert.ToInt64(input);
                    Console.WriteLine("Введите количество.");
                    var productValue = Convert.ToInt32(Console.ReadLine());

                    AddProductInOrder(productId, productValue, orderId);
                }
            }

            if (countOfProductsInOrder == 0)
            {
                DeleteOrder(orderId);
            }
        }

        internal static void AddProductInOrder(long productID, int value, long orderId)
        {
            var sum = GetOrderById(orderId).Sum;
            OrdersProductsRepo.InsertProductInOrder(orderId, productID, value);
            sum += ProductsRepo.GetProductById(productID).Price * value;
            UpdateOrders(orderId, sum);               
        }

        private static void DeleteOrder(long orderId)
        {
            var query = @"DELETE FROM orders
                        WHERE orders.id=@LookUpId";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var order = connection.QueryFirstOrDefault<Order>(
                    sql: query,
                    param: new
                    {
                        LookUpId = orderId,
                    });
                
            }
        }

        private static void UpdateOrders(long orderId, decimal sum)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        UPDATE orders SET 
                        sum = @sum
                        WHERE orders.id=@LookUpId
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "sum", value: sum));
            parameters.Add(value: new NpgsqlParameter(parameterName: "LookUpId", value: orderId));
            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();
        }

        internal static long LastOrderId()
        {
            var query = @"SELECT * 
                          FROM orders 
                          ORDER BY id DESC 
                          LIMIT 1;";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var order = connection.QueryFirstOrDefault<Order>(
                    sql: query);
                return order != null ? order.Id : 0;
            }
        }
    }
}
