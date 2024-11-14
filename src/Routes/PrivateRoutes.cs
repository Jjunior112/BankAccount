using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;

public static class PrivateRoutes
{
    public static void AddPrivateRoutes(this WebApplication app)

    {
        var PrivateRoutes = app.MapGroup(prefix: "account");

        PrivateRoutes.MapGet(pattern: "/balance/{accountNumber}", handler: async (string accountNumber, AppDbContext context) =>
                {

                    var account = await context.Accounts.FirstOrDefaultAsync(acount => acount.AccountNumber == accountNumber);
                    if (account != null)
                    {
                        return Results.Ok(new AccountDto(account.Name, account.AccountNumber, account.Balance));
                    }
                    else
                    {
                        return Results.Conflict(error: "Conta inválida");
                    }
                });

        PrivateRoutes.MapGet(pattern: "", handler: async (AppDbContext context, CancellationToken ct) =>
        {
            var account = await context.Accounts.ToListAsync(ct);

            return account;
        });

        PrivateRoutes.MapPut(pattern: "/deposit", handler: async (AppDbContext context, DepositAccountRequest request, CancellationToken ct) =>
        {
            var account = await context.Accounts.FirstOrDefaultAsync(account => account.AccountNumber == request.account);
            if (account == null)
                return Results.Conflict(error: "A conta para depósito deve ser informada!");

            account.updateBalance(request.value);
            await context.SaveChangesAsync();

            return Results.Ok(new AccountDto(account.Name, account.AccountNumber, account.Balance));

        });

        PrivateRoutes.MapPut(pattern: "/withdrawal/{accountNumber}", handler: async (string accountNumber, AppDbContext context, withdrawalAccountRequest request, CancellationToken ct) =>
        {
            var account = await context.Accounts.SingleOrDefaultAsync(account => account.AccountNumber == accountNumber);

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

        PrivateRoutes.MapDelete(pattern: "/closeAccount/{accountNumber}", handler: async (string accountNumber, AppDbContext context, CancellationToken ct) =>
        {
            var account = await context.Accounts.FirstOrDefaultAsync(account => account.AccountNumber == accountNumber, ct);

            if (account != null)
            {

                if (account.Balance == 0)
                {
                    context.Accounts.Remove(account);

                    await context.SaveChangesAsync(ct);

                }
                else if (account.Balance < 0)
                {
                    return Results.Conflict(error: "O saldo não pode estar negativo. Realize regularização das pendências antes de fechar a conta!");
                }
                else
                {
                    return Results.Conflict(error: "Não foi possível fechar a conta devido ao saldo estar positivo. Faça um saque antes do fechamento!");
                }

            }

            return Results.Ok("Conta encerrada com sucesso!");

        });
    }
}