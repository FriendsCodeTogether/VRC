using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Extensions;
using WebUI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace WebUI.Services
{
    public class QueueManagerService
    {
        private readonly IHubContext<QueueHub> _hubContext;

        public QueueManagerService(IHubContext<QueueHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public ConcurrentDictionary<string, string> UserConnectionIdList = new();
        private ConcurrentQueue<string> WaitingUsers { get; set; } = new();

        public void TryAddToQueue(string userId)
        {
            if (!WaitingUsers.Contains(userId))
            {
                WaitingUsers.Enqueue(userId);
            }
        }

        public async Task<IEnumerable<string>> TakeFromQueueAsync(int amount)
        {
            var dequeuedUsers = WaitingUsers.DequeueChunk(amount);
            await SendQueuePositionChangedAsync();
            return dequeuedUsers;
        }

        private async Task SendQueuePositionChangedAsync()
        {
            await _hubContext.Clients.All.SendAsync("RequestQueuePosition");
        }

        public int GetQueuePosition(string userId)
        {
            var position = WaitingUsers.ToArray().ToList().IndexOf(userId);
            return position == -1 ? -1 : position + 1;
        }
    }
}
