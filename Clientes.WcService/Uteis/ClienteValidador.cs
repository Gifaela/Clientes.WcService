using System.Collections.Generic;
using System.Linq;
using System;

namespace Clientes.WcService.Uteis
{
    public class ClienteValidador
    {
        private const int IdadeMinima = 18;
        private const int IdadeMaxima = 120;

        public static void ValidarNovoCliente(
            ClienteModel cliente,
            List<ClienteModel> clientesExistentes,
            List<SituacaoClienteModel> situacoesExistentes)
        {
            ValidarObjeto(cliente);
            ValidarNome(cliente.Nome);
            ValidarCpf(cliente.Cpf, clientesExistentes, cliente.Id); 
            ValidarSexo(cliente.Sexo);
            ValidarDataNascimento(cliente.DataNascimento);
            ValidarSituacao(cliente.IdSituacao, situacoesExistentes);
        }

        private static void ValidarObjeto(ClienteModel cliente)
        {
            if (cliente == null)
                throw new ArgumentException("Cliente não pode ser nulo");
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório");
        }

        private static void ValidarCpf(string cpf, List<ClienteModel> clientesExistentes, int? idAtual = null)
        {
            var cpfNumeros = new string(cpf.Where(char.IsDigit).ToArray());

            if (!CpfValidador.IsValid(cpfNumeros))
                throw new ArgumentException("CPF inválido");

            if (clientesExistentes.Any(c =>
                new string(c.Cpf.Where(char.IsDigit).ToArray()) == cpfNumeros &&
                (idAtual == null || c.Id != idAtual.Value)))
            {
                throw new ArgumentException("CPF já cadastrado");
            }
        }

        private static void ValidarSexo(string sexo)
        {
            if (!(sexo is null || sexo.Equals(string.Empty) ||
               sexo.Equals("F", StringComparison.OrdinalIgnoreCase) ||
               sexo.Equals("M", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Sexo inválido. Use apenas 'M' ou 'F'.");
            }
        }

        private static void ValidarDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento == DateTime.MinValue)
                throw new ArgumentException("Data de nascimento é obrigatória");

            if (dataNascimento > DateTime.Now)
                throw new ArgumentException("Data de nascimento não pode ser no futuro");

            var idade = CalcularIdade(dataNascimento);

            if (idade < IdadeMinima)
                throw new ArgumentException($"Cliente deve ter pelo menos {IdadeMinima} anos");

            if (idade > IdadeMaxima)
                throw new ArgumentException($"Idade do cliente inválida (máx {IdadeMaxima} anos)");
        }

        private static int CalcularIdade(DateTime dataNascimento)
        {
            var idade = DateTime.Now.Year - dataNascimento.Year;
            if (dataNascimento > DateTime.Now.AddYears(-idade)) idade--;
            return idade;
        }

        private static void ValidarSituacao(int idSituacao, List<SituacaoClienteModel> situacoesExistentes)
        {
            if (!situacoesExistentes.Any(s => s.Id == idSituacao))
                throw new ArgumentException("Situação do cliente inválida");
        }
    }
}
