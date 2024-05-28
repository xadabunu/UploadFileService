namespace API.Repositories;

public class DapperContext(IConfiguration config)
{
    private readonly string _connectionString = config.GetConnectionString("Eprolex")!;
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
