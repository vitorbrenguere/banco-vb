using System.ComponentModel.DataAnnotations;

namespace banco_vb.DTOs;

public class TransferenciaDto
{
    [Required(ErrorMessage = "O ID da conta de destino é obrigatório.")]
    public int ContaDestinoId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor da transferência deve ser maior que zero.")]
    public decimal Valor { get; set; }
}