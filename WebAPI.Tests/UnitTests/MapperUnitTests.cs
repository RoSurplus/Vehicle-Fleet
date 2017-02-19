//-----------------------------------------------------------------------
// <copyright file="HW_MapperUnitTests.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Tests.UnitTests
{
    using API.Library.APIMappers;
    using API.Library.APIModels;
    using NUnit.Framework;

    /// <summary>
    ///     Unit tests for the Mapper
    /// </summary>
    [TestFixture]
    public class HW_MapperUnitTests
    {
        /// <summary>
        ///     The implementation to test
        /// </summary>
        private HW_Mapper HW_Mapper;

        /// <summary>
        ///     Initialize the test fixture (runs one time)
        /// </summary>
        [TestFixtureSetUp]
        public void InitTestSuite()
        {
            // Create object to test
            this.HW_Mapper = new HW_Mapper();
        }

        #region StringToTodaysData Tests
        /// <summary>
        ///     Tests the class's StringToTodaysData method for success with a normal input value
        /// </summary>
        [Test]
        public void UnitTestHW_MapperStringToHW_MessageNormalSuccess()
        {
            const string Data = "Hello There2, World!";

            // Create the expected result
            var expectedResult = GetSampleHW_Message(Data);

            // Call the method to test
            var result = this.HW_Mapper.StringToHW_Message(Data);

            // Check values
            Assert.NotNull(result);
            Assert.AreEqual(result.Data, expectedResult.Data);
        }

        /// <summary>
        ///     Tests the StringToTodaysData method for success with a null input value
        /// </summary>
        [Test]
        public void UnitTestHW_MapperStringToHW_MessageNullSuccess()
        {
            const string Data = null;

            // Create the expected result
            var expectedResult = GetSampleHW_Message(Data);

            // Call the method to test
            var result = this.HW_Mapper.StringToHW_Message(Data);

            // Check values
            Assert.NotNull(result);
            Assert.AreEqual(result.Data, expectedResult.Data);
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
            return new HW_Message()
            {
                Data = data
            };
        }
        #endregion
    }
}