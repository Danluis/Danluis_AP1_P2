using System.ComponentModel.DataAnnotations;

namespace Danluis_AP1_P2.Models;

public class Modelo
{
    [Key]
    public int ModeloId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public string Nombre { get; set; } = string.Empty;
}