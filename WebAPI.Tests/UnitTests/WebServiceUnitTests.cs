﻿//-----------------------------------------------------------------------
// <copyright file="WebServiceUnitTests.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace WebAPI.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using ConsoleApp.Services;
    using API.Library.APIModels;
    using API.Library.APIResources;
    using API.Library.APIServices;
    using Moq;
    using NUnit.Framework;
    using RestSharp;
    using API.Library.APIWrappers;

    /// <summary>
    ///     Unit tests for the Console App
    /// </summary>
    [TestFixture]
    public class WebServiceUnitTests
    {
        /// <summary>
        ///     The list of log messages set by calling classes
        /// </summary>
        private List<string> logMessageList;

        /// <summary>
        ///     The list of exceptions set by calling classes
        /// </summary>
        private List<Exception> exceptionList;

        /// <summary>
        ///     The list of other properties set by calling classes
        /// </summary>
        private List<object> otherPropertiesList;

        /// <summary>
        ///     The mocked application settings service
        /// </summary>
        private Mock<IAppSettings> appSettingsMock;

        /// <summary>
        ///     The test logger
        /// </summary>
        private ILogger testLogger;

        /// <summary>
        ///     The mocked Rest client
        /// </summary>
        private Mock<IRestClient> restClientMock;

        /// <summary>
        ///     The mocked Rest request
        /// </summary>
        private Mock<IRestRequest> restRequestMock;

        /// <summary>
        ///     The mocked wrapped Uri service
        /// </summary>
        private Mock<IUri> uriServiceMock;

        /// <summary>
        ///     The implementation to test
        /// </summary>
        private HW_WebService HW_WebService;

        /// <summary>
        ///     Initialize the test fixture (runs one time)
        /// </summary>
        [TestFixtureSetUp]
        public void InitTestSuite()
        {
            // Instantiate lists
            this.logMessageList = new List<string>();
            this.exceptionList = new List<Exception>();
            this.otherPropertiesList = new List<object>();

            // Setup mocked dependencies
            this.appSettingsMock = new Mock<IAppSettings>();
            this.testLogger = new TestLogger(ref this.logMessageList, ref this.exceptionList, ref this.otherPropertiesList);
            this.restClientMock = new Mock<IRestClient>();
            this.restRequestMock = new Mock<IRestRequest>();
            this.uriServiceMock = new Mock<IUri>();

            // Create object to test
            this.HW_WebService = new HW_WebService(
                this.restClientMock.Object,
                this.restRequestMock.Object,
                this.appSettingsMock.Object,
                this.uriServiceMock.Object,
                this.testLogger);
        }

        /// <summary>
        ///     Test tear down. (runs after each test)
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clear lists
            this.logMessageList.Clear();
            this.exceptionList.Clear();
            this.otherPropertiesList.Clear();
        }

        #region GetTodaysData Tests
        /// <summary>
        ///     Tests the class's GetTodaysData method for success when normal data was found
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNormalDataSuccess()
        {
            // Create return models for dependencies
            const string Data = "Hello There3, World!";
            const string WebApiIUrl = "http://www.somesiteheretesting.com";
            var uri = new Uri(WebApiIUrl);
            var mockParameters = new Mock<List<Parameter>>();
            var mockRestResponse = new Mock<IRestResponse<HW_Message>>();
            var todaysData = GetSampleHW_Message(Data);

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(WebApiIUrl);
            this.uriServiceMock.Setup(m => m.GetUri(WebApiIUrl)).Returns(uri);
            this.restRequestMock.Setup(m => m.Parameters).Returns(mockParameters.Object);
            this.restClientMock.Setup(m => m.Execute<HW_Message>(It.IsAny<IRestRequest>())).Returns(mockRestResponse.Object);
            mockRestResponse.Setup(m => m.Data).Returns(todaysData);

            // Call the method to test
            var response = this.HW_WebService.GetHW_Message();

            // Check values
            Assert.NotNull(response);
            Assert.AreEqual(response.Data, todaysData.Data);
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method for success when there is a null response
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNormalDataNullResponse()
        {
            // Create return models for dependencies
            const string Data = "Hello There4, World!";
            const string WebApiIUrl = "http://www.somesiteheretesting.com";
            var uri = new Uri(WebApiIUrl);
            var mockParameters = new Mock<List<Parameter>>();
            var mockRestResponse = (IRestResponse<HW_Message>)null;
            var todaysData = GetSampleHW_Message(Data);
            const string ErrorMessage = "Did not get any response from the Web Api for the Method: GET /todaysdata";

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(WebApiIUrl);
            this.uriServiceMock.Setup(m => m.GetUri(WebApiIUrl)).Returns(uri);
            this.restRequestMock.Setup(m => m.Parameters).Returns(mockParameters.Object);
            this.restClientMock.Setup(m => m.Execute<HW_Message>(It.IsAny<IRestRequest>())).Returns(mockRestResponse);

            // Call the method to test
            var response = this.HW_WebService.GetHW_Message();

            // Check values
            Assert.IsNull(response);
            Assert.AreEqual(this.logMessageList.Count, 1);
            Assert.AreEqual(this.logMessageList[0], ErrorMessage);
            Assert.AreEqual(this.exceptionList.Count, 1);
            Assert.AreEqual(this.exceptionList[0].Message, ErrorMessage);
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method for success when there is null data in the response
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNormalDataNullData()
        {
            // Create return models for dependencies
            const string WebApiIUrl = "http://www.somesiteheretesting.com";
            var uri = new Uri(WebApiIUrl);
            var mockParameters = new Mock<List<Parameter>>();
            var mockRestResponse = new Mock<IRestResponse<HW_Message>>();
            HW_Message todaysData = null;
            const string ErrorMessage = "Error Message";
            const HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
            const string StatusDescription = "Status Description";
            var errorException = new Exception("errorHere");
            const string ProfileContent = "Content here";

            var errorMessage = "Error in RestSharp, most likely in endpoint URL." 
                + " Error message: " + ErrorMessage 
                + " HTTP Status Code: " + StatusCode 
                + " HTTP Status Description: " + StatusDescription;

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(WebApiIUrl);
            this.uriServiceMock.Setup(m => m.GetUri(WebApiIUrl)).Returns(uri);
            this.restRequestMock.Setup(m => m.Parameters).Returns(mockParameters.Object);
            this.restClientMock.Setup(m => m.Execute<HW_Message>(It.IsAny<IRestRequest>())).Returns(mockRestResponse.Object);
            mockRestResponse.Setup(m => m.Data).Returns(todaysData);
            mockRestResponse.Setup(m => m.ErrorMessage).Returns(ErrorMessage);
            mockRestResponse.Setup(m => m.StatusCode).Returns(StatusCode);
            mockRestResponse.Setup(m => m.StatusDescription).Returns(StatusDescription);
            mockRestResponse.Setup(m => m.ErrorException).Returns(errorException);
            mockRestResponse.Setup(m => m.Content).Returns(ProfileContent);

            // Call the method to test
            var response = this.HW_WebService.GetHW_Message();

            // Check values
            Assert.IsNull(response);
            Assert.AreEqual(this.logMessageList.Count, 1);
            Assert.AreEqual(this.logMessageList[0], errorMessage);
            Assert.AreEqual(this.exceptionList.Count, 1);
            Assert.AreEqual(this.exceptionList[0].Message, errorException.Message);
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method for success when there is null data in the response and a null error message
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNormalDataNullDataNullErrorMessage()
        {
            // Create return models for dependencies
            const string WebApiIUrl = "http://www.somesiteheretesting.com";
            var uri = new Uri(WebApiIUrl);
            var mockParameters = new Mock<List<Parameter>>();
            var mockRestResponse = new Mock<IRestResponse<HW_Message>>();
            HW_Message todaysData = null;
            const string ErrorMessage = null;
            const HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
            const string StatusDescription = "Status Description";
            var errorException = new Exception("errorHere");
            const string ProfileContent = "Content here";

            var errorMessage = "Error in RestSharp, most likely in endpoint URL."
                + " Error message: " + ErrorMessage
                + " HTTP Status Code: " + StatusCode
                + " HTTP Status Description: " + StatusDescription;

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(WebApiIUrl);
            this.uriServiceMock.Setup(m => m.GetUri(WebApiIUrl)).Returns(uri);
            this.restRequestMock.Setup(m => m.Parameters).Returns(mockParameters.Object);
            this.restClientMock.Setup(m => m.Execute<HW_Message>(It.IsAny<IRestRequest>())).Returns(mockRestResponse.Object);
            mockRestResponse.Setup(m => m.Data).Returns(todaysData);
            mockRestResponse.Setup(m => m.ErrorMessage).Returns(ErrorMessage);
            mockRestResponse.Setup(m => m.StatusCode).Returns(StatusCode);
            mockRestResponse.Setup(m => m.StatusDescription).Returns(StatusDescription);
            mockRestResponse.Setup(m => m.ErrorException).Returns(errorException);
            mockRestResponse.Setup(m => m.Content).Returns(ProfileContent);

            // Call the method to test
            var response = this.HW_WebService.GetHW_Message();

            // Check values
            Assert.IsNull(response);
            Assert.AreEqual(this.logMessageList.Count, 1);
            Assert.AreEqual(this.logMessageList[0], errorMessage);
            Assert.AreEqual(this.exceptionList.Count, 1);
            Assert.AreEqual(this.exceptionList[0].Message, ProfileContent);
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method for success when there is null data in the response and a null error exception
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNormalDataNullDataNullErrorException()
        {
            // Create return models for dependencies
            const string WebApiIUrl = "http://www.somesiteheretesting.com";
            var uri = new Uri(WebApiIUrl);
            var mockParameters = new Mock<List<Parameter>>();
            var mockRestResponse = new Mock<IRestResponse<HW_Message>>();
            HW_Message todaysData = null;
            const string ErrorMessage = "Error Message";
            const HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
            const string StatusDescription = "Status Description";
            Exception errorException = null;
            const string ProfileContent = "Content here";

            var errorMessage = "Error in RestSharp, most likely in endpoint URL."
                + " Error message: " + ErrorMessage
                + " HTTP Status Code: " + StatusCode
                + " HTTP Status Description: " + StatusDescription;

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(WebApiIUrl);
            this.uriServiceMock.Setup(m => m.GetUri(WebApiIUrl)).Returns(uri);
            this.restRequestMock.Setup(m => m.Parameters).Returns(mockParameters.Object);
            this.restClientMock.Setup(m => m.Execute<HW_Message>(It.IsAny<IRestRequest>())).Returns(mockRestResponse.Object);
            mockRestResponse.Setup(m => m.Data).Returns(todaysData);
            mockRestResponse.Setup(m => m.ErrorMessage).Returns(ErrorMessage);
            mockRestResponse.Setup(m => m.StatusCode).Returns(StatusCode);
            mockRestResponse.Setup(m => m.StatusDescription).Returns(StatusDescription);
            mockRestResponse.Setup(m => m.ErrorException).Returns(errorException);
            mockRestResponse.Setup(m => m.Content).Returns(ProfileContent);

            // Call the method to test
            var response = this.HW_WebService.GetHW_Message();

            // Check values
            Assert.IsNull(response);
            Assert.AreEqual(this.logMessageList.Count, 1);
            Assert.AreEqual(this.logMessageList[0], errorMessage);
            Assert.AreEqual(this.exceptionList.Count, 1);
            Assert.AreEqual(this.exceptionList[0].Message, ProfileContent);
        }
        #endregion
        
        #region Helper Methods
        /// <summary>
        ///     Gets a sample TodaysData model
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>A sample TodaysData model</returns>
        private static HW_Message GetSampleHW_Message(string data)
        {
            return new HW_Message { Data = data };
        }
        #endregion
    }
}