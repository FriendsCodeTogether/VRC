using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebUI.Services;

namespace WebUI.Hubs
{
    public class QueueHub : Hub
    {
        private readonly QueueManagerService _queueManagerService;

        public QueueHub(QueueManagerService queueManagerService)
        {
            _queueManagerService = queueManagerService ?? throw new ArgumentNullException(nameof(queueManagerService));
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _queueManagerService.UserConnectionIdList.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Allows a user to register his userId with his connectionId
        /// </summary>
        /// <param name="userId"></param>
        public void RegisterUserId(string userId)
        {
            _queueManagerService.UserConnectionIdList.TryAdd(Context.ConnectionId, userId);
        }

        /// <summary>
        /// Allows a user to add himself to the Queue
        /// </summary>
        /// <param name="userId"></param>
        public async Task JoinTheQueue(string userId)
        {
            _queueManagerService.TryAddToQueue(userId);
            await SendQueuePosition(userId);
        }

        /// <summary>
        /// Send a user his position in the waiting queue
        /// </summary>
        /// <param name="userId"></param>
        private async Task SendQueuePosition(string userId) => await Clients.Client(GetConnectionIdByUserId(userId)).SendAsync("ReceiveQueuePosition", _queueManagerService.GetQueuePosition(userId));

        /// <summary>
        /// Returns the connectionId for a given userId
        /// </summary>
        private string GetConnectionIdByUserId(string userId) => _queueManagerService.UserConnectionIdList.First(c => c.Value == userId).Key;
    }
}
