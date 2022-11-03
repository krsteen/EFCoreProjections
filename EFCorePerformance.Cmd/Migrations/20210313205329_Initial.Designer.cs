﻿// <auto-generated />
using System;
using EFCoreProjections.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCoreProjections.Cmd.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20210313205329_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ConfigId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Status")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("ReportId");

                    b.HasIndex("ConfigId");

                    b.HasIndex("Name")
                        .HasFilter("[IsArchived] = 0")
                        .IncludeProperties(new[] { "ReportId", "Status" });

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.ReportComment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportComments");
                });

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.ReportConfig", b =>
                {
                    b.Property<int>("ConfigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("VeryUsefulInformation")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConfigId");

                    b.ToTable("ReportConfigs");
                });

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.Report", b =>
                {
                    b.HasOne("EFCorePerformance.Cmd.Model.ReportConfig", "Config")
                        .WithMany("Reports")
                        .HasForeignKey("ConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Config");
                });

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.ReportComment", b =>
                {
                    b.HasOne("EFCorePerformance.Cmd.Model.Report", "Report")
                        .WithMany("Comments")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");
                });

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.Report", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("EFCorePerformance.Cmd.Model.ReportConfig", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
