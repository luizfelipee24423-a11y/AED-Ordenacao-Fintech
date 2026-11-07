
public class GeradorTransacoes
{
    public static Transacao[] Gerar(int quantidade)
    {
        Random random = new Random();

        Transacao[] transacoes = new Transacao[quantidade];

        string[] status = { "Aprovada", "Pendente", "Cancelada" };

        for (int i = 0; i < quantidade; i++)
        {
            transacoes[i] = new Transacao
            {
                Id = i + 1,
                Valor = random.Next(100, 1000001) / 100m,
                Data = DateTime.Today.AddDays(-random.Next(0, 30)),
                Status = status[random.Next(status.Length)]
            };
        }

        return transacoes;
    }
}
