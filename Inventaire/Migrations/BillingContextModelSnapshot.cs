﻿// <auto-generated />
using System;
using BillingManagement.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BillingManagement.UI.Migrations
{
    [DbContext(typeof(BillingContext))]
    partial class BillingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("BillingManagement.Models.ContactInfo", b =>
                {
                    b.Property<int>("ContactInfoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Contact")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactType")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ContactInfoID");

                    b.HasIndex("CustomerID");

                    b.ToTable("ContactInfos");
                });

            modelBuilder.Entity("BillingManagement.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactInfo")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PicturePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Province")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BillingManagement.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("SubTotal")
                        .HasColumnType("REAL");

                    b.HasKey("InvoiceId");

                    b.HasIndex("CustomerID");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("BillingManagement.Models.ContactInfo", b =>
                {
                    b.HasOne("BillingManagement.Models.Customer", null)
                        .WithMany("ContactInfos")
                        .HasForeignKey("CustomerID");
                });

            modelBuilder.Entity("BillingManagement.Models.Invoice", b =>
                {
                    b.HasOne("BillingManagement.Models.Customer", "Customer")
                        .WithMany("Invoices")
                        .HasForeignKey("CustomerID");
                });
#pragma warning restore 612, 618
        }
    }
}
