namespace PatientManagementApi.Settings
{
    public class ConnectionStringOptions
    {
        public const string Key = "ConnectionStrings";
        public string PostgresConstr { get; set; }
        public string RedisConstr { get; set; }
    }
}
