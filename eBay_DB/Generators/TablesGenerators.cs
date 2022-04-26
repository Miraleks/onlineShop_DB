using Npgsql;
using System;


namespace eBay_DB.Generators
{
    public class TablesGenerators
    {

        public static void CreateCustomerTable()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        CREATE SEQUENCE IF NOT EXISTS customers_id_seq;

                        CREATE TABLE IF NOT EXISTS customers
                        (
                            id              BIGINT                      NOT NULL    DEFAULT NEXTVAL('customers_id_seq'),
                            first_name      CHARACTER VARYING(255)      NOT NULL,
                            last_name       CHARACTER VARYING(255)      NOT NULL,
                            email           CHARACTER VARYING(255),
                            address         CHARACTER VARYING(255)      NOT NULL,
  
                            CONSTRAINT customers_pkey PRIMARY KEY (id),
                            CONSTRAINT customers_email_unique UNIQUE (email)
                        );

                        CREATE INDEX IF NOT EXISTS customers_last_name_idx ON customers(last_name);
                        CREATE UNIQUE INDEX IF NOT EXISTS customers_email_unq_idx ON customers(lower(email));
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);

            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();

            Console.WriteLine(value: $"Created CUSTOMERS table.");
        }

        public static void CreateProductsTable()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        CREATE SEQUENCE IF NOT EXISTS products_id_seq;

                        CREATE TABLE IF NOT EXISTS products
                        (
                            id              BIGINT                      NOT NULL    DEFAULT NEXTVAL('products_id_seq'),
                            name            CHARACTER VARYING(255)      NOT NULL,
                            price           NUMERIC(7,2)                NOT NULL,
                            customer_id     BIGINT                      NOT NULL,    
  
                            CONSTRAINT products_pkey PRIMARY KEY (id),
                            CONSTRAINT products_fk_customer_id FOREIGN KEY (customer_id) REFERENCES customers(id) ON DELETE CASCADE
                        );
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);

            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();

            Console.WriteLine(value: $"Created PRODUCTS table.");
        }

        public static void CreateOrdersTable()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        CREATE SEQUENCE IF NOT EXISTS orders_id_seq;

                        CREATE TABLE IF NOT EXISTS orders
                        (
                            id                    BIGINT                      NOT NULL    DEFAULT NEXTVAL('orders_id_seq'),
                            created_at            TIMESTAMP WITH TIME ZONE    NOT NULL,
                            sum                   NUMERIC(9,2)                NOT NULL,
  
                            CONSTRAINT orders_pkey PRIMARY KEY (id)
                        );
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);

            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();

            Console.WriteLine(value: $"Created ORDERS table.");
        }

        public static void CreateOrderProductTable()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        CREATE SEQUENCE IF NOT EXISTS orders_products_id_seq;

                        CREATE TABLE IF NOT EXISTS orders_products
                        (
                            id              BIGINT                      NOT NULL    DEFAULT NEXTVAL('orders_products_id_seq'),
                            order_id        BIGINT                      NOT NULL,
                            product_id      BIGINT                      NOT NULL,
                            product_value   INT                         NOT NULL,
  
                            CONSTRAINT orders_products_pkey PRIMARY KEY (id),
                            CONSTRAINT orders_products_fk_order_id FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
                            CONSTRAINT orders_products_fk_product_id FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
                            
                        );
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);

            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();

            Console.WriteLine(value: $"Created ORDERS_PRODUCTS table.");

        }

    }
}
