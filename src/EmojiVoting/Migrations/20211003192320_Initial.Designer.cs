﻿// <auto-generated />
using EmojiVoting.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmojiVoting.Migrations
{
    [DbContext(typeof(VotingDbContext))]
    [Migration("20211003192320_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("EmojiVoting.Domain.Result", b =>
                {
                    b.Property<string>("Shortcode")
                        .HasColumnType("TEXT");

                    b.Property<int>("Votes")
                        .HasColumnType("INTEGER");

                    b.HasKey("Shortcode");

                    b.ToTable("Results");
                });
#pragma warning restore 612, 618
        }
    }
}
