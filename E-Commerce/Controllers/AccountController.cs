using E_Commerce.DTOS;
using E_Commerce.Migrations;
using E_Commerce.Model;
using E_Commerce.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAuthService authService;
        public AccountController(IAuthService service)
        {
            authService = service;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await authService.Register(newUser);
            if (!res.IsAuthenticated)
            {
                return BadRequest(res.Message);
            }

            setRefreshTokenCookie(res.refreshToken, res.refreshTokenExpiration);
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await authService.Login(newUser);
            if (!res.IsAuthenticated)
            {
                return BadRequest(res.Message);
            }
            setRefreshTokenCookie(res.refreshToken, res.refreshTokenExpiration);
            return Ok(res);
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Cookies["refreshToken"];
            var res = await authService.RefreshToken(token);
            if (!res.IsAuthenticated)
                return BadRequest(res.Message);

            setRefreshTokenCookie(res.refreshToken, res.refreshTokenExpiration);
            return Ok(res);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken(RevokeTokenModel refreshToken)
        {
            var token = refreshToken.RefreshToken ?? Request.Cookies["refreshToken"];
            if (token == null)
                return BadRequest("Please Send token");

            var res = await authService.RevokeToken(token);
            if (!res)
                return BadRequest("Invalid Token");

            return Ok("Token Revoked Successfully");

        }


        private void setRefreshTokenCookie(string refreshToken , DateTime expireOn) {
            var options = new CookieOptions()
            {
                Expires = expireOn.ToLocalTime(),
                HttpOnly = true
            };
            Response.Cookies.Append( "refreshToken" , refreshToken , options );
        }
    }
}
