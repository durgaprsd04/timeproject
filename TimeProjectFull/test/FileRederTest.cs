using System;
using Xunit;
using Timeproject;
using System.IO;
using log4net;
using Moq;
namespace test
{
    public class FileReaderTest
    {
        [Fact]
        public void GetExecutionTimeFromXML_Test1()
        {
            using( var file = new StreamWriter("temp.xml"))
            {
            file.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
                                    "<ExecutionTime>"+
                                    "18:20:00"+
                                    "</ExecutionTime>");
            file.Close();
            }
            var logMock = new Mock<ILog>();
            var f = new FileReader("temp.xml", logMock.Object);
            Assert.Equal("18:20:00", f.GetExecutionTimeFromXML());
        }
        [Fact]
        public void GetExecutionTimeFromXML_Test2()
        {
            var logMock = new Mock<ILog>();
            var f = new FileReader("temp3.xml", logMock.Object);
            Assert.Equal(string.Empty, f.GetExecutionTimeFromXML());
        }
    }
}
