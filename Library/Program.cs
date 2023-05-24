using LoginSystem;
using ProgramCommands;
using ProgramFunctions;

namespace LibraryDatabaseProgram
{
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
                        MainFunctions.LoginUser(config);
                        break;
                    case Commands.Register:
                        MainFunctions.RegisterUser(config);
                        break;
                    case Commands.CreateUserInfo:
                        MainFunctions.CreateUserInfoTables(config);
                        break;
                    case Commands.CreateBookInfo:
                        MainFunctions.CreateBooksTables(config);
                        break;
                    case Commands.CreateBookRentInfo:
                        MainFunctions.CreateBookRentInfo(config);
                        break;
                    case Commands.ClearAll:
                        MainFunctions.ClearTables(config);
                        break;
                    case Commands.PopulateUsers:
                        MainFunctions.AddRandomUsers(config);
                        break;
                    case Commands.PopulateBooks:
                        MainFunctions.AddRandomBooks(client, config);
                        break;
                    case Commands.RentBook:
                        MainFunctions.RentBook(config);
                        break;
                    case Commands.Test:
                        MainFunctions.ReturnBook(config);
                        break;
                    default:
                        Console.WriteLine($"Command not recognized. Use {Commands.Help} for list of commands");
                        break;
                }
            }
            config.Disconnect();
        }
    }
}