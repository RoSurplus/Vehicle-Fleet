using API.Library.APIModels;
using API.Library.APIServices;
using Moq;
using NUnit.Framework;
using SampleApp.Controllers;

namespace SampleApp.Tests.Controllers
{
    [TestFixture]
    public class HW_MessageControllerTest
    {
        /// <summary>
        ///     The mocked data service
        /// </summary>
        private Mock<IDataService> dataServiceMock;

        /// <summary>
        ///     The implementation to test
        /// </summary>
        private HW_MessageController HW_MessageController;

        [Test]
        public void Get()
        {
            // Setup mocked dependencies
            this.dataServiceMock = new Mock<IDataService>();

            // Arrange
            HW_MessageController = new HW_MessageController(this.dataServiceMock.Object);

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
