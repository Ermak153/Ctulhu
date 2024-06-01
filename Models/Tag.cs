using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ctulhu.Models
{
    public class Tag
    {
        public Tag() { }
        public Tag(string name)
        {
            Name = name;
            Posts = new List<Posts>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Название тега обязательно")]
        public string Name { get; set; }

        public virtual List<Posts> Posts { get; set; }

        public override string ToString()
        {
            return $"Id:{ID} Name:{Name}";
        }
    }
}
