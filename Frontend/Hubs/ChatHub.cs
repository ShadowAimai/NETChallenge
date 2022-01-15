using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Utils;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Frontend.Hubs
{
    public class ChatHub : Hub
    {
        static readonly HttpClient client = new HttpClient();
        private readonly AppIdentityDbContext _context;
        private ChatHandler _chatHandler;

        public ChatHub(AppIdentityDbContext context)
        {
            _context = context;
            _chatHandler = new ChatHandler(context);
        }

        public async Task SendMessage(string chat, string user, string message)
        {

            string username = user;

            string output_message = message;

            bool save_entry = true;

            if (message.StartsWith("/stock="))
            {
                username = "CHATBOT";
                output_message = "The given code does not exist.";
                save_entry = false;

                string stock_code = message.Substring(7);

                if (stock_code.Length > 0)
                {
                    HttpResponseMessage response = await client.GetAsync(string.Format("https://localhost:44380/Stock/Get/{0}", stock_code));
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var list = JsonConvert.DeserializeObject<List<Models.Stock>>(responseBody); 

                    foreach(var item in list)
                    {

                        if (item.Close != "N/D")
                        {
                            output_message = String.Format("{0} quote is {1}  per share", item.Symbol, item.Close);
                        }

                    }

                }
                else
                {
                    output_message = "Code cannot be empty.";
                }

            }

            if (save_entry)
            {
                _chatHandler.AddConversation(chat, username, output_message);
            }

            await Clients.All.SendAsync("ReceiveMessage", chat, username, output_message, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }
    }
}
