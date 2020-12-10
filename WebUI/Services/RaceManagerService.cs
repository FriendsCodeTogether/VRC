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
        private Timer confirmationTimer;
        private int _lapAmount;
        private bool _isPrepared;
        private int confirmationTime;

        public RaceManagerService(IHubContext<QueueHub> hubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _hubContext = hubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;
            raceTimer = new Timer();
            
            _isPrepared = false;

            confirmationTimer = new Timer();
            confirmationTimer.Interval = 1000;
            confirmationTimer.Elapsed += ConfirmationTimer_Elapsed;
        }

        private async void ConfirmationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            confirmationTime--;
            await _hubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", confirmationTime);
            if (confirmationTime < 0)
            {
                confirmationTimer.Stop();
                await _hubContext.Clients.Group("waitingForConfirm").SendAsync("RemoveConfirm");
            }
        }

        public async Task PrepareRace(int lapAmount)
        {
            //get users from queue
            //var racerAmount = _carManagerService.Cars.Count;
            var racerAmount = 2;
            await AssignRacersAsync(racerAmount);
            ResetConfirmationTimer();

            //bericht naar user om te bevestigen of die gaat racen
            //await _hubContext.Clients.Group("racers").SendAsync("ReadyRacers");

            /*//reset stats from the car and assign user to car
            _carManagerService.ResetCartimes();
            _carManagerService.ConnectRacersToCar(racers);

            //reset timer
            raceTimer.Dispose();

            //change amount of laps to selected value on page
            _lapAmount = lapAmount;

            _isPrepared = true;*/
        }

        /// <summary>
        /// get the given amount of users from the queue and sends them the notification to join within 10 seconds
        /// </summary>
        /// <param name="racerAmount"></param>
        /// <returns></returns>
        private async Task AssignRacersAsync(int racerAmount)
        {
            var racers = await _queueManagerService.TakeFromQueueAsync(racerAmount);
            foreach (var racer in racers.ToList())
            {
                await _hubContext.Groups.AddToGroupAsync(racer.ConnectionId, "waitingForConfirm");
                await _hubContext.Groups.RemoveFromGroupAsync(racer.ConnectionId, "queue");
            }

            await _hubContext.Clients.Group("waitingForConfirm").SendAsync("WaitingForConfirm");
        }

        private async void ResetConfirmationTimer()
        {
            confirmationTimer.Stop();
            confirmationTime = 30;
            confirmationTimer.Start();
            await _hubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", confirmationTime);
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
