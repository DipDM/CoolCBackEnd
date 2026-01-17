using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        public string CommentText {get; set;} = string.Empty;

        [Range(1,5)]
        public int? Rating {get; set;}
        public Guid UserId {get; set;}
    }
}