using System;

namespace VRC.Car.Main.Hardware
{
    public class I2cConstants
    {
        public static byte Motor { get; } = Convert.ToByte('m');
        public static byte AccelerationSensor { get; } = Convert.ToByte('a');
        public static byte ColorSensor { get; } = Convert.ToByte('c');
        public static byte LightSensor { get; } = Convert.ToByte('l');
        public static byte UltrasonicSensor { get; } = Convert.ToByte('u');
        public static byte Buzzer { get; } = Convert.ToByte('b');
        public static byte Screen { get; } = Convert.ToByte('s');
    }
}
