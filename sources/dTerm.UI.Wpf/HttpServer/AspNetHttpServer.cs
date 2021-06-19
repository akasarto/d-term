using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.HttpServer
{
    public static class AspNetHttpServer
    {
        public static void StartAsync()
        {
            var args = Environment.GetCommandLineArgs();

            var server = WebHost.CreateDefaultBuilder(args).UseKestrel(x =>
            {
                x.ListenAnyIP(5075);
                x.ListenLocalhost(5075);
            }).UseStartup<AspNetConfigs>().Build();

            _ = Task.Run(() =>
            {
                server.RunAsync();
            });
        }
    }
}
