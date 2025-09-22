using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Clientes.WcService.Interface
{
    [ServiceContract]
    public interface ISituacaoCliente
    {
        [OperationContract]
        Task<List<SituacaoClienteModel>> ListarSituacoes();
    }
}
