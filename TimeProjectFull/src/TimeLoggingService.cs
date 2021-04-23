using log4net;
using System;
using System.Timers;
using Timeproject.Interface;
namespace Timeproject
{
    public class TimeLoggingService
    {
        private readonly ILog logger;
        private readonly Timer timer;
        private readonly int timeOutLimit;
        private readonly ITimeAPIHelper worldClockHelper;
        private readonly IFileReader fileReader;
        private DateTime xmlFileTime;
        private TimeSpan timeSpan;
        public TimeLoggingService(ILog logger, ITimeAPIHelper worldClockHelper, IFileReader fileReader, int timeOutLimit)
        {
            this.logger = logger;
            this.worldClockHelper = worldClockHelper;
            this.fileReader = fileReader;
            this.timeOutLimit = timeOutLimit;
            timer = new System.Timers.Timer(timeOutLimit) { AutoReset = true };
            timer.Elapsed += ExecuteEvent;
        }

        private void ExecuteEvent(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("Service running now");
            CheckIfFileTimeReached();
        }
        public void Stop()
        {
            logger.Info("Service stopped");
            timer.Stop();
        }
        public void Start()
        {
            logger.Info("Service started");
            SetUTCTime();
            GetFileTime();
            timer.Start();
        }
        private void SetUTCTime()
        {
            logger.InfoFormat("Fetching time from API {0}", worldClockHelper);
            var t2 = worldClockHelper.GetUTCTimeFromAPI();
            t2.Wait();
            var str = t2.Result;
            TimeSpan timeSpan1 = new TimeSpan(0, 0, 0);
            if (!string.IsNullOrEmpty(str))
            {
                var times = str.Split('T')[1];
                var timesArray = times.Split(':');
                timeSpan1 = new TimeSpan(Convert.ToInt32(timesArray[0]), Convert.ToInt32(timesArray[1]), Convert.ToInt32(timesArray[2].Split('.')[0]));
                logger.Info($" UTC time difference with local time {(timeSpan1).ToString()}");
            }
            timeSpan = DateTime.Today.Add(timeSpan1) - DateTime.Now;
        }
        private void GetFileTime()
        {
            logger.InfoFormat("Reading xml from file path {0}", fileReader);
            xmlFileTime = DateTime.Today.Add(TimeSpan.Parse(fileReader.GetExecutionTimeFromXML()));
            logger.InfoFormat("Time Read from xml file {0} is {1}", fileReader, xmlFileTime);
        }
        private void CheckIfFileTimeReached()
        {
            var diff = DateTime.Now.Add(timeSpan) - xmlFileTime;
            //logger.Info($"dfff time {diff}");
            if (diff < new TimeSpan(0, 0, 1) && diff > new TimeSpan(0, 0, -1))
            {
                logger.Info($"Welcome [{DateTime.Now.ToUniversalTime().ToString()}] - [{DateTime.Now.ToString()}]");
            }
        }

    }
}