using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ControleOrdensServico
{
    // Modelo da Ordem de Serviço (OS)
    class OrdemServico
    {
        public int Id { get; set; }
        public string Equipamento { get; set; }
        public string DescricaoDefeito { get; set; }
        public string Status { get; set; } // Aberto, Em Andamento, Concluído
        public DateTime DataAbertura { get; set; }

        public override string ToString()
        {
            string statusFormatado = Status.PadRight(13);
            return $"OS: {Id:D3} | Status: [{statusFormatado}] | Equipamento: {Equipamento.PadRight(15)} | Defeito: {DescricaoDefeito}";
        }
    }
