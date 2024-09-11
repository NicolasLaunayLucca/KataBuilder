using Microsoft.EntityFrameworkCore;

namespace KataBuilder;

public interface IDatabaseSeeder
{
    Task SeedAsync(DbContext dbContext);
}