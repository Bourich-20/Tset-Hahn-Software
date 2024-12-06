using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTrackerAPI.Models;


namespace ExpenseTrackerAPI.Services
{
   public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string GenerateJwtToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? string.Empty),
            new Claim(ClaimTypes.Name, user.LastName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("userId", user.Id.ToString()) 
        };

        var secretKey = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("JWT Secret Key is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpirationInMinutes")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

}
