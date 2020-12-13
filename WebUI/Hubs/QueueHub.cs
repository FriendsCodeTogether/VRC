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

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await _queueManagerService.RemoveInnactiveUserAsync(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Allows a user to register his userId with his connectionId
        /// </summary>
        /// <param name="userId"></param>
        public void UpdateConnectionId(string userId) => _queueManagerService.UpdateConnectionId(userId, Context.ConnectionId);

        /// <summary>
        /// Allows a user to add himself to the Queue
        /// </summary>
        /// <param name="userId"></param>
        public async Task JoinTheQueue(string userId)
        {
            _queueManagerService.TryAddToQueue(userId, Context.ConnectionId);
            await SendQueuePosition(userId);
        }

        /// <summary>
        /// Send a user his position in the waiting queue
        /// </summary>
        /// <param name="userId"></param>
        public async Task SendQueuePosition(string userId) => await Clients.Caller.SendAsync("ReceiveQueuePosition", _queueManagerService.GetQueuePosition(userId));

        public async Task RacerConfirmedAsync(string connectionId) => await _queueManagerService.RacerConfirmedAsync(Context.ConnectionId);

    }
}
