using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ctulhu.Models
{
    public class Posts
    {
        public Posts() { }
        public Posts(string title, string description, string author, string imageUrl, List<Tag> tags)
        {
            Title = title;
            Description = description;
            Author = author;
            IsApproved = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            ImageUrl = imageUrl;
            Tags = tags ?? new List<Tag>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }

        public string Author { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ImageUrl { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public override string ToString()
        {
            return $"Id:{ID} Title:{Title} Description: {Description} Author: {Author} CreatedAt: {CreatedAt} UpdatedAt: {UpdatedAt} ImageUrl: {ImageUrl}";
        }
    }
}
