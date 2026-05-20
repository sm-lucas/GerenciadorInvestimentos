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
 }