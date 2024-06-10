namespace API.Hubs;

public class MyHub : Hub
{
    public async Task JoinGroup(int demandeId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, demandeId.ToString());
    }

    public async Task LeaveGroup(int demandeId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, demandeId.ToString());
    }

    public async Task SendMessage(int demandeId, int documentId, string typeDocument, string scanResult)
    {
        await Clients
            .Group(demandeId.ToString())
            .SendAsync("GetScanResult", new { demandeId, documentId, typeDocument, scanResult });
    }
}