using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Roomly.Shared.Options;

namespace Roomly.Shared.Auth.Services;

public class IdentityService
{
    private readonly JwtOptions _jwtOptions;
    private readonly byte[] _key;

    public IdentityService(IOptions<JwtOptions> jwtSettings)
    {
        _jwtOptions = jwtSettings.Value;
        _key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);
    }

    public static JwtSecurityTokenHandler TokenHandler = new();

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);

        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
    {
        return new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours),
            Audience = _jwtOptions.Audiences[0],
            Issuer = _jwtOptions.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
    }
}