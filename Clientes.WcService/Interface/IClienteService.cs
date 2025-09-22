using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Clientes.WcService
{

    [ServiceContract]
    public interface IClienteService
    {
        [OperationContract]
        Task<List<ClienteModel>> ListarClientes();

        [OperationContract]
        Task<ClienteModel> ObterCliente(int id);

        [OperationContract]
        Task<bool> CriarCliente(ClienteModel cliente);

        [OperationContract]
        Task<bool> AtualizarCliente(ClienteModel cliente);

        [OperationContract]
        Task<bool> ExcluirCliente(int id);
        [OperationContract]
        Task<List<SituacaoClienteModel>> ListarSituacoes();
    }
}
