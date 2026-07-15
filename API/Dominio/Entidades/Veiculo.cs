using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MinimalApi.Dominio.Entidades;

public class Veiculo
{

    [Key]

   [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
   
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = default!;
    [Required]
    [StringLength(100)]
    public string Marca { get; set; } = default!;
    [Required]
    [StringLength(10)]
    public int Ano{ get; set; } = default!;
}
