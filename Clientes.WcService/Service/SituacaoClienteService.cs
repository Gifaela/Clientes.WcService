using Clientes.WcService.Interface;
using Clientes.WcService;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

public class SituacaoClienteService : ISituacaoCliente
{
    private readonly string _connectionString;

    public SituacaoClienteService()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["ClientesDb"].ConnectionString;
    }

    public async Task<List<SituacaoClienteModel>> ListarSituacoes()
    {
        var situacoes = new List<SituacaoClienteModel>();

        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("sp_Situacao_List", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    situacoes.Add(new SituacaoClienteModel
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1)
                    });
                }
            }
        }

        return situacoes;
    }
}
