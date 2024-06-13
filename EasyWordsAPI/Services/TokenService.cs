using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyWordsAPI.Models;
using EasyWordsAPI.Models.DataTransfer;
using Microsoft.IdentityModel.Tokens;

namespace EasyWordsAPI.Services;

public static class TokenService
{
    public static string Key = "super_secret_string_123_poiuytrewqasdfghjklzxcvbnm";
    
    public static string GetToken(ClaimsPrincipal principal)
    {
        List<Claim> claims = principal.Claims.ToList();

        SigningCredentials credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.Default.GetBytes(Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: "easywords",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(12),
                claims: claims,
                signingCredentials: credentials
            );
        
        var handler = new JwtSecurityTokenHandler();
        string result = handler.WriteToken(token);

        return result;
    }
}