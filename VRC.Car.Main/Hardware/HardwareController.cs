using System;
using System.Device.I2c;
using System.Text;
using VRC.Shared.Car;

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
                    Console.WriteLine($"{nameof(atmega1)} online");
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"Problem connecting to {nameof(atmega1)}");
                    throw;
                }

                try
                {
                    // atmega2.WriteByte(32);
                    // Console.WriteLine($"{nameof(atmega2)} online");
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
            var writeBuffer = new byte[3] { I2cConstants.Motor, (byte)carCommand.Direction, (byte)carCommand.Throttle };
            lock (i2cLock)
            {
                atmega1.Write(new ReadOnlySpan<byte>(writeBuffer));
            }
        }

        public float ReadAccelerationSensor()
        {
            throw new NotImplementedException();
        }

        public int ReadColorSensor()
        {
            var colorSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(I2cConstants.ColorSensor));
            var readresult = new Span<byte>(new byte[2]);
            lock (i2cLock)
            {
                atmega1.WriteRead(colorSensorSpan, readresult);
            }

            var color = BitConverter.ToInt16(readresult);

            return color;
        }

        public int ReadUltrasonicSensor()
        {
            var ultrasonicSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(I2cConstants.UltrasonicSensor));
            var readresult = new Span<byte>(new byte[2]);
            lock (i2cLock)
            {
                atmega1.WriteRead(ultrasonicSensorSpan, readresult);
            }
            var distance = BitConverter.ToInt16(readresult);

            return distance;
        }

        public bool ReadLightSensor()
        {
            var lightSensorSpan = new ReadOnlySpan<byte>(BitConverter.GetBytes(I2cConstants.LightSensor));
            var readresult = new Span<byte>(new byte[1]);
            lock (i2cLock)
            {
                atmega1.WriteRead(lightSensorSpan, readresult);
            }

            var lightSensorValue = BitConverter.ToBoolean(readresult);

            return lightSensorValue;
        }

        public void SetBuzzer(bool value)
        {
            var writeBuffer = new byte[2] { I2cConstants.Buzzer, Convert.ToByte(value) };
            lock (i2cLock)
            {
                atmega1.Write(new ReadOnlySpan<byte>(writeBuffer));
            }
        }
    }
}
