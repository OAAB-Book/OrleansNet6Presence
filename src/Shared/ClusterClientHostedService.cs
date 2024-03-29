﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace LoadGenerator;

public class ClusterClientHostedService : IHostedService
{
    private readonly ILogger<ClusterClientHostedService> _logger;
    public IClusterClient Client { get; }

    public ClusterClientHostedService(ILogger<ClusterClientHostedService> logger)
    {
        _logger = logger;
        Client = new ClientBuilder()
            .UseLocalhostClustering()
            .ConfigureLogging(_ => { _.AddConsole(); })
            .Build();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var attempt = 0;
        const int maxAttempts = 100;
        var delay = TimeSpan.FromSeconds(1);
        return Client.Connect(async error =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            if (++attempt < maxAttempts)
            {
                _logger.LogWarning(error,
                    "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                    attempt, maxAttempts);

                try
                {
                    await Task.Delay(delay, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return false;
                }

                return true;
            }
            else
            {
                _logger.LogError(error,
                    "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                    attempt, maxAttempts);

                return false;
            }
        });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Client.Close();
        }
        catch (OrleansException error)
        {
            _logger.LogWarning(error,
                "Error while gracefully disconnecting from Orleans cluster. Will ignore and continue to shutdown.");
        }
    }
}