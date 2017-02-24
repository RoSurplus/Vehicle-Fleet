//-----------------------------------------------------------------------
// <copyright file="DataServiceUnitTests.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Tests.UnitTests
{
    using System;
    using System.Configuration;
    using System.IO;
    using API.Library.APIMappers;
    using API.Library.APIModels;
    using API.Library.APIResources;
    using API.Library.APIServices;
    using Moq;
    using NUnit.Framework;
    using API.Library.APIWrappers;

    /// <summary>
    ///     Unit tests for the Data Service
    /// </summary>
    [TestFixture]
    public class DataServiceUnitTests
    {
        /// <summary>
        ///     The mocked application settings service
        /// </summary>
        private Mock<IAppSettings> appSettingsMock;

        /// <summary>
        ///     The mocked DateTime wrapper
        /// </summary>
        private Mock<IDateTime> dateTimeWrapperMock;

        /// <summary>
        ///     The mocked File IO service
        /// </summary>
        private Mock<IFileIOService> fileIOServiceMock;

        /// <summary>
        ///     The mocked Mapper
        /// </summary>
        private Mock<IHW_Mapper> HW_MapperMock;

        /// <summary>
        ///     The implementation to test
        /// </summary>
        private HW_DataService HW_DataService;

        /// <summary>
        ///     Initialize the test fixture (runs one time)
        /// </summary>
        [TestFixtureSetUp]
        public void InitTestSuite()
        {
            // Setup mocked dependencies
            this.appSettingsMock = new Mock<IAppSettings>();
            this.dateTimeWrapperMock = new Mock<IDateTime>();
            this.fileIOServiceMock = new Mock<IFileIOService>();
            this.HW_MapperMock = new Mock<IHW_Mapper>();

            // Create object to test
            this.HW_DataService = new HW_DataService(
                this.appSettingsMock.Object,
                this.dateTimeWrapperMock.Object,
                this.fileIOServiceMock.Object,
                this.HW_MapperMock.Object);
        }

        #region GetHW_Message Tests
        /// <summary>
        ///     Tests the class's GetTodaysData method for success
        /// </summary>
        [Test]
        public void UnitTestHWDataServiceHW_Load_Fleet_FileSuccess()
        {
            // Create return models for dependencies
            const string DataFilePath = "some/path";
            const string FileContents = "Fleet File Data.";
            var nowDate = DateTime.Now;

            //            var expectedResult = GetSampleHW_Message(FileContents);
            var expectedResult = FileContents;

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.Fleet_File)).Returns(DataFilePath);
            this.fileIOServiceMock.Setup(m => m.ReadFile(DataFilePath)).Returns(FileContents);
            this.dateTimeWrapperMock.Setup(m => m.Now()).Returns(nowDate);

            // Call the method to test
            var result = this.HW_DataService.GetFleet_File();

            // Check values
            NUnit.Framework.Assert.NotNull(result);
            NUnit.Framework.Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method for success
        /// </summary>
        [Test]
        public void UnitTestHWDataServiceGetHW_MessageSuccess()
        {
            // Create return models for dependencies
            const string DataFilePath = "some/path";
            const string FileContents = "This is the Current API Message of: {0} {1}.";
            var nowDate = DateTime.Now;
            //string rawData = null;
            //if (!string.IsNullOrEmpty(FileContents))
            //    rawData = string.Format(FileContents, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            //else
            //    rawData = FileContents;

            // Create the expected result
            var expectedResult = GetSampleHW_Message(FileContents);

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(DataFilePath);
            this.fileIOServiceMock.Setup(m => m.ReadFile(DataFilePath)).Returns(FileContents);
            this.dateTimeWrapperMock.Setup(m => m.Now()).Returns(nowDate);
            this.HW_MapperMock.Setup(m => m.StringToHW_Message(FileContents)).Returns(expectedResult);

            // Call the method to test
            var result = this.HW_DataService.GetHW_Message();

            // Check values
            NUnit.Framework.Assert.NotNull(result);
            NUnit.Framework.Assert.AreEqual(result.Data, expectedResult.Data);
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method when the setting key is null
        /// </summary>
        [Test]
        [ExpectedException(ExpectedException = typeof(SettingsPropertyNotFoundException), ExpectedMessage = ErrorCodes.HW_MessageFileSettingsKeyError)]
        public void UnitTestDataServiceGetHW_MessageSettingKeyNull()
        {
            // Create return models for dependencies
            const string DataFilePath = null;

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(DataFilePath);

            // Call the method to test
            this.HW_DataService.GetHW_Message();
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method when the setting key is an empty string
        /// </summary>
        [Test]
        [ExpectedException(ExpectedException = typeof(SettingsPropertyNotFoundException), ExpectedMessage = ErrorCodes.HW_MessageFileSettingsKeyError)]
        public void UnitTestDataServiceGetHW_MessasgeSettingKeyEmptyString()
        {
            // Create return models for dependencies
            var dataFilePath = string.Empty;

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(dataFilePath);

            // Call the method to test
            this.HW_DataService.GetHW_Message();
        }

        /// <summary>
        ///     Tests the class's GetTodaysData method for an IO Exception
        /// </summary>
        [Test]
        [ExpectedException(ExpectedException = typeof(IOException))]
        public void UnitTestDataServiceGetHW_MessageIOException()
        {
            // Create return models for dependencies
            const string DataFilePath = "some/path";

            // Set up dependencies
            this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.HW_MessageFileKey)).Returns(DataFilePath);
            this.fileIOServiceMock.Setup(m => m.ReadFile(DataFilePath)).Throws(new IOException("Error!"));

            // Call the method to test
            this.HW_DataService.GetHW_Message();
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