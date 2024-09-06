using dwatch.checker.Constants;
using dwatch.checker.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dwatch.checker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var deviceOpts = new DeviceOptions
            {
                IP = Environment.GetEnvironmentVariable(EnvironmentConstants.DEVICE_IP),
                Username = Environment.GetEnvironmentVariable(EnvironmentConstants.USERNAME),
                Password = Environment.GetEnvironmentVariable(EnvironmentConstants.PASSWORD)
            };

            //if (string.IsNullOrEmpty(deviceOpts.Username) || string.IsNullOrEmpty(deviceOpts.Password)
            //    || string.IsNullOrEmpty(deviceOpts.IP))
            //{
            //    throw new InvalidOperationException("Username, password and IP are required to connect to device");
            //}

            var watcherOpts = new WatcherOptions();

            //if (Environment.GetEnvironmentVariable(EnvironmentConstants.HEALTHCHECK_URL) is { } url)
            //{
            //    watcherOpts.HealthcheckUrl = url;
            //}

            //if (int.TryParse(Environment.GetEnvironmentVariable(EnvironmentConstants.CHECK_TIME), out var time))
            //{
            //    watcherOpts.CheckDelay = time;
            //}

            var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings
            {

            });

            builder.Services.AddSingleton(deviceOpts);
            builder.Services.AddSingleton(watcherOpts);

            builder.Services.AddHttpClient();
            //builder.Services.AddLogging();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
