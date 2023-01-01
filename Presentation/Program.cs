using AppCommand;
using Presentation.Programs;

namespace Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            CommandManager.SearchForCommands();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()){
                CommandManager.SetServiceProvider(scope.ServiceProvider);
                await CommandManager.InvokeCommand(args);
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                        .UseKestrel()
                );
    }
    
}