namespace FileSafetyService.Repositories;

public class DocumentRepository(DapperContext context) : IRepository<Document>
{
    public async Task<Document?> GetById(int id)
    {
        using var connection = context.CreateConnection();

        const string query = """
                             SELECT *
                             FROM Documents
                             WHERE id = @id
                             """;
        
        return await connection.QuerySingleOrDefaultAsync<Document>(query, new { id });
    }

    public async Task<bool?> Update(Document entity)
    {
        using var connection = context.CreateConnection();
        
        const string query =
            """
            UPDATE documents
            SET statutCode = @statut
            WHERE id = @id
            """;

        var res = await connection.ExecuteAsync(query, new { id=entity.Id, statut = entity.StatutCode });

        return res == 1;
    }
}