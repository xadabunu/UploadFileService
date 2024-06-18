namespace FileSafetyService;

public record ScanResultMessage(int DemandeId, int DocumentId, string ResultCode, string ConnectionId);