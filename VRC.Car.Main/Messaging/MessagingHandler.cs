using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VRC.Shared.Messaging;

namespace VRC.Car.Main.Messaging
{
    public class MessagingHandler
    {
        private readonly string hubUrl = "https://localhost:5001/messaginghub";
        private HubConnection hubConnection;

        public MessagingHandler()
        {
            Initialise();
        }

        public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;

        private void Initialise()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

            hubConnection.On<string, CarCommand>("ReceiveMessage", (user, command) =>
            {
                Console.WriteLine($"MessageReceived! from {user}.");
                Console.WriteLine($"Car number: {command.CarNumber} Car throttle: {command.Throttle} Car direction: {command.Direction}");
            });
        }

        public async Task ConnectAsync()
        {
            while (!IsConnected)
            {
                try
                {
                    await hubConnection.StartAsync();
                    Console.WriteLine("Connected");
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Failed to connect...");
                    Console.WriteLine("Retrying");
                    await Task.Delay(2000);
                }
            }
        }

        public Task Send(string user, CarCommand command) =>
            hubConnection.SendAsync("SendMessage", user, command);
    }
}
