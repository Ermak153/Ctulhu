using System.ComponentModel.DataAnnotations;

namespace Ctulhu.Models
{
    public class CreatePost
    {
        public List<Posts> Posts { get; set; }

        // Поля для создания поста
        [Required(ErrorMessage = "Заголовок обязателен")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать хотя бы один тег")]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public List<Tag> AvailableTags { get; set; } = new List<Tag>();
    }
}
