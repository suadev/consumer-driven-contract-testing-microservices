using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Provider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ProductDBContext>(options => options
                                .UseNpgsql(hostContext.Configuration.GetConnectionString("ProductDBConnStr"))
                                .UseSnakeCaseNamingConvention())
                            .AddControllers();
                })
                .Configure(app =>
                {
                    app.UseDeveloperExceptionPage()
                        .UseRouting()
                        .UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });

                    DbInitilializer.Migrate(app.ApplicationServices);
                }));
    }
}
