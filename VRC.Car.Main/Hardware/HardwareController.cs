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
        private readonly byte direction = 0x20;
        private readonly byte throttle = 0x30;
        private readonly byte accelerationSensor = 0x40;

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
                atmega1.WriteByte(direction);
                atmega1.Write(BitConverter.GetBytes(carCommand.Direction));

                atmega1.WriteByte(throttle);
                atmega1.Write(BitConverter.GetBytes(carCommand.Throttle));
            }
        }

        public float ReadAccelerationSensor()
        {
            var accelerationSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(accelerationSensor));
            var readresult = new Span<byte>(new byte[50]);
            lock (i2cLock)
            {
                atmega1.WriteRead(accelerationSensorSpan, readresult);
            }
            var acceleration = BitConverter.ToSingle(readresult);

            return acceleration;
        }
    }
}
