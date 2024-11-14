using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    public string Generate(Account account)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

        var Credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(account),
            SigningCredentials = Credentials,
            Expires = DateTime.UtcNow.AddHours(2),
        };

        var token = handler.CreateToken(tokenDescriptor);

        var strToken = handler.WriteToken(token);

        return strToken;
    }

    public static ClaimsIdentity GenerateClaims(Account account)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(
            ClaimTypes.Name, account.AccountNumber
        ));

        return ci;

    }
}