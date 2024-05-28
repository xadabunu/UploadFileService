namespace API.Repositories;

public class DemandeRepository(DapperContext context): IRepository<Demande>
{
    public async Task<Demande> Create(Demande demande)
    {
        const string query =
            """
            INSERT INTO demandes (numeroAttente, numeroRole)
            OUTPUT Inserted.Id
            VAlUES (null, null)
            """;

        using var connection = context.CreateConnection();

        var id = await connection.QuerySingleAsync<int>(query);

        return (await SelectById(id, connection))!;
    }

    public async Task<Demande?> Update(Demande demande)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Demande?> GetById(int id)
    {
        using var connection = context.CreateConnection();

        return await SelectById(id, connection);
    }

    public async Task<IEnumerable<Demande>> GetAll()
    {
        const string query =
            """
            SELECT *
            FROM demandes
            """;

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Demande>(query);
    }

    private static async Task<Demande?> SelectById(int id, IDbConnection connection)
    {
        const string query =
            """
            SELECT *
            FROM demandes
            WHERE id = @id
            """;

        return await connection.QuerySingleOrDefaultAsync<Demande>(query, new { id });
    }
}