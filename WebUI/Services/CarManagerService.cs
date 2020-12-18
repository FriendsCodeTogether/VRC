using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Entities;

namespace WebUI.Services
{
    public class CarManagerService
    {
        // Singleton
        public List<Car> Cars { get; set; } = new();
        public object CarsLock { get; set; } = new();

        public void ResetCars()
        {
            lock (CarsLock)
            {
                foreach (var car in Cars)
                {
                    Console.WriteLine(car.CarNumber);
                    Console.WriteLine(car.UserId);

                    car.UserId = null;
                    car.BestLap = TimeSpan.Zero;
                    car.EndTime = TimeSpan.Zero;
                    car.lapTimer.Dispose();
                }
            }
        }

        public int ConnectRacerToCar(string userId)
        {
            lock (CarsLock)
            {
                foreach (var car in Cars)
                {
                    Console.WriteLine(car.CarNumber);
                    Console.WriteLine(car.UserId);

                    if (car.UserId == null)
                    {
                        car.UserId = userId;
                        return car.CarNumber;
                    }
                }
                return -1;
            }
        }

        public void RemoveRacers()
        {
            lock (CarsLock)
            {
                foreach (var car in Cars)
                {
                    car.UserId = null;
                }
            }
        }
    }
}
