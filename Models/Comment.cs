using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace The_Wall.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public string Body {get;set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //fk
        public int MessageId {get;set;}
        public int UserId { get; set; }

        //np
        public Message Message { get; set; }
        public User User { get; set; }

    }
}