using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using CommunicationTool.Models;
using Microsoft.AspNetCore.Http;
using CommunicationTool.Helper;

namespace CommunicationTool.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly CommunicationToolContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommunicationTool.Controllers.HomeController"/> class.
        /// </summary>
        /// <param name="context">DB Context.</param>
        public HomeController(CommunicationToolContext context)
        {
            _context = context;
          
        }

        /// <summary>
        /// Root Page.
        /// </summary>
        /// <returns>The index.</returns>
        public IActionResult Index()
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }
            return View();
        }

        /// <summary>
        /// Error page handler
        /// </summary>
        /// <returns>The error.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
