﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPSPS.Data;

namespace PPSPS.Migrations
{
    [DbContext(typeof(AuthDBContext))]
    partial class AuthDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(767)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(767)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(767)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(767)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PPSPS.Areas.Identity.Data.PPSPSUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ClassId")
                        .HasColumnType("nvarchar(767)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GroupId")
                        .HasColumnType("nvarchar(767)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ClassId")
                        .IsUnique();

                    b.HasIndex("GroupId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("FileId")
                        .HasColumnType("varchar(767)");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("TaskId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(767)");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("TaskId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSClass", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("ClassName")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("ClassTeacherId")
                        .HasColumnType("varchar(767)");

                    b.Property<int>("YearOfEntry")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassTeacherId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<DateTime>("DateSubmission")
                        .HasColumnType("datetime");

                    b.Property<string>("Extension")
                        .HasColumnType("varchar(10)");

                    b.Property<byte[]>("File")
                        .HasColumnType("longblob");

                    b.Property<string>("FileName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FileType")
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSGroup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("GroupAbbreviation")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("GroupName")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSRoles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("RoleId")
                        .HasColumnType("VARCHAR(256)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSSubject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("SubjectAbbreviation")
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5);

                    b.Property<string>("SubjectName")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSTask", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("ClassId")
                        .HasColumnType("varchar(767)");

                    b.Property<DateTime>("DateDeadline")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DateEntered")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("GroupId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("SubjectId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("TaskName")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("TeacherId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("YearsOfStudiesId")
                        .HasColumnType("varchar(767)");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.HasIndex("YearsOfStudiesId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSUserView", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("aspnetusers","ppsps");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSYearsOfStudies", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<int>("FirstSemester")
                        .HasColumnType("int");

                    b.Property<int>("SecondSemester")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("YearsOfStudies");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PPSPS.Areas.Identity.Data.PPSPSUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PPSPS.Areas.Identity.Data.PPSPSUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPSPS.Areas.Identity.Data.PPSPSUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PPSPS.Areas.Identity.Data.PPSPSUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PPSPS.Areas.Identity.Data.PPSPSUser", b =>
                {
                    b.HasOne("PPSPS.Models.PPSPSClass", "Class")
                        .WithOne("User")
                        .HasForeignKey("PPSPS.Areas.Identity.Data.PPSPSUser", "ClassId");

                    b.HasOne("PPSPS.Models.PPSPSGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSAssignment", b =>
                {
                    b.HasOne("PPSPS.Models.PPSPSFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("PPSPS.Models.PPSPSTask", "Task")
                        .WithOne("Assignment")
                        .HasForeignKey("PPSPS.Models.PPSPSAssignment", "TaskId");

                    b.HasOne("PPSPS.Areas.Identity.Data.PPSPSUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSClass", b =>
                {
                    b.HasOne("PPSPS.Models.PPSPSUserView", "ClassTeacher")
                        .WithMany()
                        .HasForeignKey("ClassTeacherId");
                });

            modelBuilder.Entity("PPSPS.Models.PPSPSTask", b =>
                {
                    b.HasOne("PPSPS.Models.PPSPSClass", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");

                    b.HasOne("PPSPS.Models.PPSPSGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("PPSPS.Models.PPSPSSubject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.HasOne("PPSPS.Areas.Identity.Data.PPSPSUser", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");

                    b.HasOne("PPSPS.Models.PPSPSYearsOfStudies", "YearsOfStudies")
                        .WithMany()
                        .HasForeignKey("YearsOfStudiesId");
                });
#pragma warning restore 612, 618
        }
    }
}
