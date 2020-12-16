using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRC.Shared.Car;
using WebUI.Entities;

namespace WebUI.Services
{
    public class CarManagerService
    {
        // Singleton
        public ConcurrentBag<Car> Cars { get; set; } = new();

        public void ResetCartimes()
        {
            foreach (var car in Cars)
            {
                car.BestLap = TimeSpan.Zero;
                car.EndTime = TimeSpan.Zero;
                car.lapTimer.Dispose();
            }
        }

        public void ConnectRacerToCar(string connectionId)
        {
            foreach (var car in Cars)
            {
                if (car.ConnectionId == null)
                {
                    car.ConnectionId = connectionId;
                    break;
                }
            }
        }

        public void RemoveRacers()
        {
            foreach (var car in Cars)
            {
                car.ConnectionId = null;
            }
        }

    }
}
