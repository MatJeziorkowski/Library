using Structs;
using LoginSystem;
using Utilities;
using DatabaseUtils;

namespace ProgramFunctions
{
    public static class MainFunctions
    {
        public static void RegisterUser(MySqlConfig config)
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
        public static void LoginUser(MySqlConfig config)
        {
            Console.WriteLine("Give username");
            string ?username = ValidateUserInput();
            Console.WriteLine("Give password");
            string ?password = ValidateUserInput();
            config.LogIn(username, password);
        }
        public static string ValidateUserInput()
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
        public static void AddBook(MySqlConfig config, Book book)
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
            string ?input = Console.ReadLine();
            if(input == null)
            {
                Console.WriteLine("No input provided");
                return;
            }
            InitializeDatabase.ClearTable(config, input);
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