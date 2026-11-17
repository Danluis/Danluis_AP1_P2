using System.ComponentModel.DataAnnotations;

namespace Danluis_AP1_P2.Models;

public class Producto
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public decimal Precio { get; set; }

    public int Stock { get; set; } = 10;
}
