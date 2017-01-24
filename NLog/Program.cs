using System;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using NLog;
using NLog.Targets;
using NLog.Config;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace NLog
{
    public sealed class Program : IDisposable
    {
        #region Member variables

        // lock
        private static readonly Object obj = new Object();

        #endregion

        #region Main method

        static void Main(string[] args)
        {
            Networking networking = new Networking();
            //Logging logging = new Logging();

            BindData();
            GetConfigurationSettings();

            Console.WriteLine("NLog is a free logging platform for .NET\n");

            //// example of possible log levels
            //logging.WriteLogMessages();
            //logging.WriteParameterizedLogMessagges();

            //Thread t1 = new Thread(networking.StartServer);
            //t1.Start();

            //// Testing of lock
            //lock (obj)
            //{
            //    Thread t2 = new Thread(networking.StartClient);
            //    t2.Start();
            //}

            //// Testing of delays
            //Task.Run(async () => {
            //    Thread t3 = new Thread(networking.StartClient);
            //    await Task.Delay(8000);
            //    t3.Start();
            //});

            networking.StartServer(ConfigurationManager.AppSettings["IpAddress"], int.Parse(ConfigurationManager.AppSettings["SocketPort"]));
            networking.StartClient(ConfigurationManager.AppSettings["IpAddress"], int.Parse(ConfigurationManager.AppSettings["SocketPort"]));

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        static void BindData() 
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();

            MySqlCommand command = new MySqlCommand("select * from configuration", connection);
            MySqlDataReader read = command.ExecuteReader();
            if (read != null)
            {
                while (read.Read())
                {
                    string EventType = Convert.ToString(read["EventType"]);
                    Console.WriteLine(EventType);
                }
            }
            read.Close();

            MySqlCommand command2 = new MySqlCommand("insert into configuration values(\"umetnuto1\")", connection);
            command2.ExecuteNonQuery();

            connection.Close();
        }


        static void GetConfigurationSettings()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/mojaLogDatoteka.txt";
            fileTarget.Layout = "${message}";

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            // Example usage
            Logger logger = LogManager.GetLogger("Example");
            logger.Trace("trace log message");
            logger.Debug("debug log message");
            logger.Info("info log message");
            logger.Warn("warn log message");
            logger.Error("error log message");
            logger.Fatal("fatal log message");
        }


    }
}
