using System;
using LoginSystem;
using Structs;
using Utilities;

namespace DatabaseUtils
{
    public static class InitializeDatabase
    {
        public static void CreateUserDataTable(MySqlConfig config)
        {
            string command = @"CREATE TABLE `user_info`
            (`id` int NOT NULL AUTO_INCREMENT,
            `name` varchar(255) NOT NULL,
            `surname` varchar(255) NOT NULL,
            `username` varchar(255) NOT NULL,
            `password` varchar(255) NOT NULL,
            PRIMARY KEY (id));";
            config.ExecuteNonQuerySql(command);
            Console.WriteLine("Users table was generated.");
        }
        public static void CreateBookDataTable(MySqlConfig config)
        {
            string command = @"CREATE TABLE `books` 
            (`id` int NOT NULL AUTO_INCREMENT,
            `title` varchar(255) NOT NULL,
            `authors` varchar(255) NOT NULL,
            `publishers` varchar(255) NOT NULL,
            `release_date` DATETIME,
            `status` int NOT NULL,
            PRIMARY KEY (id));";
            config.ExecuteNonQuerySql(command);
            Console.WriteLine("Books table was generated.");
        }
        public static void CreateRentingDataTable(MySqlConfig config)
        {
            string command = @"CREATE TABLE `book_rent` 
            (`id` int NOT NULL AUTO_INCREMENT,
            `book_id` int NOT NULL,
            `user_id` int NOT NULL,
            `rent_date` DATE NOT NULL,
            `return_date` DATE,
            PRIMARY KEY (id));";
            config.ExecuteNonQuerySql(command);
            Console.WriteLine("Renting table was generated.");
        }
        
        public static void ClearTable(MySqlConfig config)
        {
            string command = @"DROP TABLE `user_info`";
            config.ExecuteNonQuerySql(command);
            command = @"DROP TABLE `books`";
            config.ExecuteNonQuerySql(command);
        }

        public static void PopulateUsers(MySqlConfig config, int number)
        {
            while(number > 0)
            {
                User user = Utils.GenerateUserData();
                config.Register(user);
                number--;
            }
            Console.WriteLine("Users were generated");
        }
        public static async Task PopulateBooks(MySqlConfig config, int number, HttpClient client)
        {
            while(number > 0)
            {
                Book book = await Utils.GenerateBookData(client);
                if(book.authors == null)
                {
                    book = await Utils.GenerateBookData(client);
                }
                Console.WriteLine(book.ToString());
                config.AddBookIndex(book);
                number--;
            }
            Console.WriteLine("Books were generated");
        }
    }
}
