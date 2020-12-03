using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRC.Shared.Car;

namespace WebUI.Services
{
    public class CarManagerService
    {
        // Singleton
        public ConcurrentBag<Car> Cars { get; set; } = new();
        public ConcurrentDictionary<string, int> CarConnectionIdList { get; set; } = new();
    }
}
