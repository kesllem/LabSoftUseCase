using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace appReversotask.Models;

public partial class DbclinicaContext : DbContext
{
    public DbclinicaContext()
    {
    }

    public DbclinicaContext(DbContextOptions<DbclinicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consultum> Consulta { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Pacientes> Pacientes1 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consultum>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.StatusConsulta)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Medico).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Medico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_paciente");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Medico");

            entity.Property(e => e.Crm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CRM");
            entity.Property(e => e.Especialidade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("paciente");

            entity.Property(e => e.Cpf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(14)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pacientes>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pacientes");

            entity.Property(e => e.Cpf)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DataNascimento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
