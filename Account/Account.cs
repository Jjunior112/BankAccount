using System.Xml;

public class Account
{
    public Guid id { get; init; }

    public string Name { get; private set; }

    public string AccountNumber { get; private set; }
    public string Password { get; private set; }

    public double Balance { get; private set; }
    public bool Active { get; private set; }

    public Account(string name, string password)
    {
        id = Guid.NewGuid();

        Name = name;

        var randomAccountNumber = new Random().Next(10000, 99999);

        AccountNumber = randomAccountNumber.ToString();

        Balance = 0;

        Password = password;

        Active = true;

    }
    public void updateBalance(double value)
    {
        Balance += value;

    }

    public void DowngradeBalance(double value){
        Balance -= value;
    }
}