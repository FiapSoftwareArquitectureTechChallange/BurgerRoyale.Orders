﻿// <auto-generated />
using System;
using BurgerRoyale.Orders.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BurgerRoyale.Orders.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231219205928_AddedOrderNumber")]
    partial class AddedOrderNumber
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CloseTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.OrderProduct", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ProductPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f132dd38-d80d-4fd8-9470-191b8d6e23f0"),
                            Category = 0,
                            Description = "Hambúrguer de carne bovina.",
                            Name = "Burger Tradicional",
                            Price = 19.2m
                        },
                        new
                        {
                            Id = new Guid("c56a7e1c-56ae-4d7c-9eb3-14ff83934320"),
                            Category = 0,
                            Description = "Hambúrguer de carne bovina com o dobro de bacon.",
                            Name = "Burger Duplo Bacon",
                            Price = 22.9m
                        },
                        new
                        {
                            Id = new Guid("8001cdcc-dd10-4977-b647-4523742378ec"),
                            Category = 0,
                            Description = "Hambúrguer de carne bovina com o dobro de cheddar.",
                            Name = "Burger Duplo Cheddar",
                            Price = 23.9m
                        },
                        new
                        {
                            Id = new Guid("bfc38a0f-414d-4e89-b76c-eb561433e826"),
                            Category = 1,
                            Description = "Porção de fritas pequena.",
                            Name = "Fritas Pequena",
                            Price = 4.9m
                        },
                        new
                        {
                            Id = new Guid("68dcd0c8-43a9-4084-b460-3e07d5fd6e58"),
                            Category = 1,
                            Description = "Porção de fritas.",
                            Name = "Fritas",
                            Price = 6.9m
                        },
                        new
                        {
                            Id = new Guid("24b2b893-e3dc-4f7d-b70f-4daa6fbae88d"),
                            Category = 1,
                            Description = "Porção de fritas grande.",
                            Name = "Fritas Grande",
                            Price = 8.9m
                        },
                        new
                        {
                            Id = new Guid("3ab44238-ea8c-4a2d-b842-2965a0be4429"),
                            Category = 2,
                            Description = "500 ml com ou sem gás",
                            Name = "Água",
                            Price = 4m
                        },
                        new
                        {
                            Id = new Guid("a0e7d06f-3555-4643-a5ed-f6d698dced62"),
                            Category = 2,
                            Description = "Copo 400 ml",
                            Name = "Refrigerante",
                            Price = 6m
                        },
                        new
                        {
                            Id = new Guid("a9a2ffb7-d61a-44a3-8854-fcb331c19265"),
                            Category = 3,
                            Description = "Sundae de diversos sabores",
                            Name = "Sundae",
                            Price = 7m
                        },
                        new
                        {
                            Id = new Guid("5b49e5dd-7f2e-4aad-ada7-cb066f305c05"),
                            Category = 3,
                            Description = "Sorvete de diversos sabores",
                            Name = "Sorvete",
                            Price = 7m
                        });
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.ProductImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.OrderProduct", b =>
                {
                    b.HasOne("BurgerRoyale.Orders.Domain.Entities.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BurgerRoyale.Orders.Domain.Entities.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.ProductImage", b =>
                {
                    b.HasOne("BurgerRoyale.Orders.Domain.Entities.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("BurgerRoyale.Orders.Domain.Entities.Product", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("OrderProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
