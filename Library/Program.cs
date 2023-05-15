using System;
using Structs;
using LoginSystem;

namespace LibraryDatabaseProgram
{
    public static class Commands
    {
        public const string Connect = "connect";
        public const string Help = "help";
        public const string Quit = "quit";
        public const string Login = "login";
        public const string Register = "register";
    }
    class Program
    {
        static void Main(string[] args)
        {
            MySqlConfig config = new MySqlConfig();
            bool closeApp = false;
            
            while(!closeApp)
            {
                Console.WriteLine("Waiting for command");
                string ?command = "";
                command = Console.ReadLine();
                if(command == null)
                    command = "";
                switch(command)
                {
                    case Commands.Quit:
                        closeApp = true;
                        break;
                    case Commands.Help:
                        Console.WriteLine("List of commands:");
                        Type type = typeof(Commands);
                        foreach (var p in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
                        {
                            var v = p.GetValue(null);
                            Console.Write($"{v?.ToString()}    ");
                        }
                        Console.WriteLine();
                        break;
                    case Commands.Connect:
                        string databaseName = "library";
                        config.Connect(databaseName);
                        break;
                    case Commands.Login:
                        LoginUser(config);
                        break;
                    case Commands.Register:
                        RegisterUser(config);
                        break;
                    default:
                        Console.WriteLine("Command not recognized. Use help for list of commands");
                        break;
                }
            }
        }
        static void RegisterUser(MySqlConfig config)
        {
            Console.WriteLine("Give name");
            string ?name = ValidateUserInput();
            Console.WriteLine("Give surname");
            string ?surname = ValidateUserInput();
            Console.WriteLine("Give username");
            string ?username = ValidateUserInput();
            string ?password = null;
            string ?checkPassword = null;
            bool passwordsMatch = false;
            do
            {
                Console.WriteLine("Give password");
                password = ValidateUserInput();
                Console.WriteLine("Repeat password");
                checkPassword = ValidateUserInput();
                if((password == checkPassword) && (password != null) && (checkPassword != null))
                    passwordsMatch = true;
                else
                    Console.WriteLine("Passwords don't match");
            }while(!passwordsMatch);
            config.Register(name, surname, username, password);
        }
        static void LoginUser(MySqlConfig config)
        {
            Console.WriteLine("Give username");
            string ?username = ValidateUserInput();
            Console.WriteLine("Give password");
            string ?password = ValidateUserInput();
            config.LogIn(username, password);
        }
        static string ?ValidateUserInput()
        {
            string ?input = null;
            bool isValid = false;
            char[] bannedCharacters = {' ', '/', '<'};// TO DO: Add full list of characters
            int maxCharacterLimit = 50;// This might be better as argument of the method
            while(!isValid)
            {
                input = Console.ReadLine();
                if(input == null)
                {
                    Console.WriteLine("No input recieved");
                    continue;
                }
                if(bannedCharacters.Any(input.Contains))
                {
                    Console.Write("Input contains banned characters: ");
                    foreach(char ch in bannedCharacters)
                        Console.Write($"'{ch}', ");
                    continue;
                }
                if(input.Length > maxCharacterLimit)
                {
                    Console.WriteLine($"Input exceeded {maxCharacterLimit} characters");
                    continue;
                }
                isValid = true;
            }
            return input;
        }
        static void AddBook(Book book, MySqlConfig config)
        {
            config.AddBookIndex(book);
        }
    }
}