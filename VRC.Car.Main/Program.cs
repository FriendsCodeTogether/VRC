using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace VRC.Car.Main
{
    public class Program
    {
        private static HubConnection hubConnection;
        private static List<string> messages = new List<string>();

        static async Task Main(string[] args)
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/chathub")
            .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
            });

            await hubConnection.StartAsync();
        }

        static Task Send() =>
        hubConnection.SendAsync("SendMessage", "userInput", "messageInput");
    }
}
