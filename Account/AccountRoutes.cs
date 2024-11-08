using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;

public static class AccountRoutes
{
    public static void AddAccountRoutes(this WebApplication app)

    {
        var AccountRoutes = app.MapGroup(prefix: "account");

        // Post - criar conta

        AccountRoutes.MapPost(pattern: "", handler: async (AppDbContext context, AddAccountRequest request) =>
        {
            if (request.name == null || request.password == null || request.confirmPassword == null)

                return Results.Conflict(error: "Todos os campos devem ser preenchidos");

            if (request.password.ToLower() != request.confirmPassword.ToLower())
                return Results.Conflict(error: "As senhas devem ser iguais");

            bool AccountExists = await context.Accounts.AnyAsync(account => account.Name.ToLower() == request.name.ToLower());

            if (!AccountExists)

            {

                var newAccount = new Account(request.name, request.password);

                await context.AddRangeAsync(newAccount);

                await context.SaveChangesAsync();

                return Results.Ok(new AccountDto(newAccount.Name, newAccount.AccountNumber, newAccount.Balance));

            }
            else
            {
                return Results.Conflict(error: "Conta já existe");
            }

        });

        // Get - Consultar saldo

        AccountRoutes.MapGet(pattern: "/balance/{accountNumber}", handler: async(string accountNumber, AppDbContext context)=>{
          
            var account = await context.Accounts.FirstOrDefaultAsync(acount => acount.AccountNumber == accountNumber );
          if(account!=null){
          return Results.Ok(new AccountDto(account.Name,account.AccountNumber, account.Balance));
          }
          else{
            return Results.Conflict(error:"Conta inválida");
          }
        });


        // Get - todas as contas
        // Nivel administrador

        AccountRoutes.MapGet(pattern: "", handler: async (AppDbContext context, CancellationToken ct) =>
           {
               var account = await context.Accounts.ToListAsync(ct);

               return account;
           });

        // Put - Depositar dinheiro

        AccountRoutes.MapPut(pattern: "/deposit", handler: async (AppDbContext context, DepositAccountRequest request, CancellationToken ct) =>
        {
            var account = await context.Accounts.FirstOrDefaultAsync(account => account.AccountNumber == request.account);
            if (account == null)
                return Results.Conflict(error: "A conta para depósito deve ser informada!");

            account.updateBalance(request.value);
            await context.SaveChangesAsync();

            return Results.Ok(new AccountDto(account.Name, account.AccountNumber, account.Balance));

        });

        // Put - sacar dinheiro
        // Incluir autenticação JWT

        AccountRoutes.MapPut(pattern: "/withdrawal", handler: async (AppDbContext context, withdrawalAccountRequest request, CancellationToken ct) =>
        {
            var account = await context.Accounts.SingleOrDefaultAsync(account => account.AccountNumber == request.account);

            if (account == null)

                return Results.Conflict(error: "Conta deve ser informada!");

            if (account.Balance >= request.value)
            {
                account.DowngradeBalance(request.value);
                await context.SaveChangesAsync(ct);
            }
            else
            {
                return Results.Conflict(error: "Saldo insuficiente!");
            }

            return Results.Ok(new AccountDto(account.Name, account.AccountNumber, account.Balance));


        });





    }
}
