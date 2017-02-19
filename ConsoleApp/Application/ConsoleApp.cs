//-----------------------------------------------------------------------
// <copyright file="ConsoleApp.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleApp.Application
{
    using global::ConsoleApp.Services;
    using API.Library.APIServices;

    /// <summary>
    ///     Console Application
    /// </summary>
    public class ConsoleApp : IConsoleApp
    {
        /// <summary>
        ///     The Web Service
        /// </summary>
        private readonly IHW_WebService WebService;

        /// <summary>
        ///     The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleApp" /> class.
        /// </summary>
        /// <param name="WebService">The injected web service</param>
        /// <param name="logger">The logger</param>
        public ConsoleApp(IHW_WebService WebService, ILogger logger)
        {
            this.WebService = WebService;
            this.logger = logger;
        }

        /// <summary>
        ///     Runs the main Console Application
        /// </summary>
        /// <param name="arguments">The command line arguments.</param>
        public void Run(string[] arguments)
        {
            //Get HW Message
            var hw_Message = this.WebService.GetHW_Message();

            //Write HW Message to the screen
            this.logger.Info(hw_Message != null ? hw_Message.Data : "No data was found!", null);
        }
    }
}