using PatientManagementApi.ConfigKeys;
using PatientManagementApi.Dtos.Auth;
using PatientManagementApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;

namespace PatientManagementApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
        IOptionsSnapshot<JwtOptions> jwtOptions,
        IJwtTokenGenerator jwtTokenGenerator,
        SignInManager<ApplicationUser> signInManager,
        IUnitOfWork unitOfWork)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(loginDto.Email);
                var claims =await  generateClaimsForAccessToken(user);
                var accessToken = _jwtTokenGenerator.GenerateToken(claims, _jwtOptions.Secret, int.Parse(_jwtOptions.AccessTokenExpirationTimeInSeconds));
                var refreshToken = _jwtTokenGenerator.GenerateToken(claims, _jwtOptions.RefreshSecret, int.Parse(_jwtOptions.RefreshTokenExpirationTimeInSeconds));

                await updateRefreshTokenForUser(user, refreshToken);
                LoginResponseDto loginResponseDto = new LoginResponseDto()
                {
                    UserId = user.Id,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpirationTime = getExpirationTimeFromToken(refreshToken, _jwtOptions.RefreshSecret)
                };
                return loginResponseDto;
            }
            else
            {
                throw new AuthenticationException("Invalid login credentials.");
            }
        }

        private DateTime getExpirationTimeFromToken(string refreshToken, string secretKey)
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(refreshToken, secretKey);
            var expirationClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expirationClaim != null)
            {
                var expirationSeconds = long.Parse(expirationClaim.Value);
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expirationSeconds).DateTime;
                return expirationTime;
            }
            throw new InvalidOperationException("Failed to retrieve expiration time from token.");
        }

        private async Task updateRefreshTokenForUser(ApplicationUser user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<IEnumerable<Claim>> generateClaimsForAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.FirstName + " " + user.LastName)
            };
            var roles = await _signInManager.UserManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;
        }
    } 
}
