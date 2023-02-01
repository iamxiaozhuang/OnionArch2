﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OnionArch.Infrastructure.Database;

#nullable disable

namespace OnionArch.Infrastructure.Migrations
{
    [DbContext(typeof(OnionArchDb20Context))]
    [Migration("20230201070754_20230201")]
    partial class _20230201
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OnionArch.Domain.Common.Entities.EntityChangedAuditEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CurrentValues")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OccurredBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OriginalValues")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("Id", "TenantId");

                    b.ToTable("EntityChangedAuditLog");
                });

            modelBuilder.Entity("OnionArch.Domain.ProductInventory.ProductInventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("InventoryAmount")
                        .HasColumnType("integer");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("Id", "TenantId");

                    b.ToTable("ProductInventory");
                });
#pragma warning restore 612, 618
        }
    }
}