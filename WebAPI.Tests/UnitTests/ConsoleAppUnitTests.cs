//-----------------------------------------------------------------------
// <copyright file="ConsoleAppUnitTests.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using ConsoleApp.Application;
    using ConsoleApp.Services;
    using API.Library.APIModels;
    using API.Library.APIServices;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    ///     Unit tests for the Console App
    /// </summary>
    [TestFixture]
    public class ConsoleAppUnitTests
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
        ///     The mocked Web Service
        /// </summary>
        private Mock<IHW_WebService> HW_WebServiceMock;

        /// <summary>
        ///     The test logger
        /// </summary>
        private ILogger testLogger;

        /// <summary>
        ///     The implementation to test
        /// </summary>
        private ConsoleApp HW_ConsoleApp;

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
            this.HW_WebServiceMock = new Mock<IHW_WebService>();
            this.testLogger = new TestLogger(ref this.logMessageList, ref this.exceptionList, ref this.otherPropertiesList);

            // Create object to test
            this.HW_ConsoleApp = new ConsoleApp(this.HW_WebServiceMock.Object, this.testLogger);
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

        #region Run Tests
        /// <summary>
        ///     Tests the class's Run method for success when normal data was found
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNormalDataSuccess()
        {
            const string Data = "Hello There0, World!";

            // Create return models for dependencies
            var todaysData = GetSampleHW_Message(Data);

            // Set up dependencies
            this.HW_WebServiceMock.Setup(m => m.GetHW_Message()).Returns(todaysData);

            // Call the method to test
            this.HW_ConsoleApp.Run(null);

            // Check values
            Assert.AreEqual(this.logMessageList.Count, 1);
            Assert.AreEqual(this.logMessageList[0], Data);
        }

        /// <summary>
        ///     Tests the class's Run method for success when null data was found
        /// </summary>
        [Test]
        public void UnitTestConsoleAppRunNullDataSuccess()
        {
            // Set up dependencies
            this.HW_WebServiceMock.Setup(m => m.GetHW_Message()).Returns((HW_Message)null);

            // Call the method to test
            this.HW_ConsoleApp.Run(null);

            // Check values
            Assert.AreEqual(this.logMessageList.Count, 1);
            Assert.AreEqual(this.logMessageList[0], "No data was found!");
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