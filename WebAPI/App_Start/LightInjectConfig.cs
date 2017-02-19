//-----------------------------------------------------------------------
// <copyright file="LightInjectConfig.cs">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Http;
using API.Library.APIWrappers;
using API.Library.APIMappers;
using API.Library.APIServices;
using LightInject;

namespace SampleApp
{
    /// <summary>
    /// Configures dependency injection via LightInject
    /// </summary>
    public static class LightInjectConfig
    {
        /// <summary>
        /// Registers main components
        /// </summary>
        /// <param name="config">Http Configuration</param>
        public static void Register(HttpConfiguration config)
        {
            var container = new ServiceContainer();
            container.RegisterApiControllers();

            container.EnablePerWebRequestScope();
            container.EnableWebApi(GlobalConfiguration.Configuration);
            container.EnableMvc();

            // Register Services
            RegisterServices(container);
        }

        /// <summary>
        /// Registers the dependency services to be injected
        /// </summary>
        /// <param name="serviceRegistry">The Service Registry</param>
        private static void RegisterServices(IServiceRegistry serviceRegistry)
        {
            // Register default Application Settings Service
            serviceRegistry.Register<IAppSettings, ConfigAppSettings>();

            // Register default Logger Service
            ////serviceRegistry.Register<ILogger, JsonL4NLogger>();
            serviceRegistry.RegisterInstance(typeof(ILogger), new JsonL4NLogger());

            // Register default Hosting Environment Service
            serviceRegistry.Register<IHostingEnvironmentService, ServerHostingEnvironmentService>();

            // Register default File IO Service
            serviceRegistry.Register<IFileIOService, TextFileIOService>();

            // Register default Data Service
            serviceRegistry.Register<IDataService, HW_DataService>();

            // Register default DateTime wrapper
            serviceRegistry.Register<IDateTime, SystemDateTime>();

            // Register default Hello World mapper
            serviceRegistry.Register<IHW_Mapper, HW_Mapper>();
        }
    }
}
