using Microsoft.EntityFrameworkCore;

public static class AccountRoutes
{
    public static void AddAccountRoutes(this WebApplication app)

    {
        var AccountRoutes = app.MapGroup(prefix: "account");

        // Post

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

                return Results.Ok(newAccount);

            }
            else
            {
                return Results.Conflict(error: "Conta já existe");
            }

        });

        // Get

        AccountRoutes.MapGet(pattern: "", handler: async (AppDbContext context, CancellationToken ct) =>
           {
               var account = await context.Accounts.ToListAsync(ct);

               return account;
           });

        

    }
}
