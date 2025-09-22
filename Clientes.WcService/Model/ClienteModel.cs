using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Clientes.WcService
{
    [DataContract]
    public class ClienteModel
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Nome { get; set; } = string.Empty;
        [DataMember(Order = 3)] public string Cpf { get; set; } = string.Empty;
        [DataMember(Order = 4)] public DateTime DataNascimento { get; set; }
        [DataMember(Order = 5)] public string Sexo { get; set; }
        [DataMember(Order = 6)] public int IdSituacao { get; set; }
        [DataMember(Order = 7)] public string NomeSituacao { get; set; }
    }
}