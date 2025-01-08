using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dotnet.Homeworks.Data.DatabaseContext;

public class EfFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().Build();
        
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        optionBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=Homeworks;");
        
        return new AppDbContext(optionBuilder.Options);
    }
}