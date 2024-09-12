using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Comment
{
    public class CommentDto
    {
        public int CommentId {get; set;}
        public string CommentText {get; set;} = string.Empty;
        public int? Rating {get; set;}
        public int ProductId {get; set;}
        public Guid UserId {get; set;}
    }
}