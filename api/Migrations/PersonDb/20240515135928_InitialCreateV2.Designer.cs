﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StargateAPI.Business.Data;

#nullable disable

namespace StargateAPI.Migrations.PersonDb
{
    [DbContext(typeof(PersonDbContext))]
    [Migration("20240515135928_InitialCreateV2")]
    partial class InitialCreateV2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("StargateAPI.Business.Data.AstronautDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CareerEndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CareerStartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentDutyTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentRank")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("AstronautDetail");
                });

            modelBuilder.Entity("StargateAPI.Business.Data.AstronautDuty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DutyEndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DutyStartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("DutyTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Rank")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("AstronautDuty");
                });

            modelBuilder.Entity("StargateAPI.Business.Data.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("StargateAPI.Business.Data.AstronautDetail", b =>
                {
                    b.HasOne("StargateAPI.Business.Data.Person", "Person")
                        .WithOne("AstronautDetail")
                        .HasForeignKey("StargateAPI.Business.Data.AstronautDetail", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("StargateAPI.Business.Data.AstronautDuty", b =>
                {
                    b.HasOne("StargateAPI.Business.Data.Person", "Person")
                        .WithMany("AstronautDuties")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("StargateAPI.Business.Data.Person", b =>
                {
                    b.Navigation("AstronautDetail");

                    b.Navigation("AstronautDuties");
                });
#pragma warning restore 612, 618
        }
    }
}
