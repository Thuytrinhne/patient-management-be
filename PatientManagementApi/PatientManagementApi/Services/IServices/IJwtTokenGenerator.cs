using System.Security.Claims;

namespace PatientManagementApi.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IEnumerable<Claim> payloads, string secretKey, int expireTimeInSeconds);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey);
    }
}
