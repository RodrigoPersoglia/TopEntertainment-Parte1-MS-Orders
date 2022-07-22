using Microsoft.EntityFrameworkCore;
using TopEntertainment.Ordenes.Domain.Entities;

namespace TopEntertainment.Ordenes.AccessData
{
    public class OrdenesContext : DbContext
    {
        public OrdenesContext() { }

        public OrdenesContext(DbContextOptions<OrdenesContext> options) : base(options) { }
        public virtual DbSet<Compra> Compras { get; set; }
        public virtual DbSet<Carrito> Carritos { get; set; }
        public virtual DbSet<JuegoCarrito> JuegoCarrito { get; set; }
        public virtual DbSet<EstadoDetalle> EstadoDetalles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TopEnterTainmentOrders;Trusted_Connection=True;Integrated Security=True;;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Compra>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImporteFinal)
                .HasMaxLength(50);
                entity.Property(e => e.UsuarioId)
                .HasMaxLength(45);
                entity.Property(e => e.CarritoId)
               .HasMaxLength(45);
                entity.Property(e => e.FechaHora).HasColumnType("date");

                ;
            });



            modelBuilder.Entity<Carrito>(entity =>
                {

                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.UsuarioId).IsRequired().HasMaxLength(45);
                    entity.HasOne(e => e.estado).WithMany(e => e.carritosEstado).HasForeignKey(e => e.EstadoID);
                });


            modelBuilder.Entity<JuegoCarrito>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductoId).IsRequired();
                entity.Property(e => e.Cantidad).IsRequired();
                entity.HasOne(e => e.Carrito).WithMany(e => e.Carritos).HasForeignKey(e => e.CarritoID);

            });

            modelBuilder.Entity<EstadoDetalle>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Estado).IsRequired();
            });

            modelBuilder.Entity<EstadoDetalle>().HasData(
                new EstadoDetalle() { Id = 1 },//, Estado = "Pendiente" },
                new EstadoDetalle() { Id = 2 }//, Estado = "Compra" }
                );



        }







    }
}