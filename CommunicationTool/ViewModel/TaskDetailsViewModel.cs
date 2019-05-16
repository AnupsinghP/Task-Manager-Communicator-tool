using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CommunicationTool.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CommunicationTool.ViewModel
{
    /// <summary>
    /// Task details view model.
    /// </summary>
    public class TaskDetailsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommunicationTool.ViewModel.TaskDetailsViewModel"/> class.
        /// </summary>
        public TaskDetailsViewModel()
        {
            //Fill out all the values from enum for combo box

            PriorityList = GetPriortiySelectList();
            SeverityList = GetSeveritySelectList();
            StatusList = GetStatusSelectList();
            Files = new List<IFormFile> ();
        }


        public string Title { get; set; }

        public int TaskId { get; set; }

        public string CreatedBy { get; set; }

        [Required]
        public string AssignedTo { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public Severity Severity { get; set; }

        public Status Status { get; set; }

        public List<IFormFile> Files { get; set; }

        public string AttachmentName { get; set; }

        public SelectList PriorityList { get; set; }

        public SelectList Assigned { get; set; }

        /// <summary>
        /// Gets the priortiy select list from Priority enum.
        /// </summary>
        /// <returns>The priortiy select list.</returns>
        public static SelectList GetPriortiySelectList()
        {
            var enumValues = Enum.GetValues(typeof(Priority)).Cast<Priority>().Select(e => new { Value = e.ToString(), Text = e.ToString() }).ToList();

            return new SelectList(enumValues, "Value", "Text", "");
        }

        public SelectList SeverityList { get; set; }

        /// <summary>
        /// Gets the severity select list from Severity enum.
        /// </summary>
        /// <returns>The severity select list.</returns>
        public static SelectList GetSeveritySelectList()
        {
            var enumValues = Enum.GetValues(typeof(Severity)).Cast<Severity>().Select(e => new { Value = e.ToString(), Text = e.ToString() }).ToList();

            return new SelectList(enumValues, "Value", "Text", "");
        }

       
        public SelectList StatusList { get; set; }

        /// <summary>
        /// Gets the status select list from status enum.
        /// </summary>
        /// <returns>The status select list.</returns>
        public static SelectList GetStatusSelectList()
        {
            var enumValues = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Value = e.ToString(), Text = e.ToString() }).ToList();

            return new SelectList(enumValues, "Value", "Text", "");
        }
    }
}
