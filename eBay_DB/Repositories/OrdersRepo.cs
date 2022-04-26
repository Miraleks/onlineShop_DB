using Dapper;
using eBay_DB.Models;
using Npgsql;
using System.Collections.Generic;
using System.Linq;


namespace eBay_DB.Repositories
{
    class OrdersRepo
    {
        public static void InsertOrdersWithParams(Order order)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        INSERT INTO orders(created_at, orders_products_id, sum) 
                        VALUES (:created_at, :orders_products_id, :sum);
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "created_at", value: order.Date));
            parameters.Add(value: new NpgsqlParameter(parameterName: "sum", value: order.Sum));
            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();
        }
        public static List<Order> GetOrders()
        {
            var query = @"select 
                        id, created_at, sum
                        from orders";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var list = connection.Query<Order>(query);
                return list.ToList();
            }

        }

    }
}
