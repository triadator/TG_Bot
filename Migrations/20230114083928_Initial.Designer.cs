﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBot.Models;

#nullable disable

namespace TelegramBot.Migrations
{
    [DbContext(typeof(BotContext))]
    [Migration("20230114083928_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TelegramBot.Models.DbSets.CashReceipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("CashReceipts");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.ClientSizeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("DressSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OverDressSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("T_ShortSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrousersSize")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("ClientSizeInfo");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Barcode")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Leftover")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.CashReceipt", b =>
                {
                    b.HasOne("TelegramBot.Models.DbSets.Order", null)
                        .WithOne("CashReceipt")
                        .HasForeignKey("TelegramBot.Models.DbSets.CashReceipt", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.ClientSizeInfo", b =>
                {
                    b.HasOne("TelegramBot.Models.DbSets.Client", null)
                        .WithOne("SizeInfo")
                        .HasForeignKey("TelegramBot.Models.DbSets.ClientSizeInfo", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Item", b =>
                {
                    b.HasOne("TelegramBot.Models.DbSets.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId");

                    b.HasOne("TelegramBot.Models.DbSets.Client", "Owner")
                        .WithMany("Wardrobe")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Order");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Order", b =>
                {
                    b.HasOne("TelegramBot.Models.DbSets.Client", "ClientName")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId");

                    b.Navigation("ClientName");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Client", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("SizeInfo")
                        .IsRequired();

                    b.Navigation("Wardrobe");
                });

            modelBuilder.Entity("TelegramBot.Models.DbSets.Order", b =>
                {
                    b.Navigation("CashReceipt");

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
