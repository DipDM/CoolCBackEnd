using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;
using Microsoft.Identity.Client;

namespace CoolCBackEnd.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(User user, IList<string> roles);
    }
}