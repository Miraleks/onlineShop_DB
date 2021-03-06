using eBay_DB.Generators;
using System;
using eBay_DB.Repositories;
using eBay_DB.Models;

namespace eBay_DB
{
    class Program
    {
        /// <summary>
        /// Описание/Пошаговая инструкция выполнения домашнего задания:
        /// Создать базу данных PostgreSQL для одной из компаний на выбор: 
        /// Авито, СберБанк, Otus или eBay.
        /// 1. Написать скрипт создания 3 таблиц, которые должны иметь первичные ключи и быть соединены внешними ключами.
        /// 2. Написать скрипт заполнения таблиц данными, минимум по пять строк в каждую.
        /// 3. Создать консольную программу, которая выводит содержимое всех таблиц.
        /// 4. Добавить в программу возможность добавления в таблицу на выбор.
        /// 
        /// 
        /// </summary>


        static void Main(string[] args)
        {
            TablesGenerators.CreateCustomerTable();
            TablesGenerators.CreateProductsTable();
            TablesGenerators.CreateOrdersTable();
            TablesGenerators.CreateOrderProductTable();

            #region генератор пользователей

            //EntityGenerator.GenerateCustomer(20);

            #endregion

            //var customers = CustomersRepo.GetAllCustomers();

            //foreach (var elem in customers)
            //{
            //    Console.WriteLine(elem);
            //}

            //var customer1 = Repositories.CustomersRepo.GetCustomerByEmail("kassandra_berge@stokes.uk");
            //var customer2 = Repositories.CustomersRepo.GetCustomerByEmail("kassandra@stokes.uk");
            //var customer3 = Repositories.CustomersRepo.GetCustomerById(id: 20);

            //Console.WriteLine(customer1);
            //Console.WriteLine(customer2 == null ? "Пользователь не найден." : customer2);
            //Console.WriteLine(customer3 == null ? "Пользователь не найден." : customer3);

            //CustomersRepo.AddNewCustomer();

            #region генератор продуктов

            //EntityGenerator.AddProduct(20);

            #endregion


            //var products = ProductsRepo.GetAllProducts();

            //foreach (var item in products)
            //{
            //    Console.WriteLine(item);
            //}


            #region добавление продуктов в ордер

            //OrdersRepo.AddProductInOrderDialog();

            #endregion

            #region генератор ордеров

            //EntityGenerator.GenerateOrders(5);

            #endregion



            //var prod = ProductsRepo.GetProductById(12);

            //Console.WriteLine(prod);

            //var collect = OrdersProductsRepo.GetAllProductsByOrderId(7);

            //foreach (var item in collect)
            //{
            //    Console.WriteLine(item.product);
            //}




            var orders = OrdersRepo.GetOrders();
            orders.Sort((ord1, ord2) => ord1.Id.CompareTo(ord2.Id));

            foreach (var item in orders)
            {
                Console.WriteLine(item);
            }

        }
    }
}
