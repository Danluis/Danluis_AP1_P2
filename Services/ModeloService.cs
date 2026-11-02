using Aplicada1.Core;
using Danluis_AP1_P2.Context;
using Danluis_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Danluis_AP1_P2.Services;

public class ModeloService : IServiceResult<Modelo, int>
{
    private readonly IDbContextFactory<Contexto> DbFactory;

    public ModeloService(IDbContextFactory<Contexto> dbFactory)
    {
        DbFactory = dbFactory;
    }

    // Devuelve una lista de registros que cumplan con el criterio recibido.
    public async Task<Result<List<Modelo>>> GetList(Expression<Func<Modelo, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var lista = await contexto.Modelos.Where(criterio).ToListAsync();
        return Result.Success(lista);
    }

    // Busca un solo registro por su Id. Puede devolver null si no existe.
    public async Task<Result<Modelo?>> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var modelo = await contexto.Modelos.FirstOrDefaultAsync(m => m.ModeloId == id);
        return Result.Success(modelo);
    }

    // Crea o modifica un registro, según si ya existe o no en la base.
    public async Task<Result> Guardar(Modelo modelo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var existe = await contexto.Modelos.AnyAsync(m => m.ModeloId == modelo.ModeloId);

        if (existe)
        {
            contexto.Modelos.Update(modelo);
        }
        else
        {
            contexto.Modelos.Add(modelo);
        }

        var filasAfectadas = await contexto.SaveChangesAsync();

        if (filasAfectadas > 0)
        {
            return Result.Success();
        }
        else
        {
            return Result.Failure(new Error("NO_GUARDADO", "No se pudo guardar el registro."));
        }
    }

    // Elimina un registro por su Id.
    public async Task<Result> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var modelo = await contexto.Modelos.FirstOrDefaultAsync(m => m.ModeloId == id);

        if (modelo is null)
        {
            return Result.Failure(new Error("NO_ENCONTRADO", "Registro no encontrado."));
        }

        contexto.Modelos.Remove(modelo);
        await contexto.SaveChangesAsync();

        return Result.Success();
    }
}