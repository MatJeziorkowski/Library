namespace Structs
{
    public struct Book
    {
        // API uses both strings and DateTimes in this property for some reason
        public int id {get; set;}
        public string title {get; set;}
        public string authors {get; set;}
        public DateTime publish_date {get; set;}
        public string publishers {get; set;}

        public override string ToString() =>
            $"Title:{title}, Author:{authors}, Publish Date:{publish_date}, Publisher:{publishers}";
    }
    public struct User
    {
        public int id {get; set;}
        public string name {get; set;}
        public string surname {get; set;}
        public string username {get; set;}
        public string password {get; set;}
    }
    public struct Person
    {
        public string name {get; set;}
        public string url {get; set;}
    }
    public struct APIResponse
    {
        public APIResponseDetails details;
    }
    public struct APIResponseDetails
    {
        public Person[] authors;
        public string[] publishers;
        public string title;
        public DateTime publish_date; 
    }
    public struct APIResponseRandomBook
    {
        public string[] isbn_13;
    }
}