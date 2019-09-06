namespace pln_v2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class plnContext : DbContext
    {
        public plnContext()
            : base("name=plnContext")
        {
        }

        public virtual DbSet<ESTADOS_PROY> ESTADOS_PROY { get; set; }
        public virtual DbSet<FACTOR> FACTOR { get; set; }
        public virtual DbSet<PROYECTO> PROYECTO { get; set; }
        public virtual DbSet<PROYECTO_FAVORITOS> PROYECTO_FAVORITOS { get; set; }
        public virtual DbSet<PROYECTO_FINAL> PROYECTO_FINAL { get; set; }
        public virtual DbSet<PROYECTO_VISTA> PROYECTO_VISTA { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ESTADOS_PROY>()
                .Property(e => e.ID_ESTADO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<ESTADOS_PROY>()
                .Property(e => e.NOM_ESTADO)
                .IsUnicode(false);
            
            modelBuilder.Entity<FACTOR>()
                .Property(e => e.ID_FACTOR);

            modelBuilder.Entity<FACTOR>()
                .Property(e => e.NOM_FACTOR)
                .IsUnicode(false);

            modelBuilder.Entity<FACTOR>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<FACTOR>()
                .Property(e => e.IMG)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.ID_PROYECTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.ID_FACTOR)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.NOM_PROYECTO)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.COSTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.REQUISITOS)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.IMG)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.ESTADO);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.RESPONSABLE)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO>()
                .Property(e => e.OBJETIVO)
                .IsUnicode(false);

            
            modelBuilder.Entity<PROYECTO_FAVORITOS>()
                .Property(e => e.ID_FAVORITO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_FAVORITOS>()
                .Property(e => e.ID_PROYECTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_FAVORITOS>()
                .Property(e => e.ESTADO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO_FAVORITOS>()
                .Property(e => e.USUARIOVOTO)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO_FINAL>()
                .Property(e => e.ID_PROYECTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_FINAL>()
                .Property(e => e.ID_FINAL)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_FINAL>()
                .Property(e => e.NOTA_PROYECTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_FINAL>()
                .Property(e => e.PUESTO_PROYECTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_FINAL>()
                .Property(e => e.RESPONSABLE)
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO_FINAL>()
                .Property(e => e.APROBADO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROYECTO_VISTA>()
                .Property(e => e.ID_VISTA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_VISTA>()
                .Property(e => e.ID_PROYECTO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PROYECTO_VISTA>()
                .Property(e => e.ESTADO)
                .IsFixedLength()
                .IsUnicode(false);
            
            modelBuilder.Entity<USUARIO>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.USUARIO1)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.CLAVE)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.NOMBRES)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.TIPO_USUARIO)
                .IsUnicode(false);

        }
    }
}
