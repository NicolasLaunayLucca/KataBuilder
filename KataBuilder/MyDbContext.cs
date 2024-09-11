namespace KataBuilder;

public class MyDbContext
{
    public string SaveChangesAsync()
    {
        return "MyConnectionString";
    }
}

public class MySpecificDbContext : MyDbContext
{
    public string SaveChangesAsync()
    {
        return "SpecificConnectionString";
    }
}