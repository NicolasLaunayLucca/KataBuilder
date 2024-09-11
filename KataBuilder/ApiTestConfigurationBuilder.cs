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

    public interface IEnd : IDatabaseEmptyOrBackupConfigurationBuilder.IRoot, IDatabaseSnapshotConfigurationBuilder.IRoot;
}

public interface IDatabaseEmptyOrBackupConfigurationBuilder
{
    public interface IRoot
    {
        IEnd WithDbEmpty();

        IEnd WithDbBackup(string backup);
    }

    public interface IEnd : IMigrationConfigurationBuilder.IRoot;
}

public interface IMigrationConfigurationBuilder
{
    public interface IRoot
    {
        IEnd MigrateDatabase();
    }

    public interface IEnd : IDatabaseConfigurationBuilder.IRoot;
}

public interface IDatabaseSnapshotConfigurationBuilder
{
    public interface IRoot
    {
        IEnd WithDbSnapshot();
    }

    public interface IEnd : IDatabaseConfigurationBuilder.IRoot;
}

public interface IDatabaseConfigurationBuilder
{
    public interface IRoot
    {
        IEnd ConfigureDbContext<T>();
    }

    public interface IEnd : ISeedDatabaseBuilder.IRoot,
        IApiTestConfigurationBuilder.IEnd;
}

public interface ISeedDatabaseBuilder
{
    public interface IRoot
    {
        IEnd SeedGlobalDatabase<T>() where T : IDatabaseSeeder, new();

        IEnd SeedTagDatabase<T>(string tag) where T : IDatabaseSeeder, new();
    }

    public interface IEnd : IApiTestConfigurationBuilder.IEnd, ISeedDatabaseBuilder.IRoot;
}

public class ApiTestConfigurationBuilder : IApiTestConfigurationBuilder.IRoot, IApiTestConfigurationBuilder.IEnd,
    ITestServerConfigurationBuilder.IRoot, ITestServerConfigurationBuilder.IEnd,
    IDatabaseConfigurationBuilder.IRoot, IDatabaseConfigurationBuilder.IEnd,
    ISeedDatabaseBuilder.IRoot, ISeedDatabaseBuilder.IEnd,
    IDatabaseEmptyOrBackupConfigurationBuilder.IRoot, IDatabaseEmptyOrBackupConfigurationBuilder.IEnd,
    IMigrationConfigurationBuilder.IRoot, IMigrationConfigurationBuilder.IEnd,
    IDatabaseSnapshotConfigurationBuilder.IRoot, IDatabaseSnapshotConfigurationBuilder.IEnd
{
    private string? _testServerMessage;
    private string? _testDatabaseInitmethode;
    private string? _testMigrationMessage;
    private string? _dbContextMessage;
    private List<IDatabaseSeeder> _seedersGlobaux = new List<IDatabaseSeeder>(); // [DefaultSeeder, UserASeeder, FactureSeeder]
    private Dictionary<string, List<IDatabaseSeeder>> _seedersParTag = new Dictionary<string, List<IDatabaseSeeder>>();

    private ApiTestConfigurationBuilder()
    {
    }

    public static IApiTestConfigurationBuilder.IRoot ApiTestConfiguration()
    {
        return new ApiTestConfigurationBuilder();
    }

    public ITestServerConfigurationBuilder.IEnd ConfigureTestServer()
    {
        _testServerMessage = "Test Server Configured";
        return this;
    }

    public IDatabaseSnapshotConfigurationBuilder.IEnd WithDbSnapshot()
    {
        _testDatabaseInitmethode = "initialise DB with Snapshot";
        return this;
    }

    public IMigrationConfigurationBuilder.IEnd MigrateDatabase()
    {
        _testMigrationMessage = "Run Migration 100%";
        return this;
    }

    public IDatabaseEmptyOrBackupConfigurationBuilder.IEnd WithDbEmpty()
    {
        _testDatabaseInitmethode = "initialise DB with Empty DB";
        return this;
    }

    public IDatabaseEmptyOrBackupConfigurationBuilder.IEnd WithDbBackup(string backup)
    {
        _testDatabaseInitmethode = $"initialise DB From backup {backup}";
        return this;
    }

    public IDatabaseConfigurationBuilder.IEnd ConfigureDbContext<T>()
    {
        _dbContextMessage = typeof(T).Name;
        return this;
    }

    public async Task RegisterAsync()
    {
        await Task.CompletedTask;
        if (_testServerMessage != null) Console.WriteLine(_testServerMessage);
        if (_testDatabaseInitmethode != null) Console.WriteLine(_testDatabaseInitmethode);
        if (_testMigrationMessage != null) Console.WriteLine(_testMigrationMessage);
        if (_dbContextMessage != null) Console.WriteLine(_dbContextMessage);
        foreach (var seeder in _seedersGlobaux)
        {
            await seeder.SeedAsync(null);
        }
        foreach (var (tag, seeders) in _seedersParTag)
        {
            Console.WriteLine($"-- Tag: {tag}");
            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(null);
            }
        }
    }

    public ISeedDatabaseBuilder.IEnd SeedGlobalDatabase<T>() where T : IDatabaseSeeder, new()
    {
        _seedersGlobaux.Add(new T());
        return this;
    }

    public ISeedDatabaseBuilder.IEnd SeedTagDatabase<T>(string tag) where T : IDatabaseSeeder, new()
    {
        if (_seedersParTag.ContainsKey(tag))
        {
            _seedersParTag[tag].Add(new T());
        }
        else
        {
            _seedersParTag.Add(tag, new List<IDatabaseSeeder> { new T() });
        }

        return this;
    }
}