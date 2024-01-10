using GreenFarmWebAPIProject.Interfaces;
using GreenFarmWebAPIProject.Models;
using GreenFarmWebAPIProject.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace GreenFarmWebAPIProject.Services
{
    public class AuthService : IAuthService
    {
        readonly ITokenService tokenService;
        private readonly ApplicationDbContext _db;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(ITokenService tokenService, ApplicationDbContext context)
        {
            _db = context;
            this.tokenService = tokenService;
            //_httpContextAccessor = httpContextAccessor;
        }


        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            UserLoginResponse response = new();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = _db.Users.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);

            if (user != null)   
            {
                //HttpContext.Session.SetString("userMail", user.Email);
                var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest { Email = request.Email });
                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;

            }

            return response;

        }


        //public IActionResult Logout()
        //{
        //    _httpContextAccessor.HttpContext.Session.Clear();

        //    return null;
        //}

    }
}
