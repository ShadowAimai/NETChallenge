using Frontend.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class ChatService : IChatService
    {
        private readonly IConfiguration _config;

        static readonly HttpClient client = new HttpClient();

        private readonly AppIdentityDbContext _context;

        public ChatService(AppIdentityDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<Tuple<string, string, bool>> CallBot(string message) {

            string username = "CHATBOT";
            string output_message = "The given code does not exist.";

            try
            {

                string serviceUri = _config.GetSection("ServiceUri").Value;

                string stock_code = message.Substring(7);

                if (stock_code.Length > 0)
                {
                    HttpResponseMessage response = await client.GetAsync(serviceUri + stock_code);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var list = JsonConvert.DeserializeObject<List<Models.Stock>>(responseBody);

                    foreach (var item in list)
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
            catch (Exception ex)
            {
                output_message = "Could not make the request.";
            }



            return new Tuple<string, string, bool>(username, output_message, false);

        }

        public void AddConversation(string chat, string username, string output_message)
        {
            _context.Conversations.Add(new Conversation() { ChatRoom = chat, Username = username, Message = output_message, date = DateTime.Now });
            _context.SaveChanges();

            RemoveOldConversations(chat);

        }

        public dynamic GetChats()
        {

            dynamic chats = new ExpandoObject();

            var Bezos = _context.Conversations.Where(x => x.ChatRoom == "Bezos").ToList();
            var Musk = _context.Conversations.Where(x => x.ChatRoom == "Musk").ToList();
            var Arnault = _context.Conversations.Where(x => x.ChatRoom == "Arnault").ToList();

            chats.Bezos = Bezos;
            chats.Musk = Musk;
            chats.Arnault = Arnault;
            return chats;

        }

        public void RemoveOldConversations(string chat)
        {
            var recent_conversations = _context.Conversations.OrderByDescending(x => x.date)
                                                                .Where(_x => _x.ChatRoom == chat)
                                                                .Take(50)
                                                                .Select(x => x.Id)
                                                                .ToArray();

            var old_conversations = _context.Conversations.Where(_x => !recent_conversations.Contains(_x.Id) && _x.ChatRoom == chat);

            _context.Conversations.RemoveRange(old_conversations);

            _context.SaveChanges();
        }

    }
}
