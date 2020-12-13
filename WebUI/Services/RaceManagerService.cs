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
        private Timer raceStartCountdownTimer;
        private Timer confirmationTimer;
        private int _lapAmount;
        private bool _isPrepared;
        private int confirmationTime;
        private int raceStartCountdown;

        public RaceManagerService(IHubContext<QueueHub> hubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _hubContext = hubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;
            raceTimer = new Timer();
            raceStartCountdownTimer = new Timer();
            raceStartCountdownTimer.Interval = 1000;
            raceStartCountdownTimer.Elapsed += RaceStartCountdownTimer_Elapsed;

            _isPrepared = false;

            confirmationTimer = new Timer();
            confirmationTimer.Interval = 1000;
            confirmationTimer.Elapsed += ConfirmationTimer_Elapsed;
        }

        private async void RaceStartCountdownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            raceStartCountdown--;
            Console.WriteLine(raceStartCountdown);
            await _hubContext.Clients.Group("racers").SendAsync("UpdateRaceCountdownTime", raceStartCountdown);
            if (raceStartCountdown == 0)
            {
                await _hubContext.Clients.Group("racers").SendAsync("UpdateRaceCountdownTime", "START");
            }
            if (raceStartCountdown < 0)
            {
                raceStartCountdownTimer.Stop();
                await _hubContext.Clients.Group("racers").SendAsync("RemoveRaceCountdown");
            }
        }

        /// <summary>
        /// gives the seconds of the confirmation timer, when countdown reaches 0, the timer stops and the user that didnt asnwer are send to the home page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            var racerAmount = 1;
            await AssignRacersAsync(racerAmount);
            ResetConfirmationTimer();
            Console.WriteLine("test prepare");

            //reset stats from the car and assign user to car
            _carManagerService.ResetCartimes();


            //change amount of laps to selected value on page
            _lapAmount = lapAmount;

            _isPrepared = true;
        }

        /// <summary>
        /// get the given amount of users from the queue and sends them the notification to join within 30 seconds
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
            confirmationTime = 10;
            confirmationTimer.Start();
            await _hubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", confirmationTime);
        }

        public async Task StartRace()
        {
            // if (!_isPrepared)
            // {
            //     return;
            // }
            await _hubContext.Clients.Group("racers").SendAsync("ConnectRacersToCar");
            Console.WriteLine("user connected to car");
            raceStartCountdown = 3;
            Console.WriteLine(raceStartCountdown);
            raceStartCountdownTimer.Start();
            await _hubContext.Clients.Group("racers").SendAsync("showRaceCountdown", raceStartCountdown);

        }

        public void EndRace()
        {
            raceTimer.Stop();
            _carManagerService.RemoveRacers();
        }

    }
}
