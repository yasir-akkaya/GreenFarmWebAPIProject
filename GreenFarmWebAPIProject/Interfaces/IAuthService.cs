using GreenFarmWebAPIProject.Models;

namespace GreenFarmWebAPIProject.Interfaces
{
    public interface IAuthService
    {
        public Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
    }
}
