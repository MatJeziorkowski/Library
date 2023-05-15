using System;
using System.Data;
using Structs;
using MySql.Data.MySqlClient;

namespace LoginSystem
{
    public class MySqlConfig
    {
        string conectionString = "";
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
                conectionString = "SERVER=" + server + ";" + "DATABASE=" + databaseName + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(conectionString);
                connection.Open();
                Console.WriteLine($"Connection successful. Server version: {connection.ServerVersion}");
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ExecuteNonQuerySql(string sqlCommand)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(conectionString);
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, connection);
                connection.Open();
                mySqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public MySqlDataReader ?ExecuteQuerySql(string sqlCommand)
        {
            MySqlDataReader ?dataReader = null;
            try
            {
                MySqlConnection connection = new MySqlConnection(conectionString);
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, connection);
                connection.Open();
                dataReader = mySqlCommand.ExecuteReader();
                if(!dataReader.HasRows)
                    Console.WriteLine("Username and password don't match");
                else
                    Console.WriteLine("Login Successful");
                while(dataReader.Read())
                {
                    ReadSingleRow((IDataRecord)dataReader);
                }
                dataReader.Close();
                connection.Close();
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
        }
        public void Register(string ?name, string ?surname, string ?username, string ?password)
        {
            string sqlCommand = $@"INSERT INTO `user_info` (`id`, `names`, `username`, `password`)
                VALUES (NULL, '{name} {surname}', '{username}', '{password}');";
            ExecuteNonQuerySql(sqlCommand);
        }
        public void AddBookIndex(Book book)
        {
            string sqlCommand = $@"INSERT INTO `books` (`id`, `title`, `author`, `description`, `edition`, `publisher`, `release_year`, `description`)
                VALUES (NULL, '{book.title}', '{book.author}', '{book.description}', '{book.edition}', '{book.publisher}', '{book.releaseYear}', '{book.description}');";
            ExecuteNonQuerySql(sqlCommand);
        }
    }
}