using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationTool.Models
{
    /// <summary>
    /// User.
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        [InverseProperty("CreatedBy")]
        public ICollection<Task> CreatedTasks { get; set; }

        [InverseProperty("AssignedTo")]
        public ICollection<Task> AssignedTasks { get; set; }

        public int IsDevTeam { get; set; }
    }
}
