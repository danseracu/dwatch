using dwatch.checker.Models.Options;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dwatch.checker.Constants;
using Microsoft.Extensions.Logging;
using Smdn.TPSmartHomeDevices.Tapo;
using System.Net;
using System.Security.Cryptography;
using static System.Threading.Tasks.Task;

namespace dwatch.checker
{
    public sealed class Worker(
        IHttpClientFactory httpFactory, 
        DeviceOptions deviceOpts,
        WatcherOptions watcherOpts) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) { 
                Console.WriteLine("Running check");

                try
                {
                    if (!await CheckHealthEndpoint(stoppingToken))
                    {
                        await PowerCycleDevice(stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception occurred: {e.Message}");

                    try
                    {
                        await PowerCycleDevice(stoppingToken);
                    }
                    catch (Exception e2)
                    {
                        RSA.Create()
                        Console.WriteLine(e2?.InnerException);
                        Console.WriteLine($"Exception occurred: {e2.Message}");
                    }
                }
                finally
                {
                    Console.WriteLine("Going to sleep");

                    //var sleepDuration = TimeSpan.FromMinutes(watcherOpts.CheckDelay);
                    var sleepDuration = TimeSpan.FromSeconds(15);

                    await Delay(sleepDuration, stoppingToken);
                }
            }
        }

        private async Task<bool> CheckHealthEndpoint(CancellationToken stoppingToken)
        {
            var client = httpFactory.CreateClient();

            Console.WriteLine("Retrieving http response");

            var response = await client.GetAsync(watcherOpts.HealthcheckUrl, stoppingToken);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Health endpoint returned: {response.StatusCode}");

                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);

            return responseContent == HealthcheckConstants.ExpectedResponse;
        }

        private async Task PowerCycleDevice(CancellationToken stoppingToken)
        {
            Console.WriteLine("Connecting to device");
            using var plug = new P110M(IPAddress.Parse(deviceOpts.IP), deviceOpts.Username, deviceOpts.Password);

            Console.WriteLine("Retrieving details");
            var device = await plug.GetDeviceInfoAsync(stoppingToken);

            if (device.IsOn)
            {
                Console.WriteLine("Turning device off");

                await plug.TurnOffAsync(stoppingToken);

                Console.WriteLine("Debounce sleep");

                await Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            Console.WriteLine("Turning device on");

            await plug.TurnOnAsync(stoppingToken);
        }
    }
}
