using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Entities;
using WebUI.Entities;

namespace WebUI.Services
{
    public class CarManagerService
    {
        // Singleton
        public List<Car> Cars { get; set; } = new();

        public void ResetCartimes()
        {
            foreach (var car in Cars)
            {
                car.BestLap = TimeSpan.Zero;
                car.EndTime = TimeSpan.Zero;
                car.lapTimer.Dispose();
            }
        }

        public void ConnectRacersToCar(IEnumerable<AnonymousUser> racers)
        {
            foreach (var racer in racers)
            {
                foreach (var car in Cars)
                {
                    if (car.ConnectionId == null)
                    {
                        car.ConnectionId = racer.ConnectionId;
                        break;
                    }
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
