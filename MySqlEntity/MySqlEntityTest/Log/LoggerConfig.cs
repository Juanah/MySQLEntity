using System;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Appender;
using log4net.Core;
using log4net;

namespace MySqlEntity
{
	internal class LoggerConfig
	{
		public static void Setup()
		{
			Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

			PatternLayout patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
			patternLayout.ActivateOptions();

			RollingFileAppender roller = new RollingFileAppender();
			roller.AppendToFile = false;
			roller.File = @"log_mysqltester.txt";
			roller.Layout = patternLayout;
			roller.MaxSizeRollBackups = 5;
			roller.MaximumFileSize = "1GB";
			roller.RollingStyle = RollingFileAppender.RollingMode.Size;
			roller.StaticLogFileName = true;            
			roller.ActivateOptions();
			hierarchy.Root.AddAppender(roller);

			ConsoleAppender consoleAppender = new ConsoleAppender ();
			consoleAppender.Layout = patternLayout;
			consoleAppender.ActivateOptions ();
			hierarchy.Root.AddAppender (consoleAppender);

			MemoryAppender memory = new MemoryAppender();
			memory.ActivateOptions();
			hierarchy.Root.AddAppender(memory);

			hierarchy.Root.Level = Level.All;
			hierarchy.Configured = true;
		}
	}
}

