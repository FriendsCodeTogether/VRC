using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using VRC.Shared.Messaging;

namespace WebUI.Hubs
{
    public class MessagingHub : Hub
    {
        public async Task SendMessage(string user, CarCommand command)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, command);
        }
    }
}
