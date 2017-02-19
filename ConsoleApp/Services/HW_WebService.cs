//-----------------------------------------------------------------------
// <copyright file="HW_WebService.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleApp.Services
{
    using System;
    using API.Library.APIWrappers;
    using API.Library.APIModels;
    using API.Library.APIResources;
    using API.Library.APIServices;
    using RestSharp;

    /// <summary>
    ///     Service class for communicating with the Web API
    /// </summary>
    public class HW_WebService : IHW_WebService
    {
        /// <summary>
        ///     The application settings service
        /// </summary>
        private readonly IAppSettings appSettings;

        /// <summary>
        ///     The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        ///     The Rest client
        /// </summary>
        private readonly IRestClient restClient;

        /// <summary>
        ///     The Rest request
        /// </summary>
        private readonly IRestRequest restRequest;

        /// <summary>
        ///     The wrapped Uri service
        /// </summary>
        private readonly IUri uriService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HW_WebService" /> class.
        /// </summary>
        /// <param name="restClient">The rest client</param>
        /// <param name="restRequest">The rest request</param>
        /// <param name="appSettings">The application settings</param>
        /// <param name="uriService">The uri service</param>
        /// <param name="logger">The logger</param>
        public HW_WebService(
            IRestClient restClient,
            IRestRequest restRequest,
            IAppSettings appSettings,
            IUri uriService,
            ILogger logger)
        {
            this.restClient = restClient;
            this.restRequest = restRequest;
            this.appSettings = appSettings;
            this.uriService = uriService;
            this.logger = logger;
        }

        /// <summary>
        ///     Gets today's data from the web API
        /// </summary>
        /// <returns>A TodaysData model containing today's data</returns>
        public HW_Message GetHW_Message()
        {
            HW_Message hw_MessageData = null;

            // Set the URL for the request
            this.restClient.BaseUrl = this.uriService.GetUri(this.appSettings.Get(AppSettingsKeys.WebAPIUrlKey));

            // Setup the request
            this.restRequest.Resource = "hw_message";
            this.restRequest.Method = Method.GET;

            // Clear the request parameters
            this.restRequest.Parameters.Clear();

            // Execute the call and get the response
            var hw_MessageDataResponse = this.restClient.Execute<HW_Message>(this.restRequest);

            // Check for data in the response
            if (hw_MessageDataResponse != null)
            {
                // Check if any actual data was returned
                if (hw_MessageDataResponse.Data != null)
                {
                    hw_MessageData = hw_MessageDataResponse.Data;
                }
                else
                {
                    var errorMessage = "Error in RestSharp, most likely in endpoint URL." + " Error message: "
                                       + hw_MessageDataResponse.ErrorMessage + " HTTP Status Code: "
                                       + hw_MessageDataResponse.StatusCode + " HTTP Status Description: "
                                       + hw_MessageDataResponse.StatusDescription;

                    // Check for existing exception
                    if (hw_MessageDataResponse.ErrorMessage != null && hw_MessageDataResponse.ErrorException != null)
                    {
                        // Log an informative exception including the RestSharp exception
                        this.logger.Error(errorMessage, null, hw_MessageDataResponse.ErrorException);
                    }
                    else
                    {
                        // Log an informative exception including the RestSharp content
                        this.logger.Error(errorMessage, null, new Exception(hw_MessageDataResponse.Content));
                    }
                }
            }
            else
            {
                // Log the exception
                const string ErrorMessage =
                    "Did not get any response from the Web Api for the Method: GET /todaysdata";

                this.logger.Error(ErrorMessage, null, new Exception(ErrorMessage));
            }

            return hw_MessageData;
        }

    }
}