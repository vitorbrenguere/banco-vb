using System.ComponentModel.DataAnnotations;

namespace banco_vb.DTOs;

public class ContaCriacaoDto
{
    [Required(ErrorMessage = "O nome do titular é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do titular deve ter entre 3 e 100 caracteres.")]
    public string Titular { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "O saldo inicial não pode ser negativo.")]
    public decimal Saldo { get; set; }
}