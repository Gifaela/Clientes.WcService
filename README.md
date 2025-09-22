# 🖥️ Clientes Backend (WCF)

Este projeto implementa um serviço **WCF** responsável por gerenciar clientes e situações no banco de dados SQL Server.

---

## 🚀 Tecnologias
- .NET Framework 4.8  
- WCF (Windows Communication Foundation)  
- SQL Server  
- Stored Procedures  

---

## 📂 Estrutura
- `Clientes.WcService` → Serviço WCF  
- `Clientes.WcService.Interface` → Interfaces (contratos)  
- `Clientes.WcService.Models` → Modelos de dados  
- `Clientes.WcService.Uteis` → Utilitários  

---

## 📜 Banco de Dados
O script para criação do banco, tabelas e procedures está no arquivo [`database.sql`](./Clientes.WcService/database.sql).

## ⚙️ Configuração

1. Ajustar a **connection string** no arquivo `Web.config`:  
   ```xml
   <connectionStrings>
     <add name="ClientesDb"
          connectionString="Server=SEU_SERVIDOR;Database=ClientesDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
