using System;
using CommunicationTool.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunicationTool
{
    public class CommunicationToolContext : DbContext
    {
        public CommunicationToolContext(DbContextOptions<CommunicationToolContext> options) : base(options) { 

        }

        //Adding DbSet for each entity type that you want to include in your model.
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        //dotnet ef migrations add CreateDatabase
        //dotnet ef database update
    }
}
