using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace VRC.Car.Main
{
    public class Program
    {
        private HubConnection hubConnection;
        private List<string> messages = new List<string>();

        static void Main(string[] args)
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
            .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        Task Send() =>
        hubConnection.SendAsync("SendMessage", userInput, messageInput);
    }
}
