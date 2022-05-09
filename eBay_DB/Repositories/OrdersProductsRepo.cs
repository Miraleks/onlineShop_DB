using Dapper;
using eBay_DB.Models;
using Npgsql;
using System.Collections.Generic;

namespace eBay_DB.Repositories
{
    class OrdersProductsRepo
    {
        /*
        для связки ордеров и продуктов в них с количеством
        инициализируется при добавлении продуктов в ордер
        через цикл?
        методы:
        добавить продукт - InsertProductInOrder
        удалить продукт - DeleteProductFromOrder
        найти все продукты в ордере - GetAllProductsByOrderId


        order
        product
        value
         
         */
        public static void InsertProductInOrder(long orderId, long productId, int value)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        INSERT INTO orders_products(order_id, product_id, product_value) 
                        VALUES (:orderId, :product_id, :product_value);
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "orderId", value: orderId));
            parameters.Add(value: new NpgsqlParameter(parameterName: "product_id", value: productId));
            parameters.Add(value: new NpgsqlParameter(parameterName: "product_value", value: value));
            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();
        }

        public static bool DeleteProductFromOrder(long productId)
        {
            return false;
        }

        public static List<OrderProduct> GetAllProductsByOrderId(long orderId)
        {
            var query = @"SELECT 
                        op.id, 
                        op.order_id, 
                        op.product_id, 
                        op.product_value,
                        p.id,
                        p.name, 
                        p.price,
                        p.customer_id,
                        c.id,
                        c.first_name,
                        c.last_name,
                        c.email,
                        c.address
                        FROM orders_products op
                        JOIN products p on p.id = op.product_id
                        JOIN customers c on c.id = p.customer_id
                        WHERE op.order_id=@LookUpId";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var list = connection.Query<OrderProduct, Product, Customer, OrderProduct>(
                    sql: query,
                    param: new
                    {
                        LookUpId = orderId,
                    },
                    map: (OrderProduct orderProduct, Product product, Customer customer) => 
                    {
                        product.Customer = customer;
                        orderProduct.product = product;
                        return orderProduct;
                    });
                
                return list.AsList<OrderProduct>();
            }
        }

    }
}
