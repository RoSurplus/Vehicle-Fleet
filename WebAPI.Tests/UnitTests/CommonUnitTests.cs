//-----------------------------------------------------------------------
// <copyright file="WebServiceUnitTests.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Tests.UnitTests
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
    using Models;
    using API.Library.Common;

    /// <summary>
    ///     Unit tests for the Console App
    /// </summary>
    [TestFixture]
    public class CommonUnitTests
    {
        ///// <summary>
        /////     The list of log messages set by calling classes
        ///// </summary>
        //private List<string> logMessageList;

        ///// <summary>
        /////     The list of exceptions set by calling classes
        ///// </summary>
        //private List<Exception> exceptionList;

        ///// <summary>
        /////     The list of other properties set by calling classes
        ///// </summary>
        //private List<object> otherPropertiesList;

        ///// <summary>
        /////     The mocked application settings service
        ///// </summary>
        //private Mock<IAppSettings> appSettingsMock;

        ///// <summary>
        /////     The test logger
        ///// </summary>
        //private ILogger testLogger;

        ///// <summary>
        /////     The mocked Rest client
        ///// </summary>
        //private Mock<IRestClient> restClientMock;

        ///// <summary>
        /////     The mocked Rest request
        ///// </summary>
        //private Mock<IRestRequest> restRequestMock;

        ///// <summary>
        /////     The mocked wrapped Uri service
        ///// </summary>
        //private Mock<IUri> uriServiceMock;

        ///// <summary>
        /////     The implementation to test
        ///// </summary>
        //private HW_WebService HW_WebService;

        /// <summary>
        ///     Initialize the test fixture (runs one time)
        /// </summary>
        [TestFixtureSetUp]
        public void InitTestSuite()
        {
            //// Instantiate lists
            //this.logMessageList = new List<string>();
            //this.exceptionList = new List<Exception>();
            //this.otherPropertiesList = new List<object>();

            //// Setup mocked dependencies
            //this.appSettingsMock = new Mock<IAppSettings>();
            //this.testLogger = new TestLogger(ref this.logMessageList, ref this.exceptionList, ref this.otherPropertiesList);
            //this.restClientMock = new Mock<IRestClient>();
            //this.restRequestMock = new Mock<IRestRequest>();
            //this.uriServiceMock = new Mock<IUri>();

            //// Create object to test
            //this.HW_WebService = new HW_WebService(
            //    this.restClientMock.Object,
            //    this.restRequestMock.Object,
            //    this.appSettingsMock.Object,
            //    this.uriServiceMock.Object,
            //    this.testLogger);
        }

        /// <summary>
        ///     Test tear down. (runs after each test)
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clear lists
            //this.logMessageList.Clear();
            //this.exceptionList.Clear();
            //this.otherPropertiesList.Clear();
        }

        #region Common Function Tests
        [Test]
        public void UnitTestCommonSerialationStability()
        {
            Fleet test_Fleet = Create_Sample_Fleet();

            var expectedResult = test_Fleet.CheckSum();

            var test_Fleet_text = Common.ToXML(test_Fleet);
            var result = Common.FromXml<Fleet>(test_Fleet_text).CheckSum();

            Assert.AreEqual(expectedResult, result);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        ///    Populate a Sample Fleet Vehicle List
        /// </summary>
        /// <returns>A Fleet Object</returns>
        public Fleet Create_Sample_Fleet()
        {
            var ret_fleet = new Fleet();

            ret_fleet.FleetList.Add(new Vehicle(1, "Mercury", "Cougar", "1967", v_type.CAR,
                "qwertqwert01234567890123", "North", "Center", v_status.INREPAIR));
            ret_fleet.FleetList.Add(new Vehicle(2, "GM", "G20", "1971", v_type.VAN,
                "kwertkwert01234567890123", "South", "Branch 3", v_status.STANDBY));
            ret_fleet.FleetList.Add(new Vehicle(3, "Chevy", "S150", "1988", v_type.TRUCK,
                "dwertdwert01234567890123", "East", "Branch 1", v_status.INTRANSIT));
            ret_fleet.FleetList.Add(new Vehicle(4, "Peterbilt", "Semi", "1994", v_type.SEMI,
                "twerttwert01234567890123", "North", "Center", v_status.INREPAIR));
            ret_fleet.FleetList.Add(new Vehicle(5, "Vox", "Trailer", "2011", v_type.TRAILER,
                "pwertpwert01234567890123", "North", "", v_status.INSERVICE));

            return ret_fleet;
        }
        #endregion
    }
}