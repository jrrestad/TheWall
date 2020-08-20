using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Wall.Models
{    
    [NotMapped]
    public class LogUser
    {
        public int UserId {get;set;}

        [Required(ErrorMessage="An email address is required")]
        [EmailAddress]
        public string LogEmail {get;set;}
        [Required(ErrorMessage="A password is required")]
        [DataType(DataType.Password)]
        public string LogPassword {get;set;}
    }
}