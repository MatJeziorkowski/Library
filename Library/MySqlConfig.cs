using System;
using System.Data;
using Structs;
using MySql.Data.MySqlClient;

namespace LoginSystem
{
    public class MySqlConfig
    {
        string connectionString = "";
        public MySqlConnection ?connection = null;
        public string server = "localhost";
        public string user = "root";
        public string password = "";

        public MySqlConfig()
        {

        }
        public void Connect(string databaseName)
        {
            try
            {
                connectionString = "SERVER=" + server + ";" + "DATABASE=" + databaseName + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine($"Connection successful. Server version: {connection.ServerVersion}");
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Disconnect()
        {
            connection?.Close();
        }
        public void ExecuteNonQuerySql(string sqlCommand)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, connection);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public MySqlDataReader ?ExecuteQuerySql(string sqlCommand)
        {
            MySqlDataReader ?dataReader = null;
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, connection);
                dataReader = mySqlCommand.ExecuteReader();
                if(!dataReader.HasRows)
                    return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dataReader;
        }
        public void LogIn(string ?username, string ?password)
        {
            string sqlCommand = $@"SELECT * FROM `user_info` WHERE username='{username}' AND password='{password}'";
            MySqlDataReader ?dataReader = ExecuteQuerySql(sqlCommand);
            if (dataReader == null)
                Console.WriteLine("Username and password don't match");
            else
                Console.WriteLine("Login Successful");
        }
        public void Register(User user)
        {
            string sqlCommand = $@"INSERT INTO `user_info` (`name`, `surname`, `username`, `password`)
                VALUES ('{user.name}', '{user.surname}', '{user.username}', '{user.password}');";
            ExecuteNonQuerySql(sqlCommand);
        }
        public void AddBookIndex(Book book)
        {
            string sqlCommand = $@"INSERT INTO `books` (`title`, `authors`, `publishers`, `release_date`, `status`)
                VALUES ('{book.title}', '{book.authors}', '{book.publishers}', '{book.publish_date}', '0');";
            ExecuteNonQuerySql(sqlCommand);
            string sqlQuery = $"SELECT id FROM `books` WHERE title='{book.title}'";
        }
        public void RentBook(int userID, int bookID, DateTime date)
        {
            Book book = GetBookFromID(bookID);
            User user = GetUserFromID(userID);
            if((book.id == 0) || (user.id == 0))
            {
                Console.WriteLine("User or book doesn't exit in database");
                return;
            }
            if(book.status != BookStatus.Open)
            {
                Console.WriteLine("Book is unavailable");
                return;
            }
            string sqlCommand = $@"INSERT INTO `book_rent` (`book_id`, `user_id`, `rent_date`)
            VALUES ('{bookID}', '{userID}', '{date}')";
            ExecuteNonQuerySql(sqlCommand);
            UpdateBookStatus(bookID, (int)BookStatus.Rent);
        }
        public void ReturnBook(int bookID, DateTime date)
        {
            Book book = GetBookFromID(bookID);
            if(book.id == 0)
            {
                Console.WriteLine("Book doesn't exit in database");
                return;
            }
            if(book.status != BookStatus.Rent)
            {
                Console.WriteLine("Book is not currently rent");
                return;
            }
            UpdateBookStatus(bookID, (int)BookStatus.Open);
        }
        public void UpdateBookStatus(int bookID, int status)
        {
            string sqlCommand = $"UPDATE `books` SET status='{status}' WHERE id={bookID}";
            ExecuteNonQuerySql(sqlCommand);
        }
        public Book GetBookFromID(int bookID)
        {
            string sqlBookQuery = $@"SELECT * FROM `books` WHERE id='{bookID}'";
            MySqlDataReader ?dataReader = ExecuteQuerySql(sqlBookQuery);
            if(dataReader == null)
            {
                Console.WriteLine("No book with given ID was found");
                return new Book();
            }
            Book book = new Book();
            while(dataReader.Read())
            {
                book.id = dataReader.GetInt32(0);
                book.title = dataReader.GetString(1);
                book.authors = dataReader.GetString(2);
                book.publishers = dataReader.GetString(3);
                // book.publish_date = dataReader.GetDateTime(4);
                int statusInt = dataReader.GetInt32(5);
                book.status = (BookStatus)statusInt;
            }
            dataReader.Close();
            Console.WriteLine("Query Book: " + book);
            return book;
        }
        public User GetUserFromID(int userID)
        {
            string sqlBookQuery = $@"SELECT * FROM `user_info` WHERE id='{userID}'";
            MySqlDataReader ?dataReader = ExecuteQuerySql(sqlBookQuery);
            if(dataReader == null)
            {
                Console.WriteLine("No user with given ID was found");
                return new User();
            }
            User user = new User();
            while(dataReader.Read())
            {
                user.id = dataReader.GetInt32(0);
                user.name = dataReader.GetString(1);
                user.surname = dataReader.GetString(2);
                user.username = dataReader.GetString(3);
            }
            dataReader.Close();
            return user;
        }
    }
}