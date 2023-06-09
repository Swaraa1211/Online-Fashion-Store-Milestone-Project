﻿// <auto-generated />
using FashionStoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FashionStoreAPI.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FashionStoreAPI.Models.ColorModel", b =>
                {
                    b.Property<int>("Color_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Color_Id"));

                    b.Property<string>("Color_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Color_Id");

                    b.ToTable("Color");
                });

            modelBuilder.Entity("FashionStoreAPI.Models.SizeModel", b =>
                {
                    b.Property<int>("Size_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Size_Id"));

                    b.Property<string>("Size_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Size_Id");

                    b.ToTable("Size");
                });
#pragma warning restore 612, 618
        }
    }
}
