using E_Commerce.DTOS;
using E_Commerce.JWT_Options;
using E_Commerce.Migrations;
using E_Commerce.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JWT jwtOptions;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> options)
        {
            this.userManager = userManager;
            jwtOptions = options.Value;
        }

        public async Task<AuthModel> Register(UserRegisterDTO newUser)
        {
            if (await userManager.FindByEmailAsync(newUser.Email) is not null)
            {
                return new AuthModel()
                {
                    Message = "EMail Is Already Registered"
                };
            }
            if (await userManager.FindByNameAsync(newUser.UserName) is not null)
            {
                return new AuthModel()
                {
                    Message = "UserName Is Already Registered"
                };
            }

            var user = new ApplicationUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,

            };

            var res = await userManager.CreateAsync(user, newUser.Password);
            if (!res.Succeeded)
            {
                string errors = "";
                foreach (var error in res.Errors)
                {
                    errors += error.Description;
                    errors += " , ";
                }
                return new AuthModel()
                {
                    Message = errors
                };
            }

            await userManager.AddToRoleAsync(user, "User");

            var jwtToken = await GenerateJWT(user);
            var refreshToken = GenerateRefreshToken();

            user.refreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
            return new AuthModel()
            {
                UserName = user.UserName,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                TokenExpiration = jwtToken.ValidTo,
                refreshToken = refreshToken.Token,
                refreshTokenExpiration = refreshToken.ExpiresOn
            };

        }
        public async Task<AuthModel> Login(LoginDTO user)
        {
            AuthModel authModel = new AuthModel();
            var userDb = await userManager.FindByEmailAsync(user.Email);
            if (userDb is null || !await userManager.CheckPasswordAsync(userDb, user.Password))
            {
                return new AuthModel()
                {
                    Message = "Invalid Email Or Passsword"
                };
            }

            var userRoles = await userManager.GetRolesAsync(userDb);
            var jwtToken = await GenerateJWT(userDb);

            var refreshToken = userDb.refreshTokens.FirstOrDefault(t => t.IsActive);
            if (refreshToken is null)
            {
                var Rtoken = GenerateRefreshToken();
                authModel.refreshToken = Rtoken.Token;
                authModel.refreshTokenExpiration = Rtoken.ExpiresOn;

                userDb.refreshTokens.Add(Rtoken);
                await userManager.UpdateAsync(userDb);
            }
            else
            {
                authModel.refreshToken = refreshToken.Token;
                authModel.refreshTokenExpiration = refreshToken.ExpiresOn;
            }

            authModel.IsAuthenticated = true;
            authModel.Roles = userRoles.ToList();
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.TokenExpiration = jwtToken.ValidTo;
            authModel.UserName = userDb.UserName;

            return authModel;


        }
        public async Task<AuthModel> RefreshToken(string refreshToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.refreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                return new AuthModel
                {
                    Message = "Invalid Token"
                };
            }
            var token = user.refreshTokens.Single(t => t.Token == refreshToken);
            if (token.IsActive == false)
            {
                return new AuthModel
                {
                    Message = "Refresh Token Is Expired Please Re-Login"
                };
            }

            token.RevokedON = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            var newJWT = await GenerateJWT(user);
            user.refreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            var roles = await userManager.GetRolesAsync(user);

            return new AuthModel
            {
                IsAuthenticated = true,
                refreshToken = newRefreshToken.Token,
                refreshTokenExpiration = newRefreshToken.ExpiresOn,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(newJWT),
                TokenExpiration = newJWT.ValidTo,
                UserName = user.UserName

            };

        }
        public async Task<bool> RevokeToken(string refreshToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.refreshTokens.Any(u => u.Token == refreshToken));
            if (user == null)
                return false;

            var token = user.refreshTokens.Single(t => t.Token == refreshToken);
            if (token.IsActive == false)
                return false;

            token.RevokedON = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
            return true;
        }
        private async Task<JwtSecurityToken> GenerateJWT(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            var claims = new List<Claim>();
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var myClaims = new[]
            {
                new Claim( ClaimTypes.NameIdentifier , user.Id ),
                new Claim( ClaimTypes.Name , user.UserName ),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            }.Union(claims)
            .Union(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                expires: DateTime.Now.AddMinutes(jwtOptions.DurationInMinutes + 10),
                claims: myClaims,
                signingCredentials: signingCredentials
                );

            return jwtToken;

        }
        private RefreshToken GenerateRefreshToken()
        {
            var token = Guid.NewGuid().ToString();
            return new RefreshToken
            {
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(10),
                Token = token,
            };

        }
    }
}
