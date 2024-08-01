namespace PatientManagementApi.ConfigKeys
{
    public class JwtOptions
    {
        public const string Key = "JwtTokenSettings";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public string AccessTokenExpirationTimeInSeconds { get; set;}
        public string RefreshSecret { get; set; }
        public string RefreshTokenExpirationTimeInSeconds { get; set;}
    }
}
