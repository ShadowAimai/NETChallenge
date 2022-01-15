using Frontend.Models;
using System;
using System.Dynamic;
using System.Linq;

namespace Frontend.Utils
{
    public class ChatHandler
    {
        private readonly AppIdentityDbContext _context;

        public ChatHandler(AppIdentityDbContext context)
        {
            _context = context;
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
