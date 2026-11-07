
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

int[] tamanhos = { 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };
int repeticoes = 3;
string arquivoResultados = "resultados.csv";

if (!File.Exists(arquivoResultados))
{
    File.WriteAllText(
        arquivoResultados,
        "Data;Registros;Algoritmo;Execucao;Tempo_ms;OrdenacaoCorreta" +
        Environment.NewLine
    );
}

while (true)
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("   SISTEMA DE TRANSAÇÕES FINANCEIRAS");
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("Escolha a quantidade de transações:");
    Console.WriteLine();

    for (int i = 0; i < tamanhos.Length; i++)
    {
        Console.WriteLine($"{i + 1} - {tamanhos[i]:N0} transações");
    }

    Console.WriteLine("0 - Sair");
    Console.WriteLine();
    Console.Write("Opção: ");

    if (!int.TryParse(Console.ReadLine(), out int opcao))
    {
        Console.WriteLine("Opção inválida.");
        continue;
    }

    if (opcao == 0)
    {
        Console.WriteLine("Programa encerrado.");
        break;
    }

    if (opcao < 1 || opcao > tamanhos.Length)
    {
        Console.WriteLine("Opção inválida.");
        continue;
    }

    int n = tamanhos[opcao - 1];

    Console.WriteLine();
    Console.WriteLine("Escolha o algoritmo:");
    Console.WriteLine("1 - Insertion Sort");
    Console.WriteLine("2 - Merge Sort");
    Console.WriteLine("3 - Quick Sort");
    Console.WriteLine("4 - Todos os algoritmos");
    Console.WriteLine("5 - Merge Sort e Quick Sort");
    Console.WriteLine();
    Console.Write("Opção: ");

    if (!int.TryParse(Console.ReadLine(), out int algoritmo) ||
        algoritmo < 1 || algoritmo > 5)
    {
        Console.WriteLine("Algoritmo inválido.");
        continue;
    }

    if (n >= 100000 && (algoritmo == 1 || algoritmo == 4))
    {
        Console.WriteLine();
        Console.WriteLine("ATENÇÃO: O Insertion Sort pode demorar muito.");
        Console.Write("Deseja continuar? (S/N): ");

        if (Console.ReadLine()?.Trim().ToUpperInvariant() != "S")
        {
            continue;
        }
    }

    if (n >= 10000000)
    {
        Console.WriteLine();
        Console.WriteLine("ATENÇÃO: Este teste pode consumir muita memória RAM.");
        Console.Write("Confirmar execução? (S/N): ");

        if (Console.ReadLine()?.Trim().ToUpperInvariant() != "S")
        {
            continue;
        }
    }

    try
    {
        Console.WriteLine();
        Console.WriteLine($"Gerando {n:N0} transações...");

        Transacao[] transacoes = GeradorTransacoes.Gerar(n);

        Console.WriteLine("Transações geradas com sucesso.");
        Console.WriteLine();

        string[] nomes = { "Insertion Sort", "Merge Sort", "Quick Sort" };

        int[] algoritmosSelecionados = algoritmo switch
        {
            1 => new int[] { 0 },
            2 => new int[] { 1 },
            3 => new int[] { 2 },
            4 => new int[] { 0, 1, 2 },
            5 => new int[] { 1, 2 },
            _ => Array.Empty<int>()
        };

        foreach (int a in algoritmosSelecionados)
        {
            double somaTempos = 0;
            double menorTempo = double.MaxValue;
            double maiorTempo = 0;
            bool todasCorretas = true;

            Console.WriteLine($"========== {nomes[a]} ==========");
            Console.WriteLine();

            for (int r = 1; r <= repeticoes; r++)
            {
                Transacao[] copia = (Transacao[])transacoes.Clone();

                Stopwatch cronometro = Stopwatch.StartNew();

                if (a == 0)
                {
                    InsertionSort.Ordenar(copia);
                }
                else if (a == 1)
                {
                    MergeSort.Ordenar(copia, 0, copia.Length - 1);
                }
                else
                {
                    QuickSort.Ordenar(copia, 0, copia.Length - 1);
                }

                cronometro.Stop();

                double tempo = cronometro.Elapsed.TotalMilliseconds;

                bool ordenado = true;

                for (int i = 1; i < copia.Length; i++)
                {
                    if (copia[i - 1].Valor > copia[i].Valor)
                    {
                        ordenado = false;
                        break;
                    }
                }

                somaTempos += tempo;
                menorTempo = Math.Min(menorTempo, tempo);
                maiorTempo = Math.Max(maiorTempo, tempo);

                if (!ordenado)
                {
                    todasCorretas = false;
                }

                Console.WriteLine(
                    $"Execução {r}: {tempo:F3} ms | Correto: {ordenado}"
                );

                string registro =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};" +
                    $"{n};" +
                    $"{nomes[a]};" +
                    $"{r};" +
                    $"{tempo.ToString("F3", CultureInfo.InvariantCulture)};" +
                    $"{ordenado}";

                File.AppendAllText(
                    arquivoResultados,
                    registro + Environment.NewLine
                );

                if (r == repeticoes && ordenado &&
                    a == algoritmosSelecionados[^1])
                {
                    Console.WriteLine();
                    Console.WriteLine("========== 10 MAIORES TRANSAÇÕES ==========");
                    Console.WriteLine();

                    int quantidade = Math.Min(10, copia.Length);

                    for (int i = copia.Length - 1;
                         i >= copia.Length - quantidade; i--)
                    {
                        Transacao t = copia[i];

                        Console.WriteLine(
                            $"ID: {t.Id} | " +
                            $"Valor: R$ {t.Valor:F2} | " +
                            $"Data: {t.Data:dd/MM/yyyy} | " +
                            $"Status: {t.Status}"
                        );
                    }

                    Console.WriteLine();
                }
            }

            double media = somaTempos / repeticoes;

            Console.WriteLine();
            Console.WriteLine($"Tempo médio: {media:F3} ms");
            Console.WriteLine($"Menor tempo: {menorTempo:F3} ms");
            Console.WriteLine($"Maior tempo: {maiorTempo:F3} ms");
            Console.WriteLine($"Todas as ordenações corretas: {todasCorretas}");
            Console.WriteLine();
        }

        Console.WriteLine("Resultados salvos em resultados.csv");
    }
    catch (OutOfMemoryException)
    {
        Console.WriteLine();
        Console.WriteLine("Memória insuficiente para executar esse teste.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

//ss