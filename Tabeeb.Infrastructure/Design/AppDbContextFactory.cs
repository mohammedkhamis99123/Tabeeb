using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tabeeb.Infrastructure.DbContexts;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
           "server=.;Initial Catalog=TabeebDB;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;MultipleActiveResultSets=true;");

        return new AppDbContext(optionsBuilder.Options);
    }
}