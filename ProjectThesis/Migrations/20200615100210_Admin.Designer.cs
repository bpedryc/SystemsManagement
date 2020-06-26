﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Migrations
{
    [DbContext(typeof(ThesisDbContext))]
    [Migration("20200615100210_Admin")]
    partial class Admin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("ProjectThesis.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ProjectThesis.Models.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("ProjectThesis.Models.Specialty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FacId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("FacId");

                    b.ToTable("Specials");
                });

            modelBuilder.Entity("ProjectThesis.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DegreeCycle");

                    b.Property<int>("SpecialtyId");

                    b.Property<int>("StudentNo");

                    b.Property<int?>("SupervisorId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SpecialtyId");

                    b.HasIndex("SupervisorId");

                    b.HasIndex("UserId");

                    b.ToTable("Studs");
                });

            modelBuilder.Entity("ProjectThesis.Models.Supervisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FacultyId");

                    b.Property<int>("StudentLimit");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("UserId");

                    b.ToTable("Supers");
                });

            modelBuilder.Entity("ProjectThesis.Models.Thesis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DegreeCycle");

                    b.Property<int>("SpecId");

                    b.Property<int?>("StudentId");

                    b.Property<string>("Subject");

                    b.Property<int>("SuperId");

                    b.HasKey("Id");

                    b.HasIndex("SpecId");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.HasIndex("SuperId");

                    b.ToTable("Theses");
                });

            modelBuilder.Entity("ProjectThesis.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectThesis.Models.Admin", b =>
                {
                    b.HasOne("ProjectThesis.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectThesis.Models.Specialty", b =>
                {
                    b.HasOne("ProjectThesis.Models.Faculty", "Fac")
                        .WithMany()
                        .HasForeignKey("FacId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectThesis.Models.Student", b =>
                {
                    b.HasOne("ProjectThesis.Models.Specialty", "Specialty")
                        .WithMany()
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectThesis.Models.Supervisor")
                        .WithMany("Students")
                        .HasForeignKey("SupervisorId");

                    b.HasOne("ProjectThesis.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectThesis.Models.Supervisor", b =>
                {
                    b.HasOne("ProjectThesis.Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectThesis.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectThesis.Models.Thesis", b =>
                {
                    b.HasOne("ProjectThesis.Models.Specialty", "Spec")
                        .WithMany()
                        .HasForeignKey("SpecId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectThesis.Models.Student", "Student")
                        .WithOne("ChosenThesis")
                        .HasForeignKey("ProjectThesis.Models.Thesis", "StudentId");

                    b.HasOne("ProjectThesis.Models.Supervisor", "Super")
                        .WithMany("Theses")
                        .HasForeignKey("SuperId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
