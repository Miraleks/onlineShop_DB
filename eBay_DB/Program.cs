using eBay_DB.Generators;
using System;
using eBay_DB.Repositories;


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
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            TablesGenerators.CreateCustomerTable();
            TablesGenerators.CreateProductsTable();
            TablesGenerators.CreateOrdersTable();
            TablesGenerators.CreateOrderProductTable();

            //CustomerGenerator.Generate(20);

            //var customers = repositories.customersrepo.getallcustomers();

            //foreach(var elem in customers)
            //{
            //    console.writeline(elem.tostring());
            //}

            var customer1 = Repositories.CustomersRepo.GetCustomerByEmail("kassandra_berge@stokes.uk");
            var customer2 = Repositories.CustomersRepo.GetCustomerByEmail("kassandra@stokes.uk");
            var customer3 = Repositories.CustomersRepo.GetCustomerById(id: 20);

            Console.WriteLine(customer1);
            Console.WriteLine(customer2 == null ? "Пользователь не найден." : customer2);
            Console.WriteLine(customer3 == null ? "Пользователь не найден." : customer3);

            CustomersRepo.AddNewCustomer();

        }
    }
}
