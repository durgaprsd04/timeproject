using System;
using System.Xml.Linq;
using Timeproject.Interface;
using log4net;
namespace Timeproject
{
    public class FileReader : IFileReader
    {
        private readonly string filepath;
        private ILog logger;
        public FileReader(string filepath, ILog logger)
        {
            this.filepath = filepath;
            this.logger = logger;
        }

        public override string ToString()
        {
            return filepath;
        }
        public string GetExecutionTimeFromXML()
        {
            //var key = ConfigurationManager.AppSettings["xmlKey"];
            var result = string.Empty;
            try
            {
                XElement xmlFile = XElement.Load(filepath);
                foreach(var x in xmlFile.Nodes())
                    if(x.Parent.Name=="ExecutionTime")
                        result = x.ToString();
            }
            catch(Exception e)
            {
                logger.ErrorFormat("Error occured while reading from file {0}",e.Message);
            }
            return result;
        }
    }
}
