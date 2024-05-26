namespace Ctulhu.Models
{
    public class Posts
    {
        public Posts() { }
        public Posts(string title, string description, string author)
        {
            Title = title;
            Description = description;
            Author = author;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public override string ToString()
        {
            return $"Id:{ID} Title:{Title} Description: {Description} Author: {Author}";
        }
    }
}
