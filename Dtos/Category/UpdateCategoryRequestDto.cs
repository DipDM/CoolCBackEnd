using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Category
{
    public class UpdateCategoryRequestDto
    {
        public string? Name {get; set;}
        public string? Description {get; set;}
    }
}