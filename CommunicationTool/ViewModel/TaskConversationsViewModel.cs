using System;
using System.Collections.Generic;
using CommunicationTool.Models;
using CommunicationTool.Resources;

namespace CommunicationTool.ViewModel
{
    /// <summary>
    /// Task conversations view model.
    /// </summary>
    public class TaskConversationsViewModel
    {
        public TaskConversationsViewModel()
        {
        }

        public int TaskId;

        public string Title { get; set; }

        public string CreatedBy { get; set; }

        public string AssignedTo { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime DueDate { get; set; }

        //List for feed back for particular topic
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
