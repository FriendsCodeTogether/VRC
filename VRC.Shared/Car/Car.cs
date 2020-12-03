using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRC.Shared.Car
{
    public class Car
    {
        public Car() {}
        public Car(int carNumber, string connentionId)
        {
            CarNumber = carNumber;
            ConnectionId = connentionId;
        }

        public int CarNumber { get; set; }

        public string ConnectionId { get; set; }

    }
}
