using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationTool.Models
{
    /// <summary>
    /// Feedback.
    /// </summary>
    public class Feedback
    {
        [Required]
        [Key]
        public int FeedBackId { get; set; }

        public string Response { get; set; }

        public DateTime Created { get; set; }

        //TaskId
        [Required]
        public int TaskId { get; set; }

        //Create by
        public User User{ get; set; }

       



    }
}
