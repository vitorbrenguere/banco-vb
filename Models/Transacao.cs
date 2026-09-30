using System.Text.Json.Serialization;

namespace banco_vb.Models;

public class Transacao
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty; // "Deposito", "Saque", "TransferenciaEnviada", "TransferenciaRecebida"
    public decimal Valor { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    // Chave Estrangeira e Propriedade de Navegação
    public int ContaId { get; set; }

    [JsonIgnore]
    public Conta? Conta { get; set; }
}