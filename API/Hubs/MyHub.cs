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

    public async Task Notify(int demandeId, string senderId)
    {
        await Clients.GroupExcept(demandeId.ToString(), senderId).SendAsync("UpdateNotification");
    }
}