using FileUploadService.Context;
using FileUploadService.Models.DBModels;
using FileUploadService.Models.ServiceModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace FileUploadService.Services
{
    public interface IAuthentication_Service
    {
        public Login_Res Login(string username, string password);
    }
    public class Authentication_Service : IAuthentication_Service
    {
        private readonly IConfiguration _configuration;
        public Authentication_Service(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Login_Res Login(string username, string password)
        {
            string accessToken = "";
            DateTime now = DateTime.UtcNow;
            DateTime expireTime = now.AddDays(1);
            DateTimeOffset exp_dto = new DateTimeOffset(expireTime);

            using (var _coreContext = new CoreContext())
            {
                Account ac = _coreContext.Accounts.FirstOrDefault(x => x.Username.Equals(username) && x.IsDeleted == false);

                if (ac == null)
                {
                    throw new Exception("Account Not Found");
                }

                if (!ac.Password.Equals(password))
                {
                    throw new Exception("Wrong Password");
                }

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, ac.AccountId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, ac.Username)
                };

                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Secret"))),
                    SecurityAlgorithms.HmacSha512);

                var token = new JwtSecurityToken(
                     _configuration.GetValue<string>("JWT:ValidIssuer"),
                    null,
                    claims,
                    now,
                    expireTime,
                    signingCredentials);

                accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            }

            return new Login_Res
            {
                AccessToken = accessToken,
                ExpireTime = expireTime
            };
        }
    }
}
