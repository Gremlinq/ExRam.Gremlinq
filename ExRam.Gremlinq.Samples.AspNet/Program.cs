using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ExRam.Gremlinq.Samples.AspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseStartup<Startup>()
                    .ConfigureKestrel(options =>
                    {
                        //This is done because we in GremlinqController.Index, we set the AutoFlush-property of the
                        //response stream writer to true. This is only for demonstration purposes and should not 
                        //be done in production code.
                        options.AllowSynchronousIO = true;
                    });
            });
    }
}
