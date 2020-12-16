using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Shared.Car;

namespace VRC.Car.Main.Messaging
{
    public class CarCommandEventArgs
    {
        public CarCommand CarCommand { get; set; }

        public CarCommandEventArgs(CarCommand carCommand)
        {
            CarCommand = carCommand;
        }
    }
}
