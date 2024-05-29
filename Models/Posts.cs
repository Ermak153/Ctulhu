using System.ComponentModel.DataAnnotations;

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
            IsApproved = false;
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Заголовок обязателен")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }
        public string Author { get; set; }
        public bool IsApproved { get; set; }

        public override string ToString()
        {
            return $"Id:{ID} Title:{Title} Description: {Description} Author: {Author}";
        }
    }
}
