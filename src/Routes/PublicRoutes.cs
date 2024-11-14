using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;

public static class PublicRoutes
{
    public static void AddPublicRoutes(this WebApplication app)

    {
        var AccountRoutes = app.MapGroup(prefix: "account");

        AccountRoutes.MapPost(pattern: "", handler: async (AppDbContext context, AddAccountRequest request) =>
        {
            if (request.name == null || request.password == null || request.confirmPassword == null)

                return Results.Conflict(error: "Todos os campos devem ser preenchidos");

            if (request.password.ToLower().Trim() != request.confirmPassword.ToLower().Trim())
                return Results.Conflict(error: "As senhas devem ser iguais");

            bool AccountExists = await context.Accounts.AnyAsync(account => account.Name.ToLower() == request.name.ToLower());

            if (!AccountExists)

            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.password);

                var newAccount = new Account(request.name, hashPassword);

                await context.AddRangeAsync(newAccount);

                await context.SaveChangesAsync();

                return Results.Ok(new AccountDto(newAccount.Name, newAccount.AccountNumber, newAccount.Balance));
            }
            else
            {
                return Results.Conflict(error: "Conta já existe");
            }

        });

        AccountRoutes.MapPost(pattern: "/login", handler: async (AppDbContext context, AccountLoginRequest request, TokenService service) =>
        {
            var user = await context.Accounts.FirstOrDefaultAsync(account => account.AccountNumber == request.accountNumber);

            if (user == null)
                return Results.Conflict(error: "Todos os campos devem ser preenchidos");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.password, user.Password);
            if (!isValidPassword)
                return Results.Unauthorized();

            var token = service.Generate(user);

            return Results.Ok(token);
        });



    }
}
