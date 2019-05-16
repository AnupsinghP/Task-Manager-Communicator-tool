    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunicationTool.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    // For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

    namespace CommunicationTool.Controllers
    {
        /// <summary>
        /// Login controller.
        /// </summary>
        public class LoginController : Controller
        {
            private readonly CommunicationToolContext _context;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CommunicationTool.Controllers.LoginController"/> class.
            /// </summary>
            /// <param name="context">DB Context.</param>
            public LoginController(CommunicationToolContext context) 
            {
                _context = context;
            }
            /// <summary>
            /// Login root page handler.
            /// </summary>
            /// <returns>The index.</returns>
            // GET: /Login/
            public IActionResult Index()
            {
                return View();
            }

            /// <summary>
            /// Autherize the specified user.
            /// </summary>
            /// <returns>Corrosonding Page.</returns>
            /// <param name="user">Current user.</param>
            [HttpPost]
            public ActionResult Autherize(User user)
            {
                //Fetch user Details
                var userDetails = _context.Users.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    ViewBag.ErrorMessage = "Wrong username or password.";
                    return View("Index", user);
                }
                else
                { 
                    HttpContext.Session.SetString("Username", userDetails.Username);
                    var s = HttpContext.Session.GetString("Username");
                    HttpContext.Session.SetString("IsDevTeam", userDetails.IsDevTeam == 1?"true":"false");
                    ViewData["userName"] = user.Username;
                    return RedirectToAction("Index", "Home");
                }
            }

            /// <summary>
            /// Logout this instance.
            /// </summary>
            /// <returns>Login Page.</returns>
            public ActionResult Logout() 
            {
                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Login");
            }
        }
    }
