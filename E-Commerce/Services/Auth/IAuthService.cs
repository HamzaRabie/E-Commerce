using E_Commerce.DTOS;
using E_Commerce.Model;

namespace E_Commerce.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthModel> Register(UserRegisterDTO newUser);
        Task<AuthModel> Login(LoginDTO user);
        Task<AuthModel> RefreshToken(string refreshToken);
        Task<bool> RevokeToken(string refreshToken);
    }
}
