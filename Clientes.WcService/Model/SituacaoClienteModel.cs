using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Clientes.WcService
{
    public class SituacaoClienteModel
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Nome { get; set; } = string.Empty;

    }
}