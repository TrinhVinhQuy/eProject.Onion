﻿using Microsoft.AspNetCore.SignalR;

namespace WebASPMvcCore.Insfrastructure.Services
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(message);
        }
    }
}
