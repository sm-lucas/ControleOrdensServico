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
    class Program
    {
        static List<OrdemServico> bancoOS = new List<OrdemServico>();
        static int proximoId = 1;
        static string arquivoOS = "historico_ordens.txt";

        static void Main(string[] args)
        {
            CarregarOrdensDoArquivo();
            bool ativo = true;

            while (ativo)
            {
                Console.Clear();
                Console.WriteLine("==========================================================================");
                Console.WriteLine("                    SISTEMA DE CONTROL DE ORDENS DE SERVIÇO               ");
                Console.WriteLine("==========================================================================");

                ListarOrdens();

                Console.WriteLine("\n--------------------------------------------------------------------------");
                Console.WriteLine("1. Abrir Nova Ordem de Serviço (OS)");
                Console.WriteLine("2. Atualizar Status da OS (Progresso)");
                Console.WriteLine("3. Cancelar/Remover OS");
                Console.WriteLine("4. Sair");
                Console.Write("\nEscolha uma opção: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CriarOS();
                        break;
                    case "2":
                        AtualizarStatusOS();
                        break;
                    case "3":
                        RemoverOS();
                        break;
                    case "4":
                        ativo = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Toque em qualquer tecla.");
                        Console.ReadKey();
                        break;
                }
                SalvarOrdensNoArquivo();
            }
        }

        static void ListarOrdens()
        {
            if (!bancoOS.Any())
            {
                Console.WriteLine("\nNenhuma Ordem de Serviço registrada no momento.");
                return;
            }

            Console.WriteLine($"\nTotal de Chamados: {bancoOS.Count} | Pendentes: {bancoOS.Count(x => x.Status != "Concluído")}\n");
            foreach (var os in bancoOS)
            {
                Console.WriteLine(os);
            }
        }