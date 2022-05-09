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
        public static void InsertOrdersWithParams(DateTime dateTime, decimal sum)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        INSERT INTO orders(created_at, sum) 
                        VALUES (:created_at, :sum)";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "created_at", value: dateTime));
            parameters.Add(value: new NpgsqlParameter(parameterName: "sum", value: sum));

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
            var createDate = DateTime.Today;
            decimal sum = 0;
            OrdersRepo.InsertOrdersWithParams(createDate, sum);
            var orderId = LastOrderId();
            bool create = false;
            while (true)
            {
                Console.WriteLine("Введите код продукта или \"выход\" для выхода.");
                var input = Console.ReadLine();

                if (input == "выход")
                {
                    break;
                }
                else
                {
                    var productId = Convert.ToInt64(input);
                    Console.WriteLine("Введите количество.");
                    var productValue = Convert.ToInt32(Console.ReadLine());

                    OrdersProductsRepo.InsertProductInOrder(orderId, productId, productValue);
                    sum += ProductsRepo.GetProductById(productId).Price * productValue;
                    UpdateOrders(orderId, sum);
                    create = true;
                }
            }


            if (create)
            {
                Console.WriteLine("Order added.");
            }
            else
            {
                DeleteOrder(orderId);
                Console.WriteLine("Продукты не добавлены в ордер.");
            }
 
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

        private static long LastOrderId()
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
