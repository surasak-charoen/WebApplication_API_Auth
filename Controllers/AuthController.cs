using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // In-memory collection to store registered users
    private static List<RegisterModel> _users = new List<RegisterModel>();
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        // Retrieve the JWT secret key from configuration (appsettings.json)
        _configuration = configuration;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterModel registerModel)
    {
        var hashedPassword = HashPassword(registerModel.Password);

        _users.Add(new RegisterModel
        {
            Username = registerModel.Username,
            Password = hashedPassword
        });

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel loginModel)
    {
        var user = _users.FirstOrDefault(u => u.Username == loginModel.Username);

        if (user == null || !VerifyPassword(loginModel.Password, user.Password))
        {
            return Unauthorized("Invalid username or password.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }

    private string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var saltAndHash = hashedPassword.Split('.');
        var salt = Convert.FromBase64String(saltAndHash[0]);
        var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hash == saltAndHash[1];
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// Models for user registration and login
public class RegisterModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}