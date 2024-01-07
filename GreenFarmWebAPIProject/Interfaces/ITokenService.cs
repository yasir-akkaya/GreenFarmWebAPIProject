using GreenFarmWebAPIProject.Models;

namespace GreenFarmWebAPIProject.Interfaces
{
    public interface ITokenService
    { 
        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request);

    }
}
