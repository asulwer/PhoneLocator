using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace PhoneLocator
{
    public class LocationHub : Hub
    {
        public Task<object> SendNotifications(string msg)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<LocationHub>();
            return context.Clients.All.receiveNotification(msg);
        }
    }
}