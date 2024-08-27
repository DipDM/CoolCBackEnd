using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.User;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IUserService
    {
        string CreateUserToken(User user);
    }
}