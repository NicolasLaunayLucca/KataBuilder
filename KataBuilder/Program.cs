// See https://aka.ms/new-console-template for more information

using KataBuilder;

var builder =
    ApiTestConfigurationBuilder
        .ApiTestConfiguration()
        .ConfigureTestServer()
        .ConfigureDatabase()
        .SeedDatabase();

Console.WriteLine("Before Register");

await builder.RegisterAsync();

Console.WriteLine("After Register");
