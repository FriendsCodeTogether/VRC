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
                    atmega1.WriteByte(32);
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"Problem connecting to {nameof(atmega1)}");
                    throw;
                }

                try
                {
                    atmega2.WriteByte(32);
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
                atmega1.WriteByte(I2cConstants.Motor);
                atmega1.Write(BitConverter.GetBytes(Convert.ToSByte(carCommand.Direction)));
                atmega1.Write(BitConverter.GetBytes(Convert.ToSByte(carCommand.Throttle)));
            }
        }

        public float ReadAccelerationSensor()
        {
            var accelerationSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(I2cConstants.AccelerationSensor));
            var readresult = new Span<byte>(new byte[4]);
            lock (i2cLock)
            {
                atmega1.WriteRead(accelerationSensorSpan, readresult);
            }
            var acceleration = BitConverter.ToSingle(readresult);

            return acceleration;
        }

        public int ReadColorSensor()
        {
            var accelerationSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(I2cConstants.ColorSensor));
            var readresult = new Span<byte>(new byte[2]);
            lock (i2cLock)
            {
                atmega1.WriteRead(accelerationSensorSpan, readresult);
            }
            var color = BitConverter.ToInt16(readresult);

            return color;
        }

        public int ReadUltrasonicSensor()
        {
            var accelerationSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(I2cConstants.UltrasonicSensor));
            var readresult = new Span<byte>(new byte[2]);
            lock (i2cLock)
            {
                atmega1.WriteRead(accelerationSensorSpan, readresult);
            }
            var distance = BitConverter.ToInt16(readresult);

            return distance;
        }
    }
}
