using Dapper;
using eBay_DB.Models;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System;

namespace eBay_DB.Repositories
{
    class CustomersRepo
    {
        /// <summary>
        /// first_name, 
        /// last_name, 
        /// email, 
        /// address
        /// </summary>
        public static void AddNewCustomer()
        {               
            Customer customerNew = new Customer();
            Console.Write("Input name: ");
            customerNew.First_Name = Console.ReadLine();
            Console.Write("Input last name: ");
            customerNew.Last_Name = Console.ReadLine();
            Console.Write("Input email: ");
            customerNew.Email = Console.ReadLine();
            Console.Write("Input address: ");
            customerNew.Address = Console.ReadLine();

            InsertCustomersWithParams(customerNew);

        }

        public static void AddNewCustomer(Customer customer)
        {
            InsertCustomersWithParams(customer);

        }

        private static void InsertCustomersWithParams(Customer customer)
        {
            

            

            using NpgsqlConnection connection = new NpgsqlConnection(connectionString: Data.Config.SqlConnectionString);
            connection.Open();

            string sql = @"
                        INSERT INTO customers(first_name, last_name, email, address) 
                        VALUES (:first_name, :last_name, :email, :address);
                        ";

            using NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: connection);
            NpgsqlParameterCollection parameters = cmd.Parameters;
            parameters.Add(value: new NpgsqlParameter(parameterName: "first_name", value: customer.First_Name));
            parameters.Add(value: new NpgsqlParameter(parameterName: "last_name", value: customer.Last_Name));
            parameters.Add(value: new NpgsqlParameter(parameterName: "email", value: customer.Email));
            parameters.Add(value: new NpgsqlParameter(parameterName: "address", value: customer.Address));

            string affectedRowsCount = cmd.ExecuteNonQuery().ToString();
        }

        public static List<Customer> GetAllCustomers()
        {
            var query = @"SELECT 
                        id, first_name, last_name, email, address
                        FROM customers";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var list = connection.Query<Customer>(query);
                return list.ToList();
            }
        }

        public static Customer GetCustomerByEmail(string email)
        {
            var query = @"SELECT 
                        id, first_name, last_name, email, address
                        FROM customers
                        WHERE customers.email=@LookUpEmail";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var customer = connection.QueryFirstOrDefault<Customer>(
                    sql: query,
                    param: new
                    {
                        LookUpEmail = email,
                    });
                return customer;
            }
        }

        public static Customer GetCustomerById(int id)
        {
            var query = @"SELECT 
                        id, first_name, last_name, email, address
                        FROM customers
                        WHERE customers.id=@LookUpId";

            using (var connection = new NpgsqlConnection(Data.Config.SqlConnectionString))
            {
                var customer = connection.QueryFirstOrDefault<Customer>(
                    sql: query,
                    param: new
                    {
                        LookUpId = id,
                    });
                return customer;
            }
        }
    }
}