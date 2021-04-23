using System;
using log4net;
using System.Threading.Tasks;
using Timeproject.Interface;
using System.Globalization;
namespace timeproject
{
    public class TimeManager : ITimeManager
    {
        private readonly ILog logger;
        private readonly IFileReader fileReader;
        private readonly ITimeAPIHelper timeAPIHelper;
        private readonly int threshold;
        public TimeManager(ILog logger, IFileReader fileReader, ITimeAPIHelper timeAPIHelper, int threshold)
        {
            this.logger = logger;
            this.fileReader = fileReader;
            this.timeAPIHelper = timeAPIHelper;
            this.threshold = threshold;

        }
        public async Task<bool> CompareTime()
        {
            var configuredTime = fileReader.GetExecutionTimeFromXML(); 
            var currentTime = await timeAPIHelper.GetUTCTimeFromAPI();
            var d1 = DateTime.ParseExact(configuredTime, "hh:mm:yy", CultureInfo.InvariantCulture);
            var d2 = DateTime.ParseExact(currentTime, "hh:mm:yy", CultureInfo.InvariantCulture);
            if(d1.TimeOfDay.Subtract(d2.TimeOfDay) < new TimeSpan(0,0,threshold) )
            {
                logger.Info($"Welcome – [{d1.ToString()}] – [{DateTime.Now.ToString()}]");
            }
            return true;
        }
    }
}