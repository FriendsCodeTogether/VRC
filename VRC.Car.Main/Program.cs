using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VRC.Car.Main.Messaging;
using VRC.Car.Main.Hardware;
using VRC.Shared.Messaging;
using System.Device.I2c;

namespace VRC.Car.Main
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var HardwareController = new HardwareController();
            HardwareController.Initialise();

            // var messagingHandler = new MessagingHandler();
            // messagingHandler.CarCommandReceivedEvent += (s, e) =>
            // {
            //     HardwareController.SendCarCommand(e.CarCommand);
            // };

            // await messagingHandler.ConnectAsync();

            while (true)
            {

                // read color sensor
                // read acc sensor
                // read ultrasoon

                Console.WriteLine("Press a key to send the car command forward");
                Console.Read();

                HardwareController.SendCarCommand(new CarCommand { CarNumber = 1, Direction = 'L', Throttle = 'F'});

                Console.WriteLine("Press a key to send the car command off");
                Console.Read();

                HardwareController.SendCarCommand(new CarCommand { CarNumber = 1, Direction = 'N', Throttle = 'N'});

                Console.WriteLine("Press a key to send the car command backwards");
                Console.Read();

                HardwareController.SendCarCommand(new CarCommand { CarNumber = 1, Direction = 'R', Throttle = 'B'});

                Console.WriteLine("Press a key to read color sensor");
                Console.Read();

                var read = HardwareController.ReadColorSensor();
                Console.WriteLine(read);

                await Task.Delay(1000);
            }
        }
    }
}
