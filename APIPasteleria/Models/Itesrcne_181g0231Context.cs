using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace APIPasteleria.Models
{
    public partial class Itesrcne_181g0231Context : DbContext
    {
        public Itesrcne_181g0231Context()
        {
        }

        public Itesrcne_181g0231Context(DbContextOptions<Itesrcne_181g0231Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Partido> Partido { get; set; }
        public virtual DbSet<Pasteles> Pasteles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Partido>(entity =>
            {
                entity.ToTable("partido");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DescripcionPartido)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.Equipos)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EstadoPartido)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FechaPartido).HasColumnType("date");

                entity.Property(e => e.Goles)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Minuto)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Pasteles>(entity =>
            {
                entity.ToTable("pasteles");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Costo).HasPrecision(10, 2);

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.FechaVenta).HasColumnType("date");

                entity.Property(e => e.NombrePastel)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Sucursal)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
