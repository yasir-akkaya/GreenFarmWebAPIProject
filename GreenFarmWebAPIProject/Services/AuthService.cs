using GreenFarmWebAPIProject.Interfaces;
using GreenFarmWebAPIProject.Models;
using GreenFarmWebAPIProject.Models.Data;

namespace GreenFarmWebAPIProject.Services
{
    public class AuthService : IAuthService
    {
        readonly ITokenService tokenService;
        private readonly ApplicationDbContext _db;
        public AuthService(ITokenService tokenService, ApplicationDbContext context)
        {
            _db = context;
            this.tokenService = tokenService;
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
                var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest { Email = request.Email });

                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
            }

            return response;

        }

    }
}
