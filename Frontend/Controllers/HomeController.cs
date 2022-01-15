using Frontend.Models;
using Frontend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private ChatHandler _chatHandler;

        public HomeController(AppIdentityDbContext context)
        {
            _context = context;
            _chatHandler = new ChatHandler(context);
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(_chatHandler.GetChats());
        }




    }
}
