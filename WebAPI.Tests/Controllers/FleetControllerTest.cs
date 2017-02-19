using API.Library.APIModels;
using API.Library.APIServices;
using Moq;
using NUnit.Framework;
using SampleApp.Controllers;
using System.Web.Mvc;
using API.Library.APIWrappers;
using API.Library.APIMappers;
using API.Library.APIResources;
using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleApp.Tests.Controllers
{
    [TestFixture]
    public class FleetControllerTest
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
        ///     The mocked data service
        /// </summary>
        private Mock<IDataService> dataServiceMock;

        /// <summary>
        ///     The implementation to test
        /// </summary>
        private FleetController controller;


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

        [Test]
        public void Get()
        {
            // Setup mocked dependencies
            this.dataServiceMock = new Mock<IDataService>();

            // Arrange
            controller = new FleetController(this.dataServiceMock.Object);

            // Create the expected result
            var expectedResult = GetSampleHW_Message();

            // Set up dependencies
            this.dataServiceMock.Setup(m => m.GetHW_Message()).Returns(expectedResult);

            // Call the method to test
            var result = this.controller.Get();

            // Check values
            Assert.NotNull(result);
            Assert.AreEqual(result.Data, expectedResult.Data);
        }

        /***
                [Test]
                public void Index()
                {
                    // Create return models for dependencies
                    const string DataFilePath = "some/path";
                    const string FileContents = "";

                    // Setup mocked dependencies
                    this.dataServiceMock = new Mock<IDataService>();

                    // Set up dependencies
                    this.appSettingsMock.Setup(m => m.Get(AppSettingsKeys.Fleet_File)).Returns(DataFilePath);
                    this.fileIOServiceMock.Setup(m => m.ReadFile(DataFilePath)).Returns(FileContents);

                    // Arrange
                    controller = new FleetController(this.dataServiceMock.Object);

                    // Act
                    ViewResult result = controller.Index() as ViewResult;

                    // Assert
                    Assert.IsNotNull(result);
                    string title = result.ViewBag.Title;
                    Assert.AreEqual("Fleet Page", result.ViewBag.Title);
                }

                [Test]
                public void TestSerialationStability()
                {
                    // Setup mocked dependencies
                    this.dataServiceMock = new Mock<IDataService>();

                    // Arrange
                    controller = new FleetController(this.dataServiceMock.Object);

                    // Act
                    controller.Load_Fleet_File();
                    int origchksum = controller.CheckSum();
                    controller.Save_Fleet_File();
                    controller.Load_Fleet_File();
                    int newchksum = controller.CheckSum();

                    // Assert
                    Assert.AreEqual(origchksum, newchksum);
                }
                ****/

        #region Helper Methods
        /// <summary>
        ///     Gets a sample HW_Message model
        /// </summary>
        /// <returns>A sample HW_Message model</returns>
        private static HW_Message GetSampleHW_Message()
        {
            return new HW_Message()
            {
                Data = "Hello There5, World!"
            };
        }
        #endregion
    }
}
