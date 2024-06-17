using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorUI.Application.Model;

public class DocumentsPageState(int demandeId, HubConnection? hubConnection)
{
    public readonly int DemandeId = demandeId;
    public readonly HubConnection? HubConnection = hubConnection;
}