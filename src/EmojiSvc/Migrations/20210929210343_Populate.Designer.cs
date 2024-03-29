﻿// <auto-generated />
using EmojiSvc.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmojiSvc.Migrations
{
    [DbContext(typeof(EmojiDbContext))]
    [Migration("20210929210343_Populate")]
    partial class Populate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("EmojiSvc.Domain.Emoji", b =>
                {
                    b.Property<string>("Shortcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Unicode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Shortcode");

                    b.ToTable("Emojies");
                });
#pragma warning restore 612, 618
        }
    }
}
