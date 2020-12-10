using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebUI.Entities;
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
        private bool _isPrepared;

        public RaceManagerService(IHubContext<QueueHub> hubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _hubContext = hubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;
            raceTimer = new Timer();
            _isPrepared = false;
        }



        public async Task PrepareRace(int lapAmount)
        {
            //get users from queue
            //var racerAmount = _carManagerService.Cars.Count;
            var racerAmount = 1;
            var racers = await _queueManagerService.TakeFromQueueAsync(racerAmount);
            foreach (var racer in racers.ToList())
            {
                await _hubContext.Groups.AddToGroupAsync(racer.ConnectionId, "racers");
            }

            //bericht naar user om te bevestigen of die gaat racen
            await _hubContext.Clients.Group("racers").SendAsync("ReadyRacers");

            /*//reset stats from the car and assign user to car
            _carManagerService.ResetCartimes();
            _carManagerService.ConnectRacersToCar(racers);

            //reset timer
            raceTimer.Dispose();

            //change amount of laps to selected value on page
            _lapAmount = lapAmount;

            _isPrepared = true;*/
        }


        public void StartRace()
        {
            // if (!_isPrepared)
            // {
            //     return;
            // }

            raceTimer.Start();
        }


        public void EndRace()
        {
            raceTimer.Stop();
            _carManagerService.RemoveRacers();
        }

    }
}
