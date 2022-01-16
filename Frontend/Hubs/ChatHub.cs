using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Frontend.Hubs
{
    public class ChatHub : Hub
    {
        
        private readonly AppIdentityDbContext _context;
        private ChatService _chatService;

        public ChatHub(AppIdentityDbContext context, IConfiguration config)
        {
            _context = context;
            _chatService = new ChatService(context, config);
        }

        public async Task SendMessage(string chat, string user, string message)
        {

            string username = user;

            string output_message = message;

            bool save_entry = true;

            if (message.StartsWith("/stock="))
            {
                var output = await _chatService.CallBot(message);
                username = output.Item1;
                output_message = output.Item2;
                save_entry = output.Item3;
            }

            if (save_entry)
            {
                _chatService.AddConversation(chat, username, output_message);
            }

            await Clients.All.SendAsync("ReceiveMessage", chat, username, output_message, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }
    }
}
