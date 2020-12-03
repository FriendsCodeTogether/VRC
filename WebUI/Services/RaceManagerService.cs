using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebUI.Hubs;

namespace WebUI.Services
{
    public class RaceManagerService
    {
        private readonly IHubContext<QueueHub> _hubContext;
        private readonly CarManagerService _carManagerService;
        private readonly QueueManagerService _queueManagerService;
        private Timer raceTimer;

        public RaceManagerService(IHubContext<QueueHub> hubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _hubContext = hubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;
            raceTimer = new Timer();
        }

        public void ResetCars()
        {
          foreach(var car in _carManagerService.Cars)
            {
                car.BestLap = TimeSpan.Zero;
                car.EndTime = TimeSpan.Zero;
                //car timer still has to be implemented
            }
        }

        public void PrepareRace()
        {
            var racerAmount = _carManagerService.Cars.Count;
            var racers = _queueManagerService.TakeFromQueueAsync(racerAmount);
            ResetCars();
            //bericht naar user om te bevestigen of die gaat racen
            raceTimer.Dispose();
            //change amount of laps to selected value on page

        }

    }
}
