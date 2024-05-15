using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Authetication;
using UserManagement.DTO;
using UserManagement.ServiceContract;

namespace UserManagement.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService (IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AutheticationResponse CreateJwtToken(AppUser user)
        {
           DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
            Claim[] claims = new Claim[]
            {
                new Claim (JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),  //JWT Unique ID
                new Claim (JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),    // Issued at (date and time of token generation)
                new Claim (ClaimTypes.NameIdentifier,user.Email.ToString()), //Unique name identifier of the user (email)
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                SigningCredentials signingCredentials =new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                 _configuration["Jwt:Audience"],
                claims,
                expires:expiration,
                signingCredentials:signingCredentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            string token =tokenHandler.WriteToken(tokenGenerator);

            return new AutheticationResponse () { Token = token, Email= user.Email , Name=user.Name, Expiration= expiration};
        }
    }
}
