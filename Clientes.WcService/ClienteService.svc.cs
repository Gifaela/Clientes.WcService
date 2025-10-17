using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Clientes.WcService.Uteis;
using Clientes.WcService.Interface;

namespace Clientes.WcService
{
    public class ClienteService : IClienteService
    {
        private readonly string _connectionString;
        private readonly ISituacaoCliente _situacaoService;

        public ClienteService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ClientesDb"].ConnectionString;
            _situacaoService = new SituacaoClienteService();
        }

        public ClienteService(ISituacaoCliente situacaoService)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ClientesDb"].ConnectionString;
            _situacaoService = situacaoService;
        }

        // 🔹 Lista todos os clientes já com nome da situação
        public async Task<List<ClienteModel>> ListarClientes()
        {
            var clientes = new List<ClienteModel>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT c.Id, c.Nome, c.Cpf, c.DataNascimento, c.Sexo, 
                       c.IdSituacao, s.Nome AS NomeSituacao
                FROM Clientes c
                INNER JOIN SituacoesCliente s ON c.IdSituacao = s.Id", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clientes.Add(new ClienteModel
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Cpf = reader.GetString(2),
                            DataNascimento = reader.GetDateTime(3),
                            Sexo = reader.IsDBNull(4) ? null : reader.GetString(4),
                            IdSituacao = reader.GetInt32(5),
                            NomeSituacao = reader.GetString(6)
                        });
                    }
                }
            }

            return clientes;
        }

        // 🔹 Busca cliente específico já com nome da situação
        public async Task<ClienteModel> ObterCliente(int id)
        {
            ClienteModel cliente = null;

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT c.Id, c.Nome, c.Cpf, c.DataNascimento, c.Sexo, 
                       c.IdSituacao, s.Nome AS NomeSituacao
                FROM Clientes c
                INNER JOIN SituacoesCliente s ON c.IdSituacao = s.Id
                WHERE c.Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cliente = new ClienteModel
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Cpf = reader.GetString(2),
                            DataNascimento = reader.GetDateTime(3),
                            Sexo = reader.IsDBNull(4) ? null : reader.GetString(4),
                            IdSituacao = reader.GetInt32(5),
                            NomeSituacao = reader.GetString(6)
                        };
                    }
                }
            }

            return cliente;
        }

        public async Task<bool> CriarCliente(ClienteModel cliente)
        {
            var clientes = await ListarClientes();
            var situacoes = await _situacaoService.ListarSituacoes();

    
            ClienteValidador.ValidarNovoCliente(cliente, clientes, situacoes);


            cliente.Cpf = new string(cliente.Cpf.Where(char.IsDigit).ToArray());

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Cliente_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
                cmd.Parameters.AddWithValue("@Sexo", (object)cliente.Sexo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdSituacao", cliente.IdSituacao);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<bool> AtualizarCliente(ClienteModel cliente)
        {
            var clientes = await ListarClientes();
            var situacoes = await _situacaoService.ListarSituacoes();

            ClienteValidador.ValidarNovoCliente(cliente, clientes, situacoes);

            cliente.Cpf = new string(cliente.Cpf.Where(char.IsDigit).ToArray());

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Cliente_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", cliente.Id);
                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
                cmd.Parameters.AddWithValue("@Sexo", (object)cliente.Sexo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdSituacao", cliente.IdSituacao);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }

        public async Task<bool> ExcluirCliente(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Cliente_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }

        public async Task<List<SituacaoClienteModel>> ListarSituacoes()
        {
            return await _situacaoService.ListarSituacoes();
        }
    }
}
