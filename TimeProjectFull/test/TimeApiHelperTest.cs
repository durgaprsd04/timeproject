using System;
using Xunit;
using Timeproject;
using System.IO;
using log4net;
using Moq;
namespace test
{
    public class TimeAPIHelperTest
    {
        [Fact]
        public void GetUTCTimeFromAPI_Test1()
        {
            var logMock = new Mock<ILog>();
            var timeApiHelper = new TimeAPIHelper("abcd", logMock.Object);
            var task = timeApiHelper.GetUTCTimeFromAPI();
            var result = task.Result;
            Assert.Equal(string.Empty,result);
        }
        [Fact]
        public void GetUTCTimeFromAPI_Test2()
        {
            var logMock = new Mock<ILog>();
            var timeApiHelper = new TimeAPIHelper("http://worldtimeapi.org/api/ip", logMock.Object);
            var task = timeApiHelper.GetUTCTimeFromAPI();
            var result = task.Result;
            Assert.True(DateTime.Now- DateTime.Parse(result) < new TimeSpan(0,0,2), "API should be less that 2 seconds");
        }
    }
}