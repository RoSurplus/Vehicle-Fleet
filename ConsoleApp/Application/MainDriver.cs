//-----------------------------------------------------------------------
// <copyright file="MainDriver.cs" company="Ryan Woodcox">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Ryan Woodcox</author>
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleApp.Application
{
    using global::ConsoleApp.Services;
    using API.Library.APIServices;
    using API.Library.APIWrappers;
    using LightInject;
    using RestSharp;
    using System;

    /// <summary>
    ///     Main class that drives the application
    /// </summary>
    public class MainDriver
    {
        /// <summary>
        ///     Starts the console application with the specified command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            // Setup dependency injection and run the application
            using (var container = new ServiceContainer())
            {
                // Configure depenency injection
                container.Register<IConsoleApp, ConsoleApp>();
                container.Register<IAppSettings, ConfigAppSettings>();
                container.Register<IConsole, SystemConsole>();
                container.Register<ILogger, ConsoleLogger>();
                container.Register<IUri, SystemUri>();
                container.Register<IHW_WebService, HW_WebService>();
                container.RegisterInstance(typeof(IRestClient), new RestClient());
                container.RegisterInstance(typeof(IRestRequest), new RestRequest());

                // Run the main program
                container.GetInstance<IConsoleApp>().Run(args);
            }

            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();
        }
    }
}