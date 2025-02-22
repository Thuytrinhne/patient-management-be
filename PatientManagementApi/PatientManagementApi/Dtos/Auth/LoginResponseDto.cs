namespace PatientManagementApi.Dtos.Auth
{
    public class LoginResponseDto
    {
        public Guid UserId { get; set; }
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime {get; set;} 
    }
}
