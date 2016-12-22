using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
// dodano
using System.IO;
using System.Threading;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace WindowsService
{

    public partial class Service1 : ServiceBase
    {
        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service
        /// by the Service Control Manager (SCM) or when the operating system starts
        /// (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            this.WriteToFile("Simple Service started {0}");
            this.ScheduleService();
        }

        /// <summary>
        /// Get settings from the database that will be used for app configuration
        /// </summary>
        private void GetConfigurationSettings()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=siem_sql_database");
                connection.Open();

                MySqlCommand command = new MySqlCommand("select * from machines where MachineName = \"MyMachineName\"", connection);
                MySqlDataReader read = command.ExecuteReader();
                if (read != null)
                {
                    while (read.Read())
                    {
                        string ServiceSettings = Convert.ToString(read["ServiceSettings"]);
                        //Console.WriteLine(EventType);
                        this.WriteToFile(ServiceSettings);
                    }
                }
                read.Close();



                /*
                // Primjer unosa u bazu podataka putem querry-ja
                MySqlCommand command2 = new MySqlCommand("insert into configuration values(\"servisUnosNovi\")", connection);
                command2.ExecuteNonQuery();
                */

                connection.Close();

                this.WriteToFile("Connection successful");
            }
            catch (Exception e)
            {
                this.WriteToFile("ERROR: " + e);
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service
        /// by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            this.WriteToFile("Simple Service stopped {0}");
            this.Schedular.Dispose();
        }

        // Mehanizam za izvršavanje metoda u zadanom intervalu
        private Timer Schedular;

        /// <summary>
        /// When the Windows Service starts it calls the ScheduleService method which first reads the Mode AppSetting.. 
        /// There’s a ScheduledTime variable which is set in both modes.
        /// 
        /// When the Mode is set to Daily then the ScheduledTime is read from the AppSettings. 
        /// In the case when the scheduled time is passed, it is updated to same time on the next day.
        /// 
        /// When the Mode is set to Interval then the IntervalMinutes is read from the AppSettings
        /// and the schedule time is calculated by adding the IntervalMinutes to the Current Time.
        /// 
        /// Finally the Timer is set to run the scheduled time. When the scheduled time is elapsed,
        /// the Timer’s Callback method is triggered which logs the current date and time to a Text file.
        /// </summary>
        public void ScheduleService()
        {
            this.GetConfigurationSettings();

            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                this.WriteToFile("Simple Service Mode: " + mode + " {0}");

                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;

                if (mode == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                this.WriteToFile("Simple Service scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void SchedularCallback(object e)
        {
            this.WriteToFile("Simple Service Log: {0}");
            this.ScheduleService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void WriteToFile(string text)
        {
            string path = "C:\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
    }

}
