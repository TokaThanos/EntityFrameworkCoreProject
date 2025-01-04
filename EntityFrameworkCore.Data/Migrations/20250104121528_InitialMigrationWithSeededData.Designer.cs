﻿// <auto-generated />
using System;
using EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFrameworkCore.Data.Migrations
{
    [DbContext(typeof(FootballLeagueDbContext))]
    [Migration("20250104121528_InitialMigrationWithSeededData")]
    partial class InitialMigrationWithSeededData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("EntityFrameworkCore.Domain.Coach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("EntityFrameworkCore.Domain.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2025, 1, 4, 12, 15, 27, 995, DateTimeKind.Unspecified).AddTicks(281),
                            Name = "Manchester United"
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(2025, 1, 4, 12, 15, 27, 995, DateTimeKind.Unspecified).AddTicks(1173),
                            Name = "F.C. Barcelona"
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(2025, 1, 4, 12, 15, 27, 995, DateTimeKind.Unspecified).AddTicks(1180),
                            Name = "Juventus"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
