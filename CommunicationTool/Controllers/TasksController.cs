using System;
using System.Collections.Generic;
using CommunicationTool.Helper;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicationTool.Models;
using CommunicationTool.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task = CommunicationTool.Models.Task;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommunicationTool.Controllers
{
    /// <summary>
    /// Tasks controller.
    /// </summary>
    public class TasksController : Controller
    {
        private readonly CommunicationToolContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommunicationTool.Controllers.TasksController"/> class.
        /// </summary>
        /// <param name="context">DB Context.</param>
        public TasksController(CommunicationToolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Root page for tasks.
        /// </summary>
        /// <returns>Tasks page.</returns>
        // GET: /Tasks/
        public async Task<IActionResult> Index()
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            //Fetch all the task
            //Lazy loading
            var taskList = await (_context.Tasks
                    .Include(x => x.CreatedBy)
                    .Include(x => x.AssignedTo)
                    .Include(x => x.Attachment)).ToListAsync();

            return View(taskList);
        }

        /// <summary>
        /// Navigagte to create task page.
        /// </summary>
        /// <returns>The create.</returns>
        // GET: Users/Create
        public IActionResult Create()
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            ViewBag.UserName = HttpContext.Session.GetString("Username");

            //Intiaize view model
            var task = new ViewModel.TaskDetailsViewModel();

            return View(task);
        }

        /// <summary>
        /// Creates the specified task.
        /// </summary>
        /// <returns>Index page.</returns>
        /// <param name="task">TaskDetailsViewModel to create the objects</param>
        // POST: Task/Create
        [HttpPost]
        public async Task<IActionResult> Create(TaskDetailsViewModel task)
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            if (ModelState.IsValid)
            {
                //create task
                await createTask(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        /// <summary>
        /// Details the Task for specified id.
        /// </summary>
        /// <returns>The details page.</returns>
        /// <param name="id">Task Identifier.</param>
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            //Fetch Task
            var task = await _context.Tasks
                     .Include(x => x.CreatedBy)
                     .Include(x => x.AssignedTo)
                     .Include(x => x.Attachment).Where(a => a.TaskId == id).FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound();
            }

            //Fill FillTaskConversation View Model to show data
            return View(FillTaskConversationVM(task));
        }

        /// <summary>
        /// Update the feedback/response on Task.
        /// </summary>
        /// <returns>Details Page.</returns>
        /// <param name="s">Form values</param>
        public async Task<ActionResult> Comment(IFormCollection s)
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            //Fetch required data to create the feedback object
            int tid = Convert.ToInt32(s.Where(ky => ky.Key == "TaskId").FirstOrDefault().Value);
            var task = _context.Tasks.Where(tsk => tsk.TaskId == tid).FirstOrDefault();
            var user = _context.Users.Where(usr => usr.Username == HttpContext.Session.GetString("Username")).FirstOrDefault();

            //Create Feedback object
            Feedback feedback = new Feedback();

            feedback.Response = s.Where(ky => ky.Key == "Response").FirstOrDefault().Value;
            feedback.TaskId = task.TaskId;
            feedback.User = user;
            feedback.Created = DateTime.Now;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = tid });
        }

        /// <summary>
        /// Edit the specified Task .
        /// </summary>
        /// <returns>Edit page</returns>
        /// <param name="id">Task Id.</param>
        public async Task<ActionResult> Edit(int? id)
        {

            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            //Lazy loading to load all the related data to task
            var ss = await _context.Tasks
                     .Include(x => x.CreatedBy)
                     .Include(x => x.AssignedTo)
                     .Include(x => x.Attachment).Where(a => a.TaskId == id).FirstOrDefaultAsync();

            //fill View model
            return View(fillTaskDetailsVM(ss));
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(TaskDetailsViewModel task)
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            if (ModelState.IsValid)
            {
                Boolean result = await UpdateTask(task);
                if (result)
                    return RedirectToAction("Index", "Tasks");
                else return View(task);
            }
            return View(task);
        }

        /// <summary>
        /// Fills the View Model from task
        /// </summary>
        /// <returns>Task ConversationsView Model.</returns>
        /// <param name="ts">Task</param>
        private TaskConversationsViewModel FillTaskConversationVM(Task ts)
        {
            //Populate Data for View Model 
            TaskConversationsViewModel vm = new TaskConversationsViewModel();
            vm.Title = ts.Title;
            vm.TaskId = ts.TaskId;
            vm.AssignedTo = ts.AssignedTo.Username;
            vm.CreatedBy = ts.CreatedBy.Username;
            vm.Priority = ts.Priority;
            vm.DueDate = ts.DueDate;
            //Load Feedback using lazy loading
            vm.Feedbacks = (ICollection<Feedback>)_context.Feedbacks
                            .Include(feedback => feedback.User)
                            .Where(x => x.TaskId == ts.TaskId).ToList();
            return vm;
        }

        /// <summary>
        /// Fills the task details vm.
        /// </summary>
        /// <returns>The task details vm.</returns>
        /// <param name="task">Task.</param>
        private TaskDetailsViewModel fillTaskDetailsVM(Task task)
        {
            //Fill View Model
            TaskDetailsViewModel vm = new TaskDetailsViewModel()
            {
                TaskId = task.TaskId,
                AssignedTo = task.AssignedTo.Username,
                CreatedBy = task.CreatedBy.Username,
                DueDate = task.DueDate,
                Created = task.Created,
                Status = task.Status,
                Priority = task.Priority,
                Severity = task.Severity,
                Title = task.Title,
                AttachmentName = task.Attachment.Filename
            };

            return vm;
        }

        /// <summary>
        /// Creates the task from TaskDetailsViewModel.
        /// </summary>
        /// <param name="task">TaskDetailsViewModel.</param>
        private async System.Threading.Tasks.Task createTask(TaskDetailsViewModel task)
        {
            var s = HttpContext.Session.GetString("Username");
            if (s == null)
            {
                Response.Redirect("/Login");
            }

            //Copy View Model data to new Task Object
            Models.Task newTask = new Models.Task();
            newTask.Created = DateTime.Now;
            newTask.CreatedBy = _context.Users.Where(usr => usr.Username == HttpContext.Session.GetString("Username")).FirstOrDefault();
            newTask.Priority = task.Priority;
            newTask.Status = task.Status;
            newTask.DueDate = task.DueDate;
            newTask.Title = task.Title;
            newTask.Severity = task.Severity;
            newTask.AssignedTo = _context.Users.Where(usr => usr.Username == task.AssignedTo).FirstOrDefault();

            //Create Attachment file if any
            if (task.Files.Count() > 0)
            {
                using (var ms = new MemoryStream())
                {
                    task.Files.FirstOrDefault().CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    Attachment attachment = new Models.Attachment() { file = fileBytes };
                    attachment.Filename = task.Files.FirstOrDefault().FileName;
                    _context.Attachments.Add(attachment);
                    newTask.Attachment = attachment;
                }
            }
            //Create task in DB
            _context.Add(newTask);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the task.
        /// </summary>
        /// <returns>Success or Failure.</returns>
        /// <param name="vm">TaskDetailsViewModel.</param>
        protected async Task<Boolean> UpdateTask(TaskDetailsViewModel vm)
        {
            //Check for error if any
            try
            {
                //Fetch task which needs to be updated
                Task task = _context.Tasks.Where(x => x.TaskId == vm.TaskId).FirstOrDefault();

                task.Priority = vm.Priority;
                task.Status = vm.Status;
                task.DueDate = vm.DueDate;
                task.Title = vm.Title;
                task.Severity = vm.Severity;
                task.AssignedTo = _context.Users.Where(usr => usr.Username == vm.AssignedTo).FirstOrDefault();

                //Check FIle count
                if (vm.Files.Count() > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        vm.Files.FirstOrDefault().CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        Attachment attachment = new Models.Attachment() { file = fileBytes };
                        attachment.Filename = vm.Files.FirstOrDefault().FileName;
                        _context.Attachments.Add(attachment);
                        task.Attachment = attachment;
                    }
                }
                //Update Task
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return false;
            }
        }

        /// <summary>
        /// Delete the specified Task.
        /// </summary>
        /// <returns>Confirm Delete page.</returns>
        /// <param name="id">Task Id.</param>
        // GET: Tasks/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .SingleOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        /// <summary>
        /// Confirms the deletion.
        /// </summary>
        /// <returns>Index page for tasks.</returns>
        /// <param name="id">Task id.</param>
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!AuthenticationHelper.isAuthorizedUser(HttpContext.Session.GetString("Username")))
            {
                Response.Redirect("/Login");
            }

            //Fetch the task to be delted
            Task task = _context.Tasks.Where(x => x.TaskId == id).FirstOrDefault();
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            //Redirect to Tasks page
            return RedirectToAction(nameof(Index));
        }
    }
}