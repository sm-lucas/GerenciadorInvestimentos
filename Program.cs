using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GerenciadorInvestimentos
{
    // Modelo do Ativo Financeiro
    class Ativo
    {
        public string Ticket { get; set; } // Ex: PETR4, IVVB11
        public int Quantidade { get; set; }
        public double PrecoMedio { get; set; }
        public double PrecoAtual { get; set; }

        public double TotalInvestido => Quantidade * PrecoMedio;
        public double ValorAtualizado => Quantidade * PrecoAtual;
        public double LucroPrejuizo => ValorAtualizado - TotalInvestido;

        public override string ToString()
        {
            return $"{Ticket.ToUpper()} | Qtd: {Quantidade} | P.Médio: R${PrecoMedio:F2} | P.Atual: R${PrecoAtual:F2} | Rendimento: R${LucroPrejuizo:F2}";
        }
    }
    class Program
    {
        static List<Ativo> carteira = new List<Ativo>();
        static string arquivoDados = "carteira_investimentos.txt";

        static void Main(string[] args)
        {
            CarregarDados();
            bool rodando = true;

            while (rodando)
            {
                Console.Clear();
                Console.WriteLine("==================================================================");
                Console.WriteLine("                SISTEMA DE GESTÃO DE INVESTIMENTOS                ");
                Console.WriteLine("==================================================================");

                ExibirCarteira();

                Console.WriteLine("\n------------------------------------------------------------------");
                Console.WriteLine("1. Comprar Ativo (Adicionar/Incrementar)");
                Console.WriteLine("2. Atualizar Preço de Mercado");
                Console.WriteLine("3. Vender Ativo (Remover)");
                Console.WriteLine("4. Sair");
                Console.Write("\nEscolha uma opção: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ComprarAtivo();
                        break;
                    case "2":
                        AtualizarPreco();
                        break;
                    case "3":
                        VenderAtivo();
                        break;
                    case "4":
                        rodando = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Pressione qualquer tecla.");
                        Console.ReadKey();
                        break;
                }
                SalvarDados();
            }
        }

        static void ExibirCarteira()
        {
            if (!carteira.Any())
            {
                Console.WriteLine("\nSua carteira está vazia. Comece a investir!");
                return;
            }

            double totalCarteira = carteira.Sum(a => a.ValorAtualizado);
            Console.WriteLine($"\nVALOR TOTAL DA CARTEIRA: R${totalCarteira:F2}\n");

            for (int i = 0; i < carteira.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {carteira[i]}");
            }
        }
        static void ComprarAtivo()
        {
            Console.Write("\nDigite o código do ativo (ex: VALE3): ");
            string ticket = Console.ReadLine().Trim().ToUpper();

            Console.Write("Quantidade: ");
            int.TryParse(Console.ReadLine(), out int qtd);

            Console.Write("Preço de Compra: R$ ");
            double.TryParse(Console.ReadLine(), out double preco);

            var ativoExistente = carteira.FirstOrDefault(a => a.Ticket == ticket);

            if (ativoExistente != null)
            {
                // Cálculo de preço médio ponderado (Regra financeira real)
                double novoCustoTotal = ativoExistente.TotalInvestido + (qtd * preco);
                ativoExistente.Quantidade += qtd;
                ativoExistente.PrecoMedio = novoCustoTotal / ativoExistente.Quantidade;
                ativoExistente.PrecoAtual = preco; // Atualiza com a última cotação
            }
            else
            {
                carteira.Add(new Ativo { Ticket = ticket, Quantidade = qtd, PrecoMedio = preco, PrecoAtual = preco });
            }

            Console.WriteLine("\nOrdem de compra executada com sucesso!");
            Console.ReadKey();
        }
        static void AtualizarPreco()
        {
            Console.Write("\nDigite o número do ativo que deseja atualizar: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= carteira.Count)
            {
                Console.Write($"Digite o novo preço de mercado para {carteira[index - 1].Ticket}: R$ ");
                double.TryParse(Console.ReadLine(), out double novoPreco);
                carteira[index - 1].PrecoAtual = novoPreco;
                Console.WriteLine("\nPreço atualizado!");
            }
            else
            {
                Console.WriteLine("Ativo não encontrado.");
            }
            Console.ReadKey();
        }

        static void VenderAtivo()
        {
            Console.Write("\nDigite o número do ativo que deseja vender (remover): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= carteira.Count)
            {
                Console.WriteLine($"Removendo {carteira[index - 1].Ticket} da carteira...");
                carteira.RemoveAt(index - 1);
                Console.WriteLine("Ativo removido!");
            }
            else
            {
                Console.WriteLine("Opção inválida.");
            }
            Console.ReadKey();
        }

        static void SalvarDados()
        {
            try
            {
                var linhas = carteira.Select(a => $"{a.Ticket};{a.Quantidade};{a.PrecoMedio};{a.PrecoAtual}");
                File.WriteAllLines(arquivoDados, linhas);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar: " + ex.Message);
            }
        }

        static void CarregarDados()
        {
            try
            {
                if (File.Exists(arquivoDados))
                {
                    string[] linhas = File.ReadAllLines(arquivoDados);
                    foreach (string linha in linhas)
                    {
                        string[] p = linha.Split(';');
                        if (p.Length == 4)
                        {
                            carteira.Add(new Ativo
                            {
                                Ticket = p[0],
                                Quantidade = int.Parse(p[1]),
                                PrecoMedio = double.Parse(p[2]),
                                PrecoAtual = double.Parse(p[3])
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                carteira = new List<Ativo>();
            }
        }
    }
}