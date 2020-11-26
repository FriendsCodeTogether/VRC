using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VRC.Car.Main.Messaging;
using VRC.Shared.Car;

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
                await Task.Delay(2000);
            }
        }
    }
}
