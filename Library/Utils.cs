using Structs;
using RandomNameGeneratorLibrary;
using PasswordGenerator;
using WebRequests;

namespace Utilities
{
    public static class Utils
    {
        public static User GenerateUserData()
        {
            PersonNameGenerator personGenerator = new PersonNameGenerator();
            User user = new User();
            user.name = personGenerator.GenerateRandomFirstName();
            user.surname = personGenerator.GenerateRandomLastName();
            user.username = user.name.ToLower() + user.surname.ToLower();
            var pass = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: false, includeSpecial: false, passwordLength: 16);
            user.password = pass.Next();
            return user;
        }
        public static async Task<Book> GenerateBookData(HttpClient client)
        {
            Book book = new Book();
            string key = await Utils.GetISBN13FromAPI(client);
            book = await LibraryAPIRequests.GetBookFromAPI(client, key);
            if(book.authors == "")
            {
                return await LibraryAPIRequests.GetBookFromAPI(client, key);
            }
            Console.WriteLine($"Generated book: {book.ToString()}");
            return book;
        }
        public static async Task<string> GetISBN13FromAPI(HttpClient client)
        {
            string? isbn13 = null;

            isbn13 = await LibraryAPIRequests.GetRandomBookFromAPI(client);
            if (isbn13 == "")
            {
                return await GetISBN13FromAPI(client);
            }
            Console.WriteLine($"Generated ISBN: {isbn13}");
            return isbn13;
        }
    }
}