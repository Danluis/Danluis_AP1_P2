using Aplicada1.Core;
using Danluis_AP1_P2.Context;
using Danluis_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Danluis_AP1_P2.Services;

public class VentaService : IServiceResult<Venta, int>
{
    private readonly IDbContextFactory<Contexto> _factory;

    public VentaService(IDbContextFactory<Contexto> factory)
    {
        _factory = factory;
    }

    public async Task<Result<List<Venta>>> GetList(Expression<Func<Venta, bool>> filtro)
    {
        using var context = await _factory.CreateDbContextAsync();
        var lista = await context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(filtro)
            .ToListAsync();
        return Result.Success(lista);
    }

    public async Task<Result<Venta?>> Buscar(int id)
    {
        using var context = await _factory.CreateDbContextAsync();
        var resultado = await context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(x => x.VentaId == id);
        return Result.Success(resultado);
    }

    public async Task<Result> Guardar(Venta venta)
    {
        try
        {
            if (venta.Detalles == null || venta.Detalles.Count == 0)
            {
                return Result.Failure(new Error("SinDetalle", "Debe agregar al menos un producto"));
            }

            using var context = await _factory.CreateDbContextAsync();

            decimal nuevoTotal = venta.Detalles.Sum(d => d.Cantidad * d.Precio);

            if (venta.VentaId == 0)
            {
                // Se resta stock de cada producto vendido
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await context.Productos.FirstOrDefaultAsync(p => p.ProductoId == detalle.ProductoId);
                    if (producto == null)
                        return Result.Failure(new Error("ProductoNoExiste", "Uno de los productos seleccionados no existe"));
                    if (producto.Stock < detalle.Cantidad)
                        return Result.Failure(new Error("StockInsuficiente", $"Stock insuficiente para {producto.Nombre}"));
                    producto.Stock -= detalle.Cantidad;
                }

                venta.Total = nuevoTotal;
                context.Ventas.Add(venta);
            }
            else
            {
                var existente = await context.Ventas
                    .Include(v => v.Detalles)
                    .FirstOrDefaultAsync(v => v.VentaId == venta.VentaId);

                if (existente == null)
                {
                    return Result.Failure(new Error("NoEncontrado", "La venta no existe"));
                }

                // Reviertir el stock de los detalles viejos
                foreach (var detalleViejo in existente.Detalles)
                {
                    var productoViejo = await context.Productos.FirstOrDefaultAsync(p => p.ProductoId == detalleViejo.ProductoId);
                    if (productoViejo != null)
                        productoViejo.Stock += detalleViejo.Cantidad;
                }

                // Aplicar stock de los detalles nuevos
                foreach (var detalleNuevo in venta.Detalles)
                {
                    var productoNuevo = await context.Productos.FirstOrDefaultAsync(p => p.ProductoId == detalleNuevo.ProductoId);
                    if (productoNuevo == null)
                        return Result.Failure(new Error("ProductoNoExiste", "Uno de los productos seleccionados no existe"));
                    if (productoNuevo.Stock < detalleNuevo.Cantidad)
                        return Result.Failure(new Error("StockInsuficiente", $"Stock insuficiente para {productoNuevo.Nombre}"));
                    productoNuevo.Stock -= detalleNuevo.Cantidad;

                    detalleNuevo.DetalleId = 0;
                    detalleNuevo.Producto = null;
                    detalleNuevo.Venta = null;
                }

                context.DetalleVentas.RemoveRange(existente.Detalles);

                existente.Fecha = venta.Fecha;
                existente.Observacion = venta.Observacion;
                existente.Total = nuevoTotal;
                existente.Detalles = venta.Detalles;
            }

            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("ErrorGuardar", ex.Message));
        }
    }

    public async Task<Result> Eliminar(int id)
    {
        try
        {
            using var context = await _factory.CreateDbContextAsync();
            var venta = await context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(x => x.VentaId == id);

            if (venta == null)
                return Result.Failure(new Error("NoEncontrado", "La venta no existe"));

            // Devolver stock al eliminar
            foreach (var detalle in venta.Detalles)
            {
                var producto = await context.Productos.FirstOrDefaultAsync(p => p.ProductoId == detalle.ProductoId);
                if (producto != null)
                    producto.Stock += detalle.Cantidad;
            }

            context.Ventas.Remove(venta);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("ErrorEliminar", ex.Message));
        }
    }
}
