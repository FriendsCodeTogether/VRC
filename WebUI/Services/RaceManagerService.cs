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
        private Timer _raceStartCountdownTimer;
        private Timer _raceTimer;
        private Timer _confirmationTimer;
        private int _confirmationTime;
        private int _lapAmount;
        private bool _isPrepared;
        private int confirmationTime;
        private int raceStartCountdown;

        public bool IsRacing { get; private set; }

        public RaceManagerService(IHubContext<QueueHub> hubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _hubContext = hubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;
            _raceTimer = new Timer();
            _raceStartCountdownTimer = new Timer();
            _raceStartCountdownTimer.Interval = 1000;
            _raceStartCountdownTimer.Elapsed += RaceStartCountdownTimer_Elapsed;

            _raceTimer = new Timer();

            _isPrepared = false;
            IsRacing = false;

            _confirmationTimer = new Timer();
            _confirmationTimer.Interval = 1000;
            _confirmationTimer.Elapsed += ConfirmationTimer_Elapsed;
        }

        private async void RaceStartCountdownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            raceStartCountdown--;
            Console.WriteLine(raceStartCountdown);
            await _hubContext.Clients.Group("racers").SendAsync("UpdateRaceCountdownTime", raceStartCountdown);
            if (raceStartCountdown == 0)
            {
                await _hubContext.Clients.Group("racers").SendAsync("UpdateRaceCountdownTime", "START");
                IsRacing = true;
            }
            if (raceStartCountdown < 0)
            {
                _raceStartCountdownTimer.Stop();
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
            _confirmationTime--;
            await _hubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", _confirmationTime);
            if (_confirmationTime < 0)
            {
                _confirmationTimer.Stop();
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
            _confirmationTimer.Stop();
            _confirmationTime = 10;
            _confirmationTimer.Start();
            await _hubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", _confirmationTime);
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
            _raceStartCountdownTimer.Start();
            await _hubContext.Clients.Group("racers").SendAsync("showRaceCountdown", raceStartCountdown);

            _raceTimer.Start();
        }

        public async void EndRace()
        {
            IsRacing = false;
            _raceTimer.Stop();
            _carManagerService.RemoveRacers();
        }
    }
}
