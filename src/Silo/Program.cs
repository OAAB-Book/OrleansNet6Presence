using Grains;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;

Console.Title = "Silo";

await new HostBuilder()
    .UseOrleans(builder =>
    {
        builder
            .UseLocalhostClustering()
            .ConfigureApplicationParts(manager =>
            {
                manager.AddApplicationPart(typeof(GameGrain).Assembly).WithReferences();
            });
    })
    .ConfigureLogging(builder =>
    {
        builder.AddConsole();
    })
    .RunConsoleAsync();