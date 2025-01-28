﻿// <auto-generated />
using Authorize.Scripts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Authorize.Migrations
{
    [DbContext(typeof(AuthorizeContext))]
    partial class AuthorizeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Authorize.DBModels.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Hash")
                        .HasColumnType("char(64)");

                    b.Property<string>("Salt")
                        .HasColumnType("longtext");

                    b.HasKey("Id", "Email");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
