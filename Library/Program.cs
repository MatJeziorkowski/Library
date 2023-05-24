using System;
using Structs;
using LoginSystem;
using DatabaseUtils;
using WebRequests;
using Utilities;

namespace LibraryDatabaseProgram
{
    public static class Commands
    {
        public const string Help = "help";
        public const string Quit = "quit";
        public const string Login = "login";
        public const string Register = "register";
        public const string CreateUserInfo = "createUserInfo";
        public const string CreateBookInfo = "createBookInfo";
        public const string CreateBookRentInfo = "createBookRentInfo";
        public const string ClearAll = "clear"; 
        public const string AddBook = "addBook";
        public const string RentBook = "rentBook";
        public const string ReturnBook = "returnBook";
        public const string PopulateUsers = "populateUser";
        public const string PopulateBooks = "populateBook";
        public const string Test = "test";
    }
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.Clear();
            MySqlConfig config = new MySqlConfig();
            bool closeApp = false;
            string databaseName = "library";
            config.Connect(databaseName);

            while (!closeApp)
            {
                Console.WriteLine("Waiting for command");
                string? command = "";
                command = Console.ReadLine();
                if (command == null)
                    command = "";
                switch (command)
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
                    case Commands.Login:
                        LoginUser(config);
                        break;
                    case Commands.Register:
                        RegisterUser(config);
                        break;
                    case Commands.CreateUserInfo:
                        CreateUserInfoTables(config);
                        break;
                    case Commands.CreateBookInfo:
                        CreateBooksTables(config);
                        break;
                    case Commands.CreateBookRentInfo:
                        CreateBookRentInfo(config);
                        break;
                    case Commands.ClearAll:
                        ClearTables(config);
                        break;
                    case Commands.PopulateUsers:
                        AddRandomUsers(config);
                        break;
                    case Commands.PopulateBooks:
                        AddRandomBooks(client, config);
                        break;
                    case Commands.RentBook:
                        RentBook(config);
                        break;
                    case Commands.Test:
                        ReturnBook(config);
                        break;
                    default:
                        Console.WriteLine($"Command not recognized. Use {Commands.Help} for list of commands");
                        break;
                }
            }
            config.Disconnect();
        }
        static void RegisterUser(MySqlConfig config)
        {
            User user = new User();
            Console.WriteLine("Give name");
            user.name = ValidateUserInput();
            Console.WriteLine("Give surname");
            user.surname = ValidateUserInput();
            Console.WriteLine("Give username");
            user.username = ValidateUserInput();
            user.password = "";
            string ?checkPassword = null;
            bool passwordsMatch = false;
            do
            {
                Console.WriteLine("Give password");
                user.password = ValidateUserInput();
                Console.WriteLine("Repeat password");
                checkPassword = ValidateUserInput();
                if((user.password == checkPassword) && (user.password != null) && (checkPassword != null))
                    passwordsMatch = true;
                else
                    Console.WriteLine("Passwords don't match");
            }while(!passwordsMatch);
            config.Register(user);
        }
        static void LoginUser(MySqlConfig config)
        {
            Console.WriteLine("Give username");
            string ?username = ValidateUserInput();
            Console.WriteLine("Give password");
            string ?password = ValidateUserInput();
            config.LogIn(username, password);
        }
        static string ValidateUserInput()
        {
            string input = "";
            bool isValid = false;
            char[] bannedCharacters = {' ', '/', '<', '>'};// TO DO: Add full list of characters
            int maxCharacterLimit = 50;// This might be better as argument of the method
            while(!isValid)
            {
                string ?consoleInput = Console.ReadLine();
                if(consoleInput != null)
                    input = consoleInput;
                else
                {
                    Console.WriteLine("No input received");
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
        static void AddBook(MySqlConfig config, Book book)
        {
            config.AddBookIndex(book);
        }
        public static void CreateUserInfoTables(MySqlConfig config)
        {
            InitializeDatabase.CreateUserDataTable(config);
        }
        public static void CreateBooksTables(MySqlConfig config)
        {
            InitializeDatabase.CreateBookDataTable(config);
        }
        public static void ClearTables(MySqlConfig config)
        {
            InitializeDatabase.ClearTable(config);
        }
        public static void AddRandomUsers(MySqlConfig config)
        {
            Console.WriteLine("Give number of users to generate");
            int input = Convert.ToInt32(Console.ReadLine());
            if(input > 0)
                InitializeDatabase.PopulateUsers(config, input); 
        }
        public static void AddRandomBooks(HttpClient client, MySqlConfig config)
        {
            Console.WriteLine("Give number of books to generate");
            int input = Convert.ToInt32(Console.ReadLine());
            if(input > 0)
            {
                Task task = InitializeDatabase.PopulateBooks(config, input, client);
                task.Wait();
            }
        }
        public static void GetRandomISBN13(HttpClient client)
        {
            Task task = Utils.GetISBN13FromAPI(client);
            task.Wait();
        }
        public static void CreateBookRentInfo(MySqlConfig config)
        {
            InitializeDatabase.CreateRentingDataTable(config);
        }
        public static void RentBook(MySqlConfig config)
        {
            Console.WriteLine("Give book id");
            int bookIndex = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Give user id");
            int userIndex = Convert.ToInt32(Console.ReadLine());
            DateTime now = DateTime.Now;
            config.RentBook(userIndex, bookIndex, now);
        }
        public static void ReturnBook(MySqlConfig config)
        {
            Console.WriteLine("Give book id");
            int bookIndex = Convert.ToInt32(Console.ReadLine());
            DateTime now = DateTime.Now;
            config.ReturnBook(bookIndex, now);
        }
    }
}