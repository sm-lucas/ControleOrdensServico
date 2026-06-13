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
        static void CriarOS()
        {
            Console.Clear();
            Console.WriteLine("--- Abertura de Chamado ---");

            Console.Write("Nome do Equipamento/Sistema: ");
            string equipamento = Console.ReadLine().Trim();

            Console.Write("Descrição do Defeito: ");
            string defeito = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(equipamento) || string.IsNullOrEmpty(defeito))
            {
                Console.WriteLine("\nErro: Todos os campos são obrigatórios!");
                Console.ReadKey();
                return;
            }

            bancoOS.Add(new OrdemServico
            {
                Id = proximoId++,
                Equipamento = equipamento,
                DescricaoDefeito = defeito,
                Status = "Aberto",
                DataAbertura = DateTime.Now
            });

            Console.WriteLine("\nOrdem de Serviço aberta com sucesso!");
            Console.ReadKey();
        }

        static void AtualizarStatusOS()
        {
            Console.Write("\nDigite o ID da OS que deseja atualizar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var os = bancoOS.FirstOrDefault(x => x.Id == id);
                if (os != null)
                {
                    Console.WriteLine($"\nStatus atual: {os.Status}");
                    Console.WriteLine("Escolha o novo status: 1. Aberto | 2. Em Andamento | 3. Concluído");
                    Console.Write("Opção: ");

                    string op = Console.ReadLine();
                    if (op == "1") os.Status = "Aberto";
                    else if (op == "2") os.Status = "Em Andamento";
                    else if (op == "3") os.Status = "Concluído";
                    else Console.WriteLine("Opção inválida. Status não alterado.");
                }
                else
                {
                    Console.WriteLine("Ordem de Serviço não encontrada.");
                }
            }
            Console.ReadKey();
        }

        static void RemoverOS()
        {
            Console.Write("\nDigite o ID da OS que deseja remover/cancelar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var os = bancoOS.FirstOrDefault(x => x.Id == id);
                if (os != null)
                {
                    bancoOS.Remove(os);
                    Console.WriteLine("\nOrdem de Serviço removida!");
                }
                else
                {
                    Console.WriteLine("ID não encontrado.");
                }
            }
            Console.ReadKey();
        }

        static void SalvarOrdensNoArquivo()
        {
            try
            {
                var linhas = bancoOS.Select(x => $"{x.Id};{x.Equipamento};{x.DescricaoDefeito};{x.Status};{x.DataAbertura}");
                File.WriteAllLines(arquivoOS, linhas);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar dados: " + ex.Message);
            }
        }

        static void CarregarOrdensDoArquivo()
        {
            try
            {
                if (File.Exists(arquivoOS))
                {
                    string[] linhas = File.ReadAllLines(arquivoOS);
                    foreach (string linha in linhas)
                    {
                        string[] p = linha.Split(';');
                        if (p.Length == 5)
                        {
                            var os = new OrdemServico
                            {
                                Id = int.Parse(p[0]),
                                Equipamento = p[1],
                                DescricaoDefeito = p[2],
                                Status = p[3],
                                DataAbertura = DateTime.Parse(p[4])
                            };
                            bancoOS.Add(os);

                            if (os.Id >= proximoId) proximoId = os.Id + 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                bancoOS = new List<OrdemServico>();
            }
        }
    }
}