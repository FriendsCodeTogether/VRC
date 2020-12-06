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
        private int _lapAmount;

        public RaceManagerService(IHubContext<QueueHub> hubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _hubContext = hubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;
            raceTimer = new Timer();
        }

       

        public async void PrepareRace(int lapAmount)
        {
            //get users from queue 
            var racerAmount = _carManagerService.Cars.Count;
            var racers = await _queueManagerService.TakeFromQueueAsync(racerAmount);

            //bericht naar user om te bevestigen of die gaat racen

            //reset stats from the car and assign user to car
            _carManagerService.ResetCartimes();
            _carManagerService.ConnectRacersToCar(racers);

            //reset timer
            raceTimer.Dispose();

            //change amount of laps to selected value on page
            _lapAmount = lapAmount;
        }


        public void StartRace()
        {
            raceTimer.Start();
        }


        public void EndRace()
        {
            raceTimer.Stop();
            _carManagerService.RemoveRacers();
        }

    }
}
