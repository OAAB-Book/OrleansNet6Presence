using LoadGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

Console.Title = nameof(LoadGenerator);

await new HostBuilder()
.ConfigureServices(services =>
{
    // this hosted service connects and disconnects from the cluster along with the host
    // it also exposes the cluster client to other services that request it
    services.AddSingleton<ClusterClientHostedService>();
    services.AddSingleton<IHostedService>(sp => sp.GetService<ClusterClientHostedService>());
    services.AddSingleton<IClusterClient>(sp => sp.GetService<ClusterClientHostedService>()?.Client);

    // this hosted service run the load generation using the available cluster client
    services.AddSingleton<IHostedService, LoadGeneratorHostedService>();
})
.ConfigureLogging(builder =>
{
    builder.AddConsole();
})
.RunConsoleAsync();