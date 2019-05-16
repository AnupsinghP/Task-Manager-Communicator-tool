using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationTool.Models
{
    /// <summary>
    /// Attachment Model
    /// </summary>
    public class Attachment
    {
        [Required]
        [Key]
        public int AttachementId { get; set; }

        [Required]
        public byte[] file { get; set; }

        public string Filename { get; set; }

        public DateTime Created { get; set; }
    }
}
