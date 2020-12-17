﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace WebUI.Entities
{
    public class Car
    {
        public Car() {}

        public Car(int carNumber, string connentionId)
        {
            CarNumber = carNumber;
            ConnectionId = connentionId;
        }

        public int CarNumber { get; set; }

        public string ConnectionId { get; set; }

        public TimeSpan BestLap { get; set; }

        public TimeSpan EndTime { get; set; }

        public Timer lapTimer;

    }
}