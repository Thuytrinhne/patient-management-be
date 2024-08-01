using PatientManagementApi.Dtos.Auth;
namespace PatientManagementApi.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginDto loginDto);

    }
}
