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

    public Task<Result<List<Modelo>>> GetList(Expression<Func<Modelo, bool>> criterio)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Modelo?>> Buscar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Guardar(Modelo modelo)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Eliminar(int id)
    {
        throw new NotImplementedException();
    }
}