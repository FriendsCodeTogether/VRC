using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using VRC.Shared.Car;
using System.Collections.Concurrent;
using WebUI.Services;

namespace WebUI.Hubs
{
    public class RacingHub : Hub
    {
        private readonly CarManagerService _carManagerService;
        private readonly RaceManagerService _raceManagerService;

        public RacingHub(CarManagerService carManagerService, RaceManagerService raceManagerService)
        {
            _carManagerService = carManagerService;
            _raceManagerService = raceManagerService;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var car = _carManagerService.Cars.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            _carManagerService.Cars.Remove(car);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Send a CarCommand to a car if a race is in progress
        /// </summary>
        /// <param name="carNumber">The car to send it to</param>
        /// <param name="command">The CarCommand to be sent</param>
        public async Task SendCarCommand(int carNumber, CarCommand command)
        {
            if (_raceManagerService.IsRacing)
            {
                await Clients.Client(GetConnectionIdByCarNumber(carNumber)).SendAsync("ReceiveCarCommand", command);
            }
        }

        /// <summary>
        /// Assign a new CarNumber to a car
        /// </summary>
        /// <param name="connectionId">The connectionId associated with the car</param>
        /// <param name="carNumber">The intended car number</param>
        public async Task AssignCarNumber(string connectionId, int carNumber) => await Clients.Client(connectionId).SendAsync("AssignCarNumber", carNumber);

        /// <summary>
        /// Allows a car to request a car number when it connects.
        /// </summary>
        /// <param name="carNumber"></param>
        public async Task RequestCarNumber()
        {
            await AssignNewCarNumber(Context.ConnectionId);
        }

        /// <summary>
        /// Allows a car to associaste it's car number with it's new connectionId after a disconnect.
        /// If the car number has been re-assigned to another car then a new number will be provided.
        /// </summary>
        /// <param name="carNumber"></param>
        public async Task ReclaimCarNumber(int carNumber)
        {
            var connectionId = Context.ConnectionId;
            Car existingCar;
            lock (_carManagerService.CarsLock)
            {
                existingCar = _carManagerService.Cars.FirstOrDefault(c => c.CarNumber == carNumber);
            }
            if (existingCar == null)
            {
                lock (_carManagerService.CarsLock)
                {
                    Car car = new Car(carNumber, connectionId);
                    _carManagerService.Cars.Add(car);
                } 
            }
            else
            {
                await AssignNewCarNumber(connectionId);
            }
        }

        /// <summary>
        /// Assign a new car number to a car
        /// </summary>
        /// <param name="connectionId">The connectionId of the car to assign to</param>
        private async Task AssignNewCarNumber(string connectionId)
        {
            var newCarNumber = FindAvailableNumber();
            Car car = new Car(newCarNumber, connectionId);
            lock (_carManagerService.CarsLock)
            {
                _carManagerService.Cars.Add(car);
            }
            await AssignCarNumber(connectionId, car.CarNumber);
        }

        /// <summary>
        /// Selects the first available CarNumber from the list
        /// </summary>
        /// <returns>The first available car number</returns>
        private int FindAvailableNumber()
        {
            lock (_carManagerService.CarsLock)
            {
                for (var i = 1; i <= _carManagerService.Cars.Count + 1; i++)
                {
                    if (_carManagerService.Cars.FirstOrDefault(c => c.CarNumber == i) != null)
                    {
                        continue;
                    }
                    return i;
                }
                return 0;
            } 
        }

        /// <summary>
        /// Returns the connectionId for a given car number
        /// </summary>
        private string GetConnectionIdByCarNumber(int carNumber) => _carManagerService.Cars.First(c => c.CarNumber == carNumber).ConnectionId;

        /// <summary>
        /// Prepares the settings for the race
        /// </summary>
        public async Task PrepareRaceAsync(int lapAmount) => await _raceManagerService.PrepareRace(lapAmount);

        /// <summary>
        /// connecets a racer to a car
        /// </summary>
        public int ConnectRacerToCar(string userId) => _carManagerService.ConnectRacerToCar(userId);

        /// <summary>
        /// starts the race
        /// </summary>
        public async Task StartRaceAsync() => await _raceManagerService.StartRace();

        /// <summary>
        /// gets the amount of laps to display on the racer page
        /// </summary>
        /// <returns>int lapAmount</returns>
        public int GetLapAmount() => _raceManagerService.LapAmount;

        /// <summary>
        /// stops the race and removes all racers from their cars and the racing page
        /// </summary>
        public async Task StopRaceAsync() => await _raceManagerService.EndRace();

        /// <summary>
        /// when an admin connects, put them in the admin group to be able to call them specifically
        /// </summary>
        public async Task PutAdminInGroupAsync() => await Groups.AddToGroupAsync(Context.ConnectionId, "admins");

        /// <summary>
        /// when a racer connects, put them in the racer group to be able to call them specifically
        /// </summary>
        public async Task PutRacerInGroupAsync() => await Groups.AddToGroupAsync(Context.ConnectionId, "racers");

    }
}
