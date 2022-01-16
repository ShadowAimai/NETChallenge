using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private ChatService _chatService;

        public HomeController(AppIdentityDbContext context, IConfiguration config)
        {
            _context = context;
            _chatService = new ChatService(context, config);
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(_chatService.GetChats());
        }




    }
}
