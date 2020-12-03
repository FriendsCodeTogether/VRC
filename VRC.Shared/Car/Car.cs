using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRC.Shared.Car
{
    public class Car
    {
        public Car() { CarId = new Guid().ToString(); }
        public Car(int carNumber, string connentionId)
        {
            CarId = new Guid().ToString();
            CarNumber = carNumber;
            ConnectionId = connentionId;
        }

        public string CarId { get; set; }

        public int CarNumber { get; set; }

        public string ConnectionId { get; set; }

    }
}
