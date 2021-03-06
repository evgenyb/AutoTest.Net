using AutoTest.Core.Configuration;
using System.Reflection;
using Castle.Core.Logging;
using log4net.Config;
using log4net;
using Castle.Facilities.Logging;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AutoTest.Console
{
    internal class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof (Program));

        private static void Main(string[] args)
        {
            _logger.Info("Starting up AutoTester");
			if (userWantedCommandLineHelpPrinted(args))
				return;
            BootStrapper.Configure();
            BootStrapper.Container
                .AddFacility("logging", new LoggingFacility(LoggerImplementation.Log4net));
            BootStrapper.RegisterAssembly(Assembly.GetExecutingAssembly());
            var directory = getWatchDirectory(args);
            BootStrapper.InitializeCache(directory);
            var application = BootStrapper.Services.Locate<IConsoleApplication>();
            application.Start(directory);
            BootStrapper.ShutDown();
        }
		
		private static bool userWantedCommandLineHelpPrinted(string[] args)
		{
			if (args.Length != 1)
				return false;
			if (args[0] != "--help" && args[0] != "-help" && args[0] != "/help")
				return false;
			writeConsoleUsage();
			return true;
		}
		
		private static void writeConsoleUsage()
		{
			_logger.Info("AutoTest.WinForms.exe command line arguments");
			_logger.Info("");
			_logger.Info("To specify watch directory on startup you can type:");
			_logger.Info("\tAutoTest.WinForms.exe \"Path to the directory you want\"");
		}

        private static string getWatchDirectory(string[] args)
        {
            string directory;
            if ((directory = getPossibleCommandArgs(args)) != null)
                return directory;
            if ((directory = pickFromConfiguration()) != null)
                return directory;
            return "";
        }

        private static string pickFromConfiguration()
        {
            var configuration = BootStrapper.Services.Locate<IConfiguration>();
            if (configuration.WatchDirectores.Length == 0)
                return null;
            _logger.Info("Pick a directory to watch. Type the number of your choice");
            for (int i = 0; i < configuration.WatchDirectores.Length; i++)
                _logger.InfoFormat("1. {0}", configuration.WatchDirectores[i]);
            return pickDirectory(configuration);
        }

        private static string pickDirectory(IConfiguration configuration)
        {
            var numberOfDirectories = configuration.WatchDirectores.Length;
            var directory = "";
            while (true)
            {
                int number;
                var numberString = System.Console.ReadLine();
                if (int.TryParse(numberString, out number))
                {
                    if (number > 0 && number <= numberOfDirectories)
                    {
                        directory = configuration.WatchDirectores[number-1];
                        break;
                    }
                }
                _logger.Info("Invalid number");
            }
            return directory;
        }

        private static string getPossibleCommandArgs(string[] args)
        {
            if (args == null)
                return null;
            if (args.Length != 1)
                return null;
            return args[0];
        }
    }
}