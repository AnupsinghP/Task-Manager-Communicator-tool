using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CommunicationTool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommunicationTool.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    public class UsersController : Controller
    {
        private readonly CommunicationToolContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommunicationTool.Controllers.UsersController"/> class.
        /// </summary>
        /// <param name="context">DB Context.</param>
        public UsersController(CommunicationToolContext context) {    
            _context = context;
            addAdmin();


        }

        /// <summary>
        /// Adds the admin if database is newly created and no user is present.
        /// </summary>
        private void addAdmin() 
        { 
            if(_context.Users.Count() == 0) {
                _context.Users.Add(new User() { Username="admin", Password="admin",IsDevTeam=1});
                _context.SaveChangesAsync();
            } 
        }

        /// <summary>
        /// Return Users page.
        /// </summary>
        /// <returns>The index.</returns>
        //GET: Users
        public async Task<IActionResult> Index()
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }
            //Return all users
            return View(await _context.Users.ToListAsync());
        }

        /// <summary>
        /// Navigate to create new user page .
        /// </summary>
        /// <returns>Create user page</returns>
        // GET: Users/Create
        public IActionResult Create()
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }
            return View();
        }

        /// <summary>
        /// Create the user.
        /// </summary>
        /// <returns>Users page</returns>
        /// <param name="user">User.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }
           
            //Check if all details are valid or not
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        /// <summary>
        /// Navigate to confirm delete page.
        /// </summary>
        /// <returns>Confirm Delete.</returns>
        /// <param name="id">User id.</param>
        // GET: Users/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <returns>Users page.</returns>
        /// <param name="id">User Id.</param>
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }

            //Fetch the user to delete
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Details the specified User.
        /// </summary>
        /// <returns>The details.</returns>
        /// <param name="id">User Id.</param>
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            //Fetch data to be shown
            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    }
}
