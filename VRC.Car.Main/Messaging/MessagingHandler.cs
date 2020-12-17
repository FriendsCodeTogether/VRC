using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VRC.Shared.Car;

namespace VRC.Car.Main.Messaging
{
    public class MessagingHandler
    {
        private readonly string _hubUrl = "https://localhost:5001/racinghub";
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
            // Create a handler that accepts custom (untrusted) certificates
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => {
                    // Validate the cert here and return true if it's correct.
                    // If this is a development app, you could just return true always
                    // In production you should ALWAYS either use a trusted cert or check the thumbprint of the cert matches one you expect.
                    return true;
                }
            };
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl, options =>
            {
                // Register the custom handler above and also configure WebSockets
                options.HttpMessageHandlerFactory = _ => handler;
                options.WebSocketConfiguration = sockets =>
                {
                    sockets.RemoteCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => {
                        // You have to repeat your cert validation code here. Feel free to use a helper method!
                        return true;
                    });
                };
            })
            .Build();

            _hubConnection.On<CarCommand>("ReceiveCarCommand", (command) =>
            {
                // Console.WriteLine($"Car number: {command.CarNumber} Car throttle: {command.Throttle} Car direction: {command.Direction}");
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
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException?.Message);
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

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
