using System;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using Topshelf;
using Microsoft.Extensions.Configuration;
using Autofac;
using Timeproject.Interface;
namespace Timeproject
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            var logger = LogManager.GetLogger(typeof(Program));
            var builder = new ContainerBuilder();

            try
            {


                IConfiguration configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json", true, true)
                   .AddUserSecrets<Program>()
                   .Build();
                //configuration.GetSection("");
                builder.Register(l => logger).As<ILog>();
                builder.RegisterType<FileReader>().As<IFileReader>().WithParameter("filepath", configuration.GetSection("xmlFilePath").Value);
                builder.RegisterType<TimeAPIHelper>().As<ITimeAPIHelper>().WithParameter("uri", configuration.GetSection("worldTimeUri").Value);
                builder.RegisterType<TimeLoggingService>().As<TimeLoggingService>().WithParameter("timeOutLimit", Convert.ToInt32(configuration.GetSection("timeOutLimit").Value));

                var Container = builder.Build();
                using (var scope = Container.BeginLifetimeScope())
                {

                    HostFactory.Run(x => x.Service<TimeLoggingService>(s =>
                                {
                                    s.ConstructUsing(service => scope.Resolve<TimeLoggingService>());
                                    s.WhenStarted(service => service.Start());
                                    s.WhenStopped(service => service.Stop());
                                }));
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Error occuring while starting service {0}", e.Message);
                logger.Error(e.StackTrace);
            }

        }
    }
}
