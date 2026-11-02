using Danluis_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;

namespace Danluis_AP1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Modelo> Modelos { get; set; }
}