# Projeto de Gerenciamento de Pet Shop

Este é um projeto de gerenciamento de pet shop desenvolvido com **ASP.NET Core** e **PostgreSQL**. O sistema permite a gestão de clientes, funcionários, pets, vendas de produtos e serviços de banho e tosa, e controle de fornecedores e produtos.

## Tecnologias Utilizadas

- **ASP.NET Core 8.0**: Framework principal para o desenvolvimento do backend.
- **Entity Framework Core**: ORM para interação com o banco de dados.
- **PostgreSQL**: Banco de dados utilizado para armazenar informações de clientes, funcionários, pets, etc.
- **C#**: Linguagem de programação utilizada para a implementação.

## Como Rodar o Projeto

### 1. Requisitos

Certifique-se de ter o seguinte instalado em seu ambiente antes de iniciar:

- **.NET 8.0 SDK** 
- **PostgreSQL** 

### 2. Instruções de Instalação

### Passos

1. **Configure a Conexão com o Banco de Dados**  
   Abra o arquivo `appsettings.json` e insira as configurações de conexão com o PostgreSQL.

2. **Execute os comandos** 
```bash
    dotnet restore .\prjGura.csproj
    dotnet build .\prjGura.csproj
    dotnet run --project "prjGura.csproj"
 ```
