using System;

namespace VRC.Car.Main.Hardware
{
    public class I2cConstants
    {
        public readonly char Motor { get; } = 'm';
        public readonly char AccelerationSensor { get; } = 'a';
        public readonly char ColorSensor { get; } = 'c';
        public readonly char LightSensor { get; } = 'l';
        public readonly char UltrasonicSensor { get; } = 'u';
        public readonly char Buzzer { get; } = 'b';
        public readonly char Screen { get; } = 's';
    }
}
