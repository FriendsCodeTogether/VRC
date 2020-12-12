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
            // var messagingHandler = new MessagingHandler();
            // await messagingHandler.ConnectAsync();

            // var command = new CarCommand
            // {
            //     CarNumber = messagingHandler.CarNumber,
            //     Throttle = 1,
            //     Direction = -1
            // };

            // Console.WriteLine("Hello I2C!");
            // I2cDevice i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x20));
            // i2c.WriteByte(0x60);
            // var read = i2c.ReadByte();
            // Console.WriteLine(read);


            var HardwareController = new HardwareController();
            HardwareController.Initialise();

            // Console.WriteLine("Connecting to API...");
            // var messagingHandler = new MessagingHandler();
            // await messagingHandler.ConnectAsync();

            while (true)
            {

                // read color sensor
                // read acc sensor
                // read ultrasoon

                Console.WriteLine("Press a key to send the car command");
                Console.Read();

                HardwareController.SendCarCommand(null);

                // await Task.Delay(2000);
            }
        }
    }
}
