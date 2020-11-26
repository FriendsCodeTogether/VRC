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

        public RacingHub(CarManagerService carManagerService)
        {
            _carManagerService = carManagerService;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _carManagerService.CarConnectionIdList.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Send a CarCommand to a car
        /// </summary>
        /// <param name="carNumber">The car to send it to</param>
        /// <param name="command">The CarCommand to be sent</param>
        public async Task SendCarCommand(int carNumber, CarCommand command) => await Clients.Client(GetConnectionIdByCarNumber(carNumber)).SendAsync("ReceiveCarCommand", command);

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
            if (!_carManagerService.CarConnectionIdList.Values.Contains(carNumber))
            {
                _carManagerService.CarConnectionIdList.TryAdd(connectionId, carNumber);
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
            _carManagerService.CarConnectionIdList.TryAdd(connectionId, newCarNumber);
            await AssignCarNumber(connectionId, _carManagerService.CarConnectionIdList[connectionId]);
        }

        /// <summary>
        /// Selects the first available CarNumber from the list
        /// </summary>
        /// <returns>The first available car number</returns>
        private int FindAvailableNumber()
        {
            for (var i = 1; i <= _carManagerService.CarConnectionIdList.Count + 1; i++)
            {
                if (_carManagerService.CarConnectionIdList.Values.Contains(i))
                {
                    continue;
                }
                return i;
            }
            return 0;
        }

        /// <summary>
        /// Returns the connectionId for a given car number
        /// </summary>
        private string GetConnectionIdByCarNumber(int carNumber) => _carManagerService.CarConnectionIdList.First(c => c.Value == carNumber).Key;
    }
}
