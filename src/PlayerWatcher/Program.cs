using Grains.Interfaces;
using LoadGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using PlayerWatcher;

Console.Title = nameof(PlayerWatcher);

await new HostBuilder().ConfigureServices(services =>
{
    // add regular services
    services.AddTransient<IGameObserver, LoggerGameObserver>();
    
    // this hosted service connects and disconnects from the cluster along with the host
    // it also exposes the cluster client to other services that request it
    services.AddSingleton<ClusterClientHostedService>();
    services.AddSingleton<IHostedService>(sp => sp.GetService<ClusterClientHostedService>());
    services.AddSingleton<IClusterClient>(sp => sp.GetService<ClusterClientHostedService>()?.Client);

    // this hosted service runs the sample logic
    services.AddSingleton<IHostedService, PlayerWatcherHostedService>();
})
.ConfigureLogging(builder =>
{
    builder.AddConsole();
})
.RunConsoleAsync();