using System.ComponentModel.DataAnnotations;

namespace Danluis_AP1_P2.Models;

public class Venta
{
    [Key]
    public int VentaId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    public string Observacion { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}
