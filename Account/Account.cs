using System.Xml;

public class Account 
	{
	    public Guid id {get; init;}
	
	    public string Name {get; private set;}

        public int AccountNumber {get; private set;}
        public string Password {get; private set;}
	    public bool Active {get; private set;}
	
	    public Account(string name,string password)
	    {
	        id = Guid.NewGuid();

	        Name = name;

            AccountNumber = 0;
	
	        Password = password;

            Active = true;

	    }
}