namespace FileSafetyService;

public record ScanResultMessage(int demandeId,
    int DocumentId, string DocumentType, string ResultCode, string connectionId);