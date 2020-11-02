using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs
{
    public class MessagingHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
