using System;

namespace VRC.Car.Main.Hardware
{
    public class I2cConstants
    {
        public static byte Motor { get; } = Convert.ToByte('m');
        public static byte ColorSensor { get; } = Convert.ToByte('c');
        public static byte LightSensor { get; } = Convert.ToByte('l');
        public static byte UltrasonicSensor { get; } = Convert.ToByte('u');
        public static byte Buzzer { get; } = Convert.ToByte('b');

        public static byte DirectionLeft { get; } = Convert.ToByte('L');
        public static byte DirectionRight { get; } = Convert.ToByte('R');
        public static byte DirectionNeutral { get; } = Convert.ToByte('N');
        public static byte ThrottleForward { get; } = Convert.ToByte('F');
        public static byte ThrottleBackward { get; } = Convert.ToByte('B');
        public static byte ThrottleNeutral { get; } = Convert.ToByte('N');
    }
}
