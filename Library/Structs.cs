namespace Structs
{
    public struct Book
    {
        public int id {get; set;}
        public string title {get; set;}
        public string authors {get; set;}
        public DateTime publish_date {get; set;}
        public string publishers {get; set;}
        public BookStatus status {get; set;}

        public override string ToString() =>
            $"ID:{id}, Title:{title}, Author:{authors}, Publish Date:{publish_date}, Publisher:{publishers}, Status:{status}";
    }
    public enum BookStatus
    {
        Error = -1,
        Open,
        Rent,
        Unavailable
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
        public APIResponseDetails details {get; set;}
    }
    public struct APIResponseDetails
    {
        public Person[] authors {get; set;}
        public string[] publishers {get; set;}
        public string title {get; set;}
        public string publish_date {get; set;}
    }
    public struct APIResponseRandomBook
    {
        public string[] isbn_13 {get; set;}
    }
}