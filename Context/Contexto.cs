using Danluis_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;

namespace Danluis_AP1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetalleVentas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Producto>().HasData(
            new Producto { ProductoId = 1, Nombre = "Frappuccino Mocha", Precio = 120, Stock = 10 },
            new Producto { ProductoId = 2, Nombre = "Caramel Macchiato", Precio = 130, Stock = 10 },
            new Producto { ProductoId = 3, Nombre = "Vanilla Latte", Precio = 110, Stock = 10 }
        );
    }
}
