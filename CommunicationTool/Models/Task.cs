using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CommunicationTool.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommunicationTool.Models
{
    /// <summary>
    /// Task.
    /// </summary>
    public class Task
    {
        [Required]
        public string Title { get; set; }

        [Key]
        public int TaskId { get; set; }

        //User
        public User CreatedBy { get; set; }

        //User
        public User AssignedTo { get; set; }

        public DateTime Created { get; set; }

        public DateTime DueDate { get; set; }

        //Attachment
        public Attachment Attachment { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public Severity Severity{ get; set; }

        public Status Status { get; set; }


    }
}
