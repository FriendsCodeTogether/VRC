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
        private readonly IHubContext<QueueHub> _queueHubContext;
        private readonly IHubContext<RacingHub> _racingHubContext;
        private readonly CarManagerService _carManagerService;
        private readonly QueueManagerService _queueManagerService;
        private Timer _raceStartCountdownTimer;
        private Timer _raceTimer;
        private Timer _confirmationTimer;
        private int _confirmationTime;
        private bool _isPrepared;
        private int _raceStartCountdown;

        public int LapAmount { get; private set; }
        public bool IsRacing { get; private set; }

        public RaceManagerService(IHubContext<QueueHub> queueHubContext, IHubContext<RacingHub> racingHubContext, CarManagerService carManagerService, QueueManagerService queueManagerService)
        {
            _queueHubContext = queueHubContext;
            _racingHubContext = racingHubContext;
            _carManagerService = carManagerService;
            _queueManagerService = queueManagerService;

            _raceTimer = new Timer();
            _raceStartCountdownTimer = new Timer();
            _raceStartCountdownTimer.Interval = 1000;
            _raceStartCountdownTimer.Elapsed += RaceStartCountdownTimer_Elapsed;

            _raceTimer = new Timer();

            _confirmationTimer = new Timer();
            _confirmationTimer.Interval = 1000;
            _confirmationTimer.Elapsed += ConfirmationTimer_Elapsed;

            _isPrepared = false;
            IsRacing = false;
        }

        /// <summary>
        /// race countdown timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RaceStartCountdownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _raceStartCountdown--;
            Console.WriteLine(_raceStartCountdown);
            await _racingHubContext.Clients.All.SendAsync("UpdateRaceCountdownTime", _raceStartCountdown);
            if (_raceStartCountdown == 0)
            {
                await _racingHubContext.Clients.All.SendAsync("UpdateRaceCountdownTime", "START");
                IsRacing = true;
            }
            if (_raceStartCountdown < 0)
            {
                _raceStartCountdownTimer.Stop();
                await _racingHubContext.Clients.All.SendAsync("RemoveRaceCountdown");
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
            await _queueHubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", _confirmationTime);
            if (_confirmationTime < 0)
            {
                _confirmationTimer.Stop();
                await _queueHubContext.Clients.Group("waitingForConfirm").SendAsync("RemoveConfirm");
            }
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
                await _queueHubContext.Groups.AddToGroupAsync(racer.ConnectionId, "waitingForConfirm");
                await _queueHubContext.Groups.RemoveFromGroupAsync(racer.ConnectionId, "queue");
            }

            await _queueHubContext.Clients.Group("waitingForConfirm").SendAsync("WaitingForConfirm");
        }

        /// <summary>
        /// resets the confirmation timer
        /// </summary>
        private async void ResetConfirmationTimer()
        {
            _confirmationTimer.Stop();
            _confirmationTime = 10;
            _confirmationTimer.Start();
            await _queueHubContext.Clients.Group("waitingForConfirm").SendAsync("UpdateConfirmationTime", _confirmationTime);
        }

        /// <summary>
        /// gets racers from queue
        /// resets cars
        /// sets amount of laps
        /// </summary>
        /// <param name="lapAmount"></param>
        /// <returns></returns>
        public async Task PrepareRace(int lapAmount)
        {
            //get users from queue
            var racerAmount = _carManagerService.Cars.Count;
            await AssignRacersAsync(racerAmount);
            ResetConfirmationTimer();

            //reset stats from the car and assign user to car
            _carManagerService.ResetCars();

            //change amount of laps to selected value on page
            LapAmount = lapAmount;
            Console.WriteLine(LapAmount);

            _isPrepared = true;
        }

        public async Task StartRace()
        {
            if (!_isPrepared)
            {
                return;
            }
            _isPrepared = false;

            _raceStartCountdown = 3;
            _raceStartCountdownTimer.Start();
            await _racingHubContext.Clients.All.SendAsync("UpdateRaceCountdownTime", _raceStartCountdown);
            await _racingHubContext.Clients.All.SendAsync("showRaceCountdown", _raceStartCountdown);

            _raceTimer.Start();
            
        }

        public async Task EndRace()
        {
            IsRacing = false;
            _raceTimer.Stop();
            _carManagerService.RemoveRacers();
            await _racingHubContext.Clients.All.SendAsync("RemoveRacers");
        }
    }
}
