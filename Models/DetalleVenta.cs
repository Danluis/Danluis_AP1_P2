using System.ComponentModel.DataAnnotations;

namespace Danluis_AP1_P2.Models;

public class DetalleVenta
{
    [Key]
    public int DetalleId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public int VentaId { get; set; }
    public Venta? Venta { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public int Cantidad { get; set; }

    public decimal Precio { get; set; }
}
