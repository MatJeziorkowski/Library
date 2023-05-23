using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Structs;

namespace WebRequests
{
    public static class LibraryAPIRequests
    {
        public static async Task<Book> GetBookFromAPI(HttpClient client, string key){
            Dictionary<string, APIResponse> ?output;
            Book book = new Book();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://openlibrary.org/api/books?bibkeys={key}&jscmd=details&format=json");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<Dictionary<string, APIResponse>>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return book;
            }
            if(output == null)
            {
                return book;
            }
            if(!output.ContainsKey(key))
                return book;
            APIResponse value;
            output.TryGetValue(key, out value);
            foreach(Person author in value.details.authors)
            {
                book.authors += author.name;
            }
            foreach(string publisher in value.details.publishers)
            {
                book.publishers = publisher;
            }
            book.title = value.details.title;
            book.publish_date = value.details.publish_date;
            return book;
        }
        public static async Task<string> GetRandomBookFromAPI(HttpClient client)
        {
            string isbn = "";
            string url;
            APIResponseRandomBook output = new APIResponseRandomBook();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://openlibrary.org/random");
                response.EnsureSuccessStatusCode();
                url = await GetRedirectAddress(client);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<APIResponseRandomBook>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
            if(output.isbn_13 != null)
            {
              isbn = $"ISBN:{output.isbn_13[0]}";
            }
            return isbn;
        }
        public static async Task<string> GetRedirectAddress(HttpClient client)
        {
            using HttpResponseMessage response = await client.GetAsync($"https://openlibrary.org/random");
            response.EnsureSuccessStatusCode();
            string uri = response.RequestMessage.RequestUri.ToString();
            if(uri == null)
            {
                Console.WriteLine("No Url found");
                return "";
            }
            char character = '/';
            int i = uri.Length - 1;
            while(uri[i] != character)
            {
                i--;
            }
            uri = uri.Substring(0, i);
            uri += ".json";
            return uri;
        }
    }
}