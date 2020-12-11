using System;
using System.Device.I2c;
using VRC.Shared.Messaging;

namespace VRC.Car.Main.Hardware
{
    public class HardwareController
    {
        private I2cDevice atmega1;
        private I2cDevice atmega2;

        private readonly object i2cLock = new();

        public void Initialise()
        {
            Console.WriteLine("Initializing devices...");
            atmega1 = I2cDevice.Create(new I2cConnectionSettings(1, 0x20));
            atmega2 = I2cDevice.Create(new I2cConnectionSettings(1, 0x30));

            TestI2cDevices();
        }

        public void TestI2cDevices()
        {
            lock (i2cLock)
            {
                Console.WriteLine("Testing I2C devices...");
                try
                {
                    atmega1.WriteByte(0x10);
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"Problem connecting to {nameof(atmega1)}");
                    throw;
                }

                try
                {
                    atmega2.WriteByte(0x10);
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"Problem connecting to {nameof(atmega2)}");
                    throw;
                }
            }
        }

        public void SendCarCommand(CarCommand carCommand)
        {
            lock (i2cLock)
            {
                atmega1.WriteByte(0x20);
            }
        }
    }
}
