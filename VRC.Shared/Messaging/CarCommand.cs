using System;

namespace VRC.Shared.Messaging
{
    public class CarCommand
    {
        public char CarNumber { get; set; }
        public char Throttle { get; set; }
        public char Direction { get; set; }
    }
}
