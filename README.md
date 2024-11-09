# Simulador de Banco

Um programa que simula operações bancárias (depósitos, saques, transferências e consultas de saldo) para contas de clientes.

## Funcionalidades

1. **Criar Conta**
   - Permite que o usuário crie uma nova conta bancária com um número de conta exclusivo e um nome de titular usando uma rota post que gera randomicamente o número da conta, e é criado um hash para a senha usando a Lib BCrypt.Net.Core
 
2. **Depositar Dinheiro**
   - Permite que o usuário faça um depósito em sua conta, informando o valor e a conta a ser depositada.
   - Atualiza o saldo da conta após o depósito.

3. **Sacar Dinheiro**
   - Permite que o usuário faça um saque, informando o valor desejado.
   - Verifica se o saldo é suficiente para a operação antes de efetuar o saque.
   - Atualiza o saldo da conta após o saque.

4. **Consultar Saldo**
   - Permite que o usuário consulte o saldo atual de sua conta.

5. **Transferir Dinheiro**
   - Permite que o usuário transfira um valor de uma conta para outra.
   - Verifica se o saldo é suficiente antes de efetuar a transferência.

6. **Listar Todas as Contas**
   - Exibe uma lista de todas as contas criadas, com detalhes como número da conta e nome do titular.

7. **Fechar Conta**
   - Permite que o usuário feche uma conta existente (apenas se o saldo for zero).

## Requisitos de Instalação

Para rodar este projeto, é necessário ter o .NET SDK instalado. Para instalar, siga as instruções [neste link](https://dotnet.microsoft.com/download).

## Como Executar o Projeto

1. Clone este repositório:
   ```bash
   git clone https://github.com/Jjunior112/BankAccount
   ```
2. Entre na pasta do projeto:
   ```bash
   cd BankAccount
   ```
3. Compile e execute o projeto:
   ```bash
   dotnet run
   ```
