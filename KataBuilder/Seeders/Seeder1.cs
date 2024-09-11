using Microsoft.EntityFrameworkCore;

namespace KataBuilder.Seeders;

public class Seeder1 : IDatabaseSeeder
{
    public async Task SeedAsync(DbContext dbContext)
    {
        // Création des AppInstances
        // Création des Establishments
        // Création des Departments
        // Création des Users

        //dbContext.SaveChangesAsync();
        Console.WriteLine("Seeder1");
    }
}