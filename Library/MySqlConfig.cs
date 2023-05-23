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
                    Console.WriteLine("Username and password don't match");
                while(dataReader.Read())
                {
                    ReadSingleRow((IDataRecord)dataReader);
                }
                dataReader.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (dataReader == null)
                return null;
            
            return dataReader;
        }
        private static void ReadSingleRow(IDataRecord dataRecord)
        {
            Console.WriteLine(String.Format("{0}, {1}", dataRecord[0], dataRecord[1]));
        }
        public void LogIn(string ?username, string ?password)
        {
            string sqlCommand = $@"SELECT * FROM `user_info` WHERE username='{username}' AND password='{password}'";
            MySqlDataReader ?dataReader = ExecuteQuerySql(sqlCommand);
            if (dataReader == null)
                return;
            Console.WriteLine("Login Successful");
        }
        public void Register(User user)
        {
            string sqlCommand = $@"INSERT INTO `user_info` (`names`, `username`, `password`)
                VALUES ('{user.name} {user.surname}', '{user.username}', '{user.password}');";
            ExecuteNonQuerySql(sqlCommand);
        }
        public void AddBookIndex(Book book)
        {
            string sqlCommand = $@"INSERT INTO `books` (`title`, `authors`, `publishers`, `release_date`)
                VALUES ('{book.title}', '{book.authors}', '{book.publishers}', '{book.publish_date}');";
            ExecuteNonQuerySql(sqlCommand);
        }
    }
}