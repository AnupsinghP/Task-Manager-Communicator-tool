﻿// <auto-generated />
using System;
using CommunicationTool;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CommunicationTool.Migrations
{
    [DbContext(typeof(CommunicationToolContext))]
    partial class CommunicationToolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085");

            modelBuilder.Entity("CommunicationTool.Models.Attachment", b =>
                {
                    b.Property<int>("AttachementId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Filename");

                    b.Property<byte[]>("file")
                        .IsRequired();

                    b.HasKey("AttachementId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("CommunicationTool.Models.Feedback", b =>
                {
                    b.Property<int>("FeedBackId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Response");

                    b.Property<int>("TaskId");

                    b.Property<int?>("UserId");

                    b.HasKey("FeedBackId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("CommunicationTool.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AssignedToId");

                    b.Property<int?>("AttachmentAttachementId");

                    b.Property<DateTime>("Created");

                    b.Property<int?>("CreatedById");

                    b.Property<DateTime>("DueDate");

                    b.Property<int>("Priority");

                    b.Property<int>("Severity");

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("TaskId");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("AttachmentAttachementId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CommunicationTool.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<int>("IsDevTeam");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CommunicationTool.Models.Feedback", b =>
                {
                    b.HasOne("CommunicationTool.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CommunicationTool.Models.Task", b =>
                {
                    b.HasOne("CommunicationTool.Models.User", "AssignedTo")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("AssignedToId");

                    b.HasOne("CommunicationTool.Models.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentAttachementId");

                    b.HasOne("CommunicationTool.Models.User", "CreatedBy")
                        .WithMany("CreatedTasks")
                        .HasForeignKey("CreatedById");
                });
#pragma warning restore 612, 618
        }
    }
}