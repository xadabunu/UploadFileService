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

    public async Task<Document?> Update(Document entity)
    {
        throw new NotImplementedException();
    }
}