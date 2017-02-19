//-----------------------------------------------------------------------
// <copyright file="TodaysDataControllerUnitTests.cs" company="Kenneth Larimer">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Tests.UnitTests
{
    using System.Configuration;
    using System.IO;
    using Controllers;
    using API.Library.APIModels;
    using API.Library.APIServices;
    using Moq;
    using NUnit.Framework;
    using SampleApp.Controllers;

    /// <summary>
    ///     Unit tests for the HW_Message's Data Controller
    /// </summary>
    [TestFixture]
    public class HW_ControllerUnitTests
    {
        /// <summary>
        ///     The mocked data service
        /// </summary>
        private Mock<IDataService> dataServiceMock;

        /// <summary>
        ///     The implementation to test
        /// </summary>
        private HW_MessageController HW_MessageController;

        /// <summary>
        ///     Initialize the test fixture (runs one time)
        /// </summary>
        [TestFixtureSetUp]
        public void InitTestSuite()
        {
            // Setup mocked dependencies
            this.dataServiceMock = new Mock<IDataService>();

            // Create object to test
            this.HW_MessageController = new HW_MessageController(this.dataServiceMock.Object);
        }

        #region Get Tests
        /// <summary>
        ///     Tests the controller's get method for success
        /// </summary>
        [Test]
        public void UnitHW_MessageDataControllerGetSuccess()
        {
            // Create the expected result
            var expectedResult = GetSampleHW_Message();

            // Set up dependencies
            this.dataServiceMock.Setup(m => m.GetHW_Message()).Returns(expectedResult);

            // Call the method to test
            var result = this.HW_MessageController.Get();

            // Check values
            Assert.NotNull(result);
            Assert.AreEqual(result.Data, expectedResult.Data);
        }

        /// <summary>
        ///     Tests the controller's get method for a SettingsPropertyNotFoundException
        /// </summary>
        [Test]
        [ExpectedException(ExpectedException = typeof(SettingsPropertyNotFoundException))]
        public void UnitTestHW_MesssageControllerGetSettingsPropertyNotFoundException()
        {
            // Set up dependencies
            this.dataServiceMock.Setup(m => m.GetHW_Message()).Throws(new SettingsPropertyNotFoundException("Error!"));

            // Call the method to test
            this.HW_MessageController.Get();
        }

        /// <summary>
        ///     Tests the controller's get method for an IOException
        /// </summary>
        [Test]
        [ExpectedException(ExpectedException = typeof(IOException))]
        public void UnitTestHW_MessageControllerGetIOException()
        {
            // Set up dependencies
            this.dataServiceMock.Setup(m => m.GetHW_Message()).Throws(new IOException("Error!"));

            // Call the method to test
            this.HW_MessageController.Get();
        }
        #endregion

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
