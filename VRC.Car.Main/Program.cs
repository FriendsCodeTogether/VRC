using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VRC.Car.Main.Messaging;
using VRC.Shared.Messaging;

namespace VRC.Car.Main
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var messagingHandler = new MessagingHandler();
            await messagingHandler.ConnectAsync();

            var command = new CarCommand
            {
                CarNumber = messagingHandler.CarNumber,
                Throttle = 1,
                Direction = -1
            };
            while (true)
            {
                // var carNumber = messagingHandler.CarNumber;
                // await messagingHandler.SendCarCommandAsync(carNumber, command);
                // Console.WriteLine("Message sent");
                await Task.Delay(2000);
            }
        }
    }
}
