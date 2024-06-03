using Microsoft.Extensions.Hosting;

namespace Ctulhu.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
