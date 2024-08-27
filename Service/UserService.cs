using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoolCBackEnd.Dtos.User;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.IdentityModel.Tokens;

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public UserService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    }
    public string CreateUserToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}