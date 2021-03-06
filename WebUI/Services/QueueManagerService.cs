﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Extensions;
using WebUI.Hubs;
using Microsoft.AspNetCore.SignalR;
using WebUI.Entities;

namespace WebUI.Services
{
    public class QueueManagerService
    {
        private readonly IHubContext<QueueHub> _hubContext;

        public QueueManagerService(IHubContext<QueueHub> hubContext)
        {
            _hubContext = hubContext;
        }

        private ConcurrentQueue<AnonymousUser> WaitingUsers { get; set; } = new();

        public async void TryAddToQueue(string userId, string connectionId)
        {
            if (!WaitingUsers.Any(u => u.UserId == userId))
            {
                WaitingUsers.Enqueue(new AnonymousUser(userId, connectionId));
                await _hubContext.Groups.AddToGroupAsync(connectionId, "queue");
            }
        }

        public async Task<IEnumerable<AnonymousUser>> TakeFromQueueAsync(int amount)
        {
            var dequeuedUsers = WaitingUsers.DequeueChunk(amount);
            await SendQueuePositionChangedAsync();
            return dequeuedUsers;
        }

        private async Task SendQueuePositionChangedAsync()
        {
            await _hubContext.Clients.Group("queue").SendAsync("RequestQueuePosition");
        }

        public int GetQueuePosition(string userId)
        {
            var waitingUserList = WaitingUsers.ToArray().ToList();
            var user = waitingUserList.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return -1;
            }
            var position = waitingUserList.IndexOf(user);
            return position == -1 ? -1 : position + 1;
        }

        /// <summary>
        /// Returns the connectionId for a given userId
        /// </summary>
        public string GetConnectionIdByUserId(string userId) => WaitingUsers.FirstOrDefault(u => u.UserId == userId)?.ConnectionId;

        /// <summary>
        /// Update the connectionId for a given userId
        /// </summary>
        /// <param name="userId"></param>
        /// /// <param name="connectionId">The new connectionId</param>
        public void UpdateConnectionId(string userId, string connectionId)
        {
            var user = WaitingUsers.ToArray().ToList().FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.ConnectionId = connectionId;
            }
        }

        public async Task RemoveInnactiveUserAsync(string connectionId)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            var innactiveUser = WaitingUsers.ToArray().ToList().FirstOrDefault(u => u.ConnectionId == connectionId);
            if (innactiveUser != null)
            {
                WaitingUsers.Remove(innactiveUser);
                await SendQueuePositionChangedAsync();
            }
        }
    }
}
