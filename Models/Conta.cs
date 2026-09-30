namespace banco_vb.Models;

public class Conta
{
    public int Id { get; set; }
    public string Titular { get; set; } = string.Empty;
    public decimal Saldo { get; set; }

    // Uma conta possui várias transações
    public List<Transacao> Transacoes { get; set; } = new();
}