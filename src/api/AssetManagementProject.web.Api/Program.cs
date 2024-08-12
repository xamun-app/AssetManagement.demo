﻿using System;
using System.IO;
using AssetManagementProject.web.Api.Settings;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

/// <summary>
/// Designed by BlastAsia, Inc.
/// https://www.blastasia.com
///
/// NOTE:
/// Must update database connection in appsettings.json - "AssetManagementProject.web.ApiDB"
/// </summary>

namespace AssetManagementProject.web.Api
{
    public class Program
    {

        private static string _environmentName;

        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            //read configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{_environmentName}.json", optional: true, reloadOnChange: true)
                        .Build();

            //Must have Loggly account and setup correct info in appsettings
            if (configuration["Serilog:UseLoggly"] == "true")
            {
                var logglySettings = new LogglySettings();
                configuration.GetSection("Serilog:Loggly").Bind(logglySettings);
                SetupLogglyConfiguration(logglySettings);
            }

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            //Start webHost
            try
            {
                Log.Information("Starting web host");
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();  //Disabling default integrated logger
                    _environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });


        /// <summary>
        /// Configure Loggly provider
        /// </summary>
        /// <param name="logglySettings"></param>
        private static void SetupLogglyConfiguration(LogglySettings logglySettings)
        {
            //Configure Loggly
            var config = LogglyConfig.Instance;
            config.CustomerToken = logglySettings.CustomerToken;
            config.ApplicationName = logglySettings.ApplicationName;
            config.Transport = new TransportConfiguration()
            {
                EndpointHostname = logglySettings.EndpointHostname,
                EndpointPort = logglySettings.EndpointPort,
                LogTransport = logglySettings.LogTransport
            };
            config.ThrowExceptions = logglySettings.ThrowExceptions;

            //Define Tags sent to Loggly
            config.TagConfig.Tags.AddRange(new ITag[]{
                new ApplicationNameTag {Formatter = "Application-{0}"},
                new HostnameTag { Formatter = "Host-{0}" }
            });
        }


    }
}

