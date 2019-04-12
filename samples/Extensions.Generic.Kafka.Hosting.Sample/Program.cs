﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Kafka;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Extensions.Generic.Kafka.Hosting.Sample
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    hostContext.HostingEnvironment.ApplicationName = "Sample Hostbuilder Kafka Consumer Sample";
                    hostContext.HostingEnvironment.ContentRootPath = Directory.GetCurrentDirectory();
                })
                .UseKafka(config => // Equivalent to .UseKafka<string, byte[]>()
                {
                    // Configuration for the kafka consumer
                    config.BootstrapServers = new[] { "kafka:9092" };
                    config.Topics = new[] { "topic1" };
                    config.ConsumerGroup = "group3";
                    config.AutoOffsetReset = "Latest";
                    config.AutoCommitIntervall = 1000;
                    config.IsAutocommitEnabled = true;
                })
                .ConfigureServices(container =>
                {
                    // The message that matches the 
                    container.AddScoped<IMessageHandler<string, byte[]>, JobMessageHandler>();
                })
                .ConfigureLogging((ILoggingBuilder loggingBuilder) =>
                {
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
