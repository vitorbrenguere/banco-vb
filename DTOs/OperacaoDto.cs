using System.ComponentModel.DataAnnotations;

namespace banco_vb.DTOs;

public class OperacaoDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor da operação deve ser maior que zero.")]
    public decimal Valor { get; set; }
}