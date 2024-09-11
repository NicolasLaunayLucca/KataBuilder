namespace KataBuilder;

public interface IApiTestConfigurationBuilder
{
    public interface IRoot : ITestServerConfigurationBuilder.IRoot;
    public interface IEnd
    {
         Task RegisterAsync();
    }
} 
public interface ITestServerConfigurationBuilder
{
    public interface IRoot
    {
        IEnd ConfigureTestServer();
    }
    public interface IEnd : IDatabaseConfigurationBuilder.IRoot;
}
public  interface IDatabaseConfigurationBuilder
{
    public interface IRoot
    {
        IEnd ConfigureDatabase();
    }
    public interface IEnd:ISeedDatabaseBuilder.IRoot,
        IApiTestConfigurationBuilder.IEnd;
}
public  interface ISeedDatabaseBuilder
{
    public interface IRoot
    {
        IEnd SeedDatabase();
    }
    public interface IEnd : IApiTestConfigurationBuilder.IEnd;
}

public class ApiTestConfigurationBuilder : IApiTestConfigurationBuilder.IRoot, IApiTestConfigurationBuilder.IEnd,
    ITestServerConfigurationBuilder.IRoot, ITestServerConfigurationBuilder.IEnd,
    IDatabaseConfigurationBuilder.IRoot, IDatabaseConfigurationBuilder.IEnd,
    ISeedDatabaseBuilder.IRoot, ISeedDatabaseBuilder.IEnd
{
    private string? _testServerMessage;
    private string? _testDatabaseMessage;
    private string? _testSeedMessage;

    private ApiTestConfigurationBuilder()
    {}
    
    public static IApiTestConfigurationBuilder.IRoot ApiTestConfiguration() => new ApiTestConfigurationBuilder();
        
    
    public ITestServerConfigurationBuilder.IEnd ConfigureTestServer()
    { 
        _testServerMessage = "Test Server Configured";
        return this;
    }

    public IDatabaseConfigurationBuilder.IEnd ConfigureDatabase()
    {
        _testDatabaseMessage = "Database Configured";
        return this;
    }

    public ISeedDatabaseBuilder.IEnd SeedDatabase()
    {
        _testSeedMessage = "Database seeded";
        return this;
    }
    
    public async Task RegisterAsync()
    {
        await Task.CompletedTask;
        if (_testServerMessage != null) Console.WriteLine(_testServerMessage);
        if (_testDatabaseMessage != null) Console.WriteLine(_testDatabaseMessage);
        if (_testSeedMessage != null) Console.WriteLine(_testSeedMessage);
    }
}