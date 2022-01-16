using System;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface IChatService
    {
        public Task<Tuple<string, string, bool>> CallBot(string message);

        public void AddConversation(string chat, string username, string output_message);

        public dynamic GetChats();

        public void RemoveOldConversations(string chat);
    }
}
