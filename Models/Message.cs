using System;
using System.Collections.Generic;

namespace The_Wall.Models
{
    public class Message
    {
        public int MessageId {get;set;}
        public string Body {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        //fk
        public int UserId { get; set; }
        // np
        public User Poster {get;set;}
        public List<Comment> MessageComments {get;set;}
    }
}