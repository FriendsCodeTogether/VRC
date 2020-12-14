using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VRC.Shared.Car;

namespace VRC.Car.Main.Messaging
{
    public class MessagingHandler
    {
        private readonly string _hubUrl = "https://192.168.0.201:5001/messaginghub";
        private HubConnection _hubConnection;

        public event EventHandler<CarCommandEventArgs> CarCommandReceivedEvent;

        public int CarNumber { get; set; }

        public MessagingHandler()
        {
            Initialise();
        }

        public bool IsConnected =>
        _hubConnection.State == HubConnectionState.Connected;

        private void Initialise()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .Build();

            _hubConnection.On<CarCommand>("ReceiveCarCommand", (command) =>
            {
                Console.WriteLine($"Car number: {command.CarNumber} Car throttle: {command.Throttle} Car direction: {command.Direction}");
                CarCommandReceivedEvent?.Invoke(this, new CarCommandEventArgs(command));
            });

            _hubConnection.On<int>("AssignCarNumber", (carNumber) =>
            {
                CarNumber = carNumber;
                Console.WriteLine($"Our new car number: {carNumber}");
            });

            _hubConnection.Closed += async (error) =>
            {
                Console.WriteLine("Connection lost");
                await ConnectAsync();
            };

            _hubConnection.Reconnected += async (newConnectionId) =>
            {
                await _hubConnection.SendAsync("ReclaimCarNumber", CarNumber);
            };
        }

        public async Task ConnectAsync()
        {
            Console.WriteLine($"Connecting to API at \"{_hubUrl}\"...");
            while (!IsConnected)
            {
                try
                {
                    await _hubConnection.StartAsync();
                    Console.WriteLine("Connected to API");
                    await RequestCarNumberAsync();
                    while (CarNumber == 0)
                    {
                        await Task.Delay(50);
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Failed to connect to API");
                    Console.WriteLine("Retrying...");
                    await Task.Delay(2000);
                }
            }
        }

        // obsolete, for testing
        public async Task SendCarCommandAsync(int carNumber, CarCommand command) =>
            await _hubConnection.SendAsync("SendCarCommand", carNumber, command);

        private async Task RequestCarNumberAsync() =>
            await _hubConnection.SendAsync("RequestCarNumber");
    }
}
