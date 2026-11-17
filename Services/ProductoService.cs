using Aplicada1.Core;
using Danluis_AP1_P2.Context;
using Danluis_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Danluis_AP1_P2.Services;

public class ProductoService : IServiceResult<Producto, int>
{
    private readonly IDbContextFactory<Contexto> _factory;

    public ProductoService(IDbContextFactory<Contexto> factory)
    {
        _factory = factory;
    }

    public async Task<Result<List<Producto>>> GetList(Expression<Func<Producto, bool>> filtro)
    {
        using var context = await _factory.CreateDbContextAsync();
        var lista = await context.Productos.Where(filtro).ToListAsync();
        return Result.Success(lista);
    }

    public async Task<Result<Producto?>> Buscar(int id)
    {
        using var context = await _factory.CreateDbContextAsync();
        var resultado = await context.Productos.FirstOrDefaultAsync(x => x.ProductoId == id);
        return Result.Success(resultado);
    }

    public async Task<Result> Guardar(Producto entidad)
    {
        try
        {
            using var context = await _factory.CreateDbContextAsync();
            if (entidad.ProductoId == 0)
                context.Productos.Add(entidad);
            else
                context.Productos.Update(entidad);
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
            var entidad = await context.Productos.FirstOrDefaultAsync(x => x.ProductoId == id);
            if (entidad == null)
                return Result.Failure(new Error("NoEncontrado", "El producto no existe"));
            context.Productos.Remove(entidad);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("ErrorEliminar", ex.Message));
        }
    }
}

