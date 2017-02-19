//-----------------------------------------------------------------------
// <copyright file="HW_MessageController.cs" company="Kenneth Larimer">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Controllers
{
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Web.Http;
    using API.Library.APIAttributes;
    using API.Library.APIResources;
    using API.Library.APIModels;
    using API.Library.APIServices;


    /// <summary>
    ///     API controller for getting and setting today's value.
    /// </summary>
    [WebApiExceptionFilter]
    public class HW_MessageController : ApiController
    {
        /// <summary>
        ///     The data service
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HW_MessageController" /> class.
        /// </summary>
        /// <param name="dataService">The injected data service</param>
        public HW_MessageController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        ///     Gets HW_Message value
        /// </summary>
        /// <returns>A HW_Message model containing HW_Message value</returns>
        [WebApiExceptionFilter(Type = typeof(IOException), Status = HttpStatusCode.ServiceUnavailable, Severity = SeverityCode.Error)]
        [WebApiExceptionFilter(Type = typeof(SettingsPropertyNotFoundException), Status = HttpStatusCode.ServiceUnavailable, Severity = SeverityCode.Error)]
        public HW_Message Get()
        {
            return this.dataService.GetHW_Message();
        }
    }
}
