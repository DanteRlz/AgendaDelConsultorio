using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AgendaDelConsultorio.Models;

namespace AgendaDelConsultorio.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Especialidad> Especialidades { get; set; } = null!;
        public virtual DbSet<estadosturno> EstadosTurnos { get; set; } = null!;
        public virtual DbSet<Localidad> Localidades { get; set; } = null!;
        public virtual DbSet<paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Profesional> Profesionales { get; set; } = null!;
        public virtual DbSet<provincia> Provincias { get; set; } = null!;
        public virtual DbSet<tiposespecialidad> TiposEspecialidades { get; set; } = null!;
        public virtual DbSet<turno> Turnos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.EspecialidadId)
                    .HasName("PK_especialidades_EspecialidadId");

                entity.ToTable("especialidades", "agendadeconsultorio");

                entity.HasIndex(e => e.TipoEspecialidadId, "FK_Especialidades_TiposEspecialidad");

                entity.HasIndex(e => e.Descripcion, "especialidades$Descripcion")
                    .IsUnique();

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 0)");

                entity.HasOne(d => d.TipoEspecialidad)
                    .WithMany(p => p.especialidades)
                    .HasForeignKey(d => d.TipoEspecialidadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("especialidades$FK_Especialidades_TiposEspecialidad");
            });

            modelBuilder.Entity<estadosturno>(entity =>
            {
                entity.HasKey(e => e.EstadoTurnoId)
                    .HasName("PK_estadosturno_EstadoTurnoId");

                entity.ToTable("estadosturno", "agendadeconsultorio");

                entity.HasIndex(e => e.Descripcion, "estadosturno$Descripcion")
                    .IsUnique();

                entity.Property(e => e.Descripcion).HasMaxLength(30);
            });

            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.HasKey(e => e.LocalidadId)
                    .HasName("PK_localidades_LocalidadId");

                entity.ToTable("localidades", "agendadeconsultorio");

                entity.HasIndex(e => e.ProvinciaId, "FK_Localidades_Provincias");

                entity.Property(e => e.Descripcion).HasMaxLength(255);

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.localidades)
                    .HasForeignKey(d => d.ProvinciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("localidades$FK_Localidades_Provincias");
            });

            modelBuilder.Entity<paciente>(entity =>
            {
                entity.ToTable("pacientes", "agendadeconsultorio");

                entity.HasIndex(e => e.LocalidadId, "FK_Pacientes_Localidades");

                entity.HasIndex(e => e.ProvinciaId, "FK_Pacientes_Provincias");

                entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "pacientes$UQ_Pacientes_TipoDoc_NumDoc")
                    .IsUnique();

                entity.Property(e => e.Apellido).HasMaxLength(50);

                entity.Property(e => e.Calle).HasMaxLength(50);

                entity.Property(e => e.CorreoElectronico).HasMaxLength(50);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Telefono).HasMaxLength(30);

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.HasOne(d => d.Localidad)
                    .WithMany(p => p.pacientes)
                    .HasForeignKey(d => d.LocalidadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pacientes$FK_Pacientes_Localidades");

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.pacientes)
                    .HasForeignKey(d => d.ProvinciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pacientes$FK_Pacientes_Provincias");
            });

            modelBuilder.Entity<Profesional>(entity =>
            {
                entity.HasKey(e => e.ProfesionalId)
                    .HasName("PK_profesionales_ProfesionalId");

                entity.ToTable("profesionales", "agendadeconsultorio");

                entity.HasIndex(e => e.LocalidadId, "FK_Profesionales_Localidades");

                entity.HasIndex(e => e.ProvinciaId, "FK_Profesionales_Provincias");

                entity.Property(e => e.Apellido).HasMaxLength(50);

                entity.Property(e => e.Calle).HasMaxLength(50);

                entity.Property(e => e.CorreoElectronico).HasMaxLength(50);

                entity.Property(e => e.Especialidad).HasMaxLength(50);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Telefono).HasMaxLength(30);

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.HasOne(d => d.Localidad)
                    .WithMany(p => p.profesionales)
                    .HasForeignKey(d => d.LocalidadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesionales$FK_Profesionales_Localidades");

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.profesionales)
                    .HasForeignKey(d => d.ProvinciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesionales$FK_Profesionales_Provincias");
            });

            modelBuilder.Entity<provincia>(entity =>
            {
                entity.ToTable("provincias", "agendadeconsultorio");

                entity.Property(e => e.Descripcion).HasMaxLength(255);
            });

            modelBuilder.Entity<tiposespecialidad>(entity =>
            {
                entity.HasKey(e => e.TipoEspecialidadId)
                    .HasName("PK_tiposespecialidad_TipoEspecialidadId");

                entity.ToTable("tiposespecialidad", "agendadeconsultorio");

                entity.Property(e => e.Descripcion).HasMaxLength(100);
            });

            modelBuilder.Entity<turno>(entity =>
            {
                entity.ToTable("turnos", "agendadeconsultorio");

                entity.HasIndex(e => e.EspecialidadId, "FK_Turnos_Especialidades");

                entity.HasIndex(e => e.EstadoTurnoId, "FK_Turnos_EstadosTurno");

                entity.HasIndex(e => e.PacienteId, "FK_Turnos_Pacientes");

                entity.Property(e => e.FechaTurno).HasColumnType("date");

                entity.HasOne(d => d.Especialidad)
                    .WithMany(p => p.turnos)
                    .HasForeignKey(d => d.EspecialidadId)
                    .HasConstraintName("turnos$FK_Turnos_Especialidades");

                entity.HasOne(d => d.EstadoTurno)
                    .WithMany(p => p.turnos)
                    .HasForeignKey(d => d.EstadoTurnoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("turnos$FK_Turnos_EstadosTurno");

                entity.HasOne(d => d.Paciente)
                    .WithMany(p => p.turnos)
                    .HasForeignKey(d => d.PacienteId)
                    .HasConstraintName("turnos$FK_Turnos_Pacientes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
