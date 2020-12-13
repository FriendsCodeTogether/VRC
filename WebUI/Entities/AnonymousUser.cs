using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Entities
{
    public class AnonymousUser
    {
        public AnonymousUser(string userId, string connectionId)
        {
            UserId = userId;
            ConnectionId = connectionId;
        }

        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
