// See https://aka.ms/new-console-template for more information

using KataBuilder;
using KataBuilder.Seeders;
using Microsoft.EntityFrameworkCore;

var builder =
    ApiTestConfigurationBuilder
        .ApiTestConfiguration()
        .ConfigureTestServer()
        .WithDbSnapshot()
        .ConfigureDbContext<DbContext>()
        .SeedGlobalDatabase<DefaultSeeder>()
        .SeedTagDatabase<Seeder1>("tag1")
        .SeedTagDatabase<Seeder1>("tag2")
        .SeedTagDatabase<Seeder2>("tag2")
        ;

Console.WriteLine("Before Register");

await builder.RegisterAsync();

Console.WriteLine("After Register");