using System;
using Xunit;
using Frontend;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Frontend.Services;
using Frontend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace FrontendUT
{
    public class UnitTest1 : IClassFixture<TestSetup>
    {
        private readonly IConfiguration _config;
        private readonly AppIdentityDbContext _context;


        private ServiceProvider _serviceProvider;
        private ChatService _chatService;

        public UnitTest1(TestSetup testSetup)
        {
            _serviceProvider = testSetup.ServiceProvider;

            var appIdentBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
            appIdentBuilder.UseInMemoryDatabase("TempDatabase");
            _context = new AppIdentityDbContext(appIdentBuilder.Options);


            var myConfiguration = new Dictionary<string, string>{
                {"ServiceUri", "https://localhost:44380/Stock/Get/"},
                {"ConnectionStrings:DefaultConnection", "Server=HESTIA\\SQLEXPRESS;Initial Catalog=Frontend;Integrated Security=True;MultipleActiveResultSets=true"},
            };

            _config = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

            _chatService = new ChatService(_context, _config);
        }

        [Fact]
        public void create_new_conversation_empty_conversations()
        {

            _chatService.AddConversation("TEST", "Tester", "This is a test message");

            var inserted = _context.Conversations.Any(x => x.Username == "Tester");

            Assert.True(inserted);

        }

        [Fact]
        public void create_new_conversation_full_conversations()
        {

            for(int i = 0; i < 60; i++)
            {
                _context.Conversations.Add(new Conversation()
                {
                    ChatRoom = "TEST",
                    Username = "Tester",
                    Message =   String.Format("Test No. {0}", i.ToString())
                });
            }

            _context.SaveChanges();

            _chatService.AddConversation("TEST", "Tester", "This is a test message");

            var total_records = _context.Conversations.Where(x => x.ChatRoom == "TEST").Count();

            Assert.Equal(50, total_records);

        }

        [Fact]
        public void get_chats()
        {

            for (int i = 0; i < 10; i++)
            {
                _context.Conversations.Add(new Conversation()
                {
                    ChatRoom = "Bezos",
                    Username = "Tester",
                    Message = String.Format("Test No. {0}", i.ToString())
                });

                _context.Conversations.Add(new Conversation()
                {
                    ChatRoom = "Musk",
                    Username = "Tester",
                    Message = String.Format("Test No. {0}", i.ToString())
                });

                _context.Conversations.Add(new Conversation()
                {
                    ChatRoom = "Arnault",
                    Username = "Tester",
                    Message = String.Format("Test No. {0}", i.ToString())
                });

            }

            _context.SaveChanges();

            var data = _chatService.GetChats();

            Assert.Equal(10, data.Bezos.Count);
            Assert.Equal(10, data.Musk.Count);
            Assert.Equal(10, data.Arnault.Count);

        }

        [Fact]
        public void remove_conversations_exceeding_fifty_records()
        {

            for (int i = 0; i < 60; i++)
            {
                _context.Conversations.Add(new Conversation()
                {
                    ChatRoom = "Fifty",
                    Username = "Tester",
                    Message = String.Format("Test No. {0}", i.ToString())
                });

            }

            _context.SaveChanges();

            _chatService.RemoveOldConversations("Fifty");

            var total_records = _context.Conversations.Where(x => x.ChatRoom == "TEST").Count();

            Assert.Equal(50, total_records);

        }


    }
}
