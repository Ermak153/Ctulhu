using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ctulhu.Models
{
    public class Posts
    {
        public Posts()
        {
        }
        public Posts(string title, string description, string author, string imageUrl, string tag)
        {
            Title = title;
            Description = description;
            Author = author;
            IsApproved = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            ImageUrl = imageUrl;
            Tag = tag;
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        [MaxLength(20, ErrorMessage = "Заголовок не может быть длиннее 20 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }

        public string Author { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Теги обязательны")]
        public string Tag { get; set; }

        public override string ToString()
        {
            return $"Id:{ID} Title:{Title} Description: {Description} Author: {Author} CreatedAt: {CreatedAt} UpdatedAt: {UpdatedAt} ImageUrl: {ImageUrl} Tag: {Tag}";
        }
    }
}
