using Model.Entities;

namespace API.Repositories;

public class DocumentRepository(DapperContext context): IRepository<Document>
{
    // #pragma warning disable 8613
    public async Task<Document> Create(Document document)
    {
        const string query = """
            INSERT INTO documents (typeCode, nom, description, statutCode, demandeId)
            OUTPUT Inserted.Id
            VALUES (@typeCode, @nom, @description, @statutCode, @demandeId)
            """;
        
        using var connection = context.CreateConnection();
        
        var id = await connection.QuerySingleAsync<int>(query, document);

        return (await SelectById(id, connection))!;
    }

    public async Task<Document?> Update(Document document)
    {
        const string query = """
            UPDATE documents
            SET statutCode = @statutCode
            WHERE id = @id
            """;

        using var connection = context.CreateConnection();

        await connection.ExecuteAsync(query, new { id = document.Id, statutCode = document.StatutCode });

        return await SelectById(document.Id, connection);
    }
    
    public async Task Delete(int id)
    {
        const string query = """
            DELETE FROM documents
            WHERE id = @id
            """;

        using var connection = context.CreateConnection();

        await connection.ExecuteAsync(query, new { id });
    }

    private static async Task<Document?> SelectById(int id, IDbConnection connection)
    {
        const string query = """
            SELECT *
            FROM Documents
            WHERE id = @id
            """;
        
        return await connection.QuerySingleOrDefaultAsync<Document>(query, new { id });
    }   

    public async Task<Document?> GetById(int id)
    {
        using var connection = context.CreateConnection();

        return await SelectById(id, connection);
    }

    public async Task<IEnumerable<Document>> GetAll()
    {
        const string query = """
            SELECT *
            FROM documents
        """;

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Document>(query);
    }

    public async Task<IEnumerable<Document>> GetProjetByDemandeId(int demandeId)
    {
        const string query =
            """
            SELECT *
            FROM documents
            WHERE demandeId = @demandeId
            AND typeCode = 'projet'
            """;

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Document>(query, new { demandeId });
    }

    public async Task<IEnumerable<Document>> GetAnnexesByDemandeId(int demandeId)
    {
        const string query =
            """
            SELECT *
            FROM documents
            WHERE demandeId = @demandeId
            AND typeCode = 'annexeProjet'
            """;

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Document>(query, new { demandeId });
    }

    public async Task<IEnumerable<Document>> GetAutresDocumentsByDemandeId(int demandeId)
    {
        const string query =
            """
            SELECT *
            FROM documents
            WHERE demandeId = @demandeId
            AND typeCode = 'autreDocument'
            """;

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Document>(query, new { demandeId });
    }
}