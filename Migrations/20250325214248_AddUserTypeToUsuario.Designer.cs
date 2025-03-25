﻿// <auto-generated />
using System;
using Api.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(AmadeusContext))]
    [Migration("20250325214248_AddUserTypeToUsuario")]
    partial class AddUserTypeToUsuario
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Api.Models.Continente", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("descripcion");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nombre");

                    b.HasKey("Id")
                        .HasName("continentes_pkey");

                    b.HasIndex(new[] { "Nombre" }, "ukk8ce7fss8cy9megssacnggnts")
                        .IsUnique();

                    b.ToTable("continentes", (string)null);
                });

            modelBuilder.Entity("Api.Models.Destino", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("ComidaTipica")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("comida_tipica");

                    b.Property<long?>("ContinentesId")
                        .HasColumnType("bigint")
                        .HasColumnName("continentes_id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Idioma")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("idioma");

                    b.Property<string>("ImgUrl")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("img_url");

                    b.Property<string>("LugarImperdible")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("lugar_imperdible");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nombre");

                    b.Property<string>("Pais")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("pais");

                    b.HasKey("Id")
                        .HasName("destinos_pkey");

                    b.HasIndex("ContinentesId");

                    b.HasIndex(new[] { "Nombre" }, "uklffrnfiyqdj9vbjl1h02g6q3b")
                        .IsUnique();

                    b.ToTable("destinos", (string)null);
                });

            modelBuilder.Entity("Api.Models.DestinosPreferencia", b =>
                {
                    b.Property<long>("DestinosId")
                        .HasColumnType("bigint")
                        .HasColumnName("destinos_id");

                    b.Property<long>("PreferenciasId")
                        .HasColumnType("bigint")
                        .HasColumnName("preferencias_id");

                    b.HasIndex("DestinosId");

                    b.HasIndex(new[] { "PreferenciasId", "DestinosId" }, "ukbe1ma940t4i053op4ikiqk4sy")
                        .IsUnique();

                    b.ToTable("destinos_preferencias", (string)null);
                });

            modelBuilder.Entity("Api.Models.Preferencia", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Actividad")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("actividad");

                    b.Property<string>("Alojamiento")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("alojamiento");

                    b.Property<string>("Clima")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("clima");

                    b.Property<string>("Entorno")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("entorno");

                    b.Property<string>("RangoEdad")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("rango_edad");

                    b.Property<string>("TiempoViaje")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("tiempo_viaje");

                    b.HasKey("Id")
                        .HasName("preferencias_pkey");

                    b.ToTable("preferencias", (string)null);
                });

            modelBuilder.Entity("Api.Models.PreferenciaUsuario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("PreferenciasId")
                        .HasColumnType("bigint")
                        .HasColumnName("preferencias_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("updated_at");

                    b.Property<long?>("UsuariosId")
                        .HasColumnType("bigint")
                        .HasColumnName("usuarios_id");

                    b.HasKey("Id")
                        .HasName("preferencia_usuarios_pkey");

                    b.HasIndex("PreferenciasId");

                    b.HasIndex("UsuariosId");

                    b.ToTable("preferencia_usuarios", (string)null);
                });

            modelBuilder.Entity("Api.Models.Usuario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nombre");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasDefaultValue("CLIENT")
                        .HasColumnName("user_type");

                    b.HasKey("Id")
                        .HasName("usuarios_pkey");

                    b.HasIndex(new[] { "Email" }, "ukkfsp0s1tflm1cwlj8idhqsad0")
                        .IsUnique();

                    b.ToTable("usuarios", (string)null);
                });

            modelBuilder.Entity("Api.Models.Destino", b =>
                {
                    b.HasOne("Api.Models.Continente", "Continentes")
                        .WithMany("Destinos")
                        .HasForeignKey("ContinentesId")
                        .HasConstraintName("fkrk7e7gdcc8yoku5eo69q1tsfx");

                    b.Navigation("Continentes");
                });

            modelBuilder.Entity("Api.Models.DestinosPreferencia", b =>
                {
                    b.HasOne("Api.Models.Destino", "Destinos")
                        .WithMany()
                        .HasForeignKey("DestinosId")
                        .IsRequired()
                        .HasConstraintName("fk3lwk5ehbhgvvcl721y9xasj9i");

                    b.HasOne("Api.Models.Preferencia", "Preferencias")
                        .WithMany()
                        .HasForeignKey("PreferenciasId")
                        .IsRequired()
                        .HasConstraintName("fkn97i9vn5nbx806tseoajw69um");

                    b.Navigation("Destinos");

                    b.Navigation("Preferencias");
                });

            modelBuilder.Entity("Api.Models.PreferenciaUsuario", b =>
                {
                    b.HasOne("Api.Models.Preferencia", "Preferencias")
                        .WithMany("PreferenciaUsuarios")
                        .HasForeignKey("PreferenciasId")
                        .HasConstraintName("fk3fdxcfsotuv8gji3v0lg9aya3");

                    b.HasOne("Api.Models.Usuario", "Usuarios")
                        .WithMany("PreferenciaUsuarios")
                        .HasForeignKey("UsuariosId")
                        .HasConstraintName("fk3hxwg7yibg01euk8c0wxy93qa");

                    b.Navigation("Preferencias");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Api.Models.Continente", b =>
                {
                    b.Navigation("Destinos");
                });

            modelBuilder.Entity("Api.Models.Preferencia", b =>
                {
                    b.Navigation("PreferenciaUsuarios");
                });

            modelBuilder.Entity("Api.Models.Usuario", b =>
                {
                    b.Navigation("PreferenciaUsuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
