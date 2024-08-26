using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Comment
    {
        [Key]
        public int CommentId {get; set;}
        public string CommentText {get; set;} = string.Empty;
        public int? Rating {get; set;}
        public int? ProductId {get; set;}
        public Product? Product {get; set;}
    }
}