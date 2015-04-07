using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Net.Mail;
using System.Net;


namespace mailService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.WriteToFile("Simple Service started {0}");
            this.ScheduleService();
        }

        protected override void OnStop()
        {
            this.WriteToFile("Simple Service stopped {0}");
            this.Schedular.Dispose();
        }
        private Timer Schedular;
        public void ScheduleService()
        {
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
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]); //18:41
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
            catch(Exception ex) //catch error and write to file concerning line number in which code breaks
            {
                WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);
 
                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }
 
        private void SchedularCallback(object e) //when the appropriate time has ellapsed, query all transactions and create 2d array of users with past due books and due date
        {
            //email test

            this.WriteToFile("Emailing gmail account, Simple Service Log: {0}");


            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress("spencernewell93@gmail.com");

            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   // [1] You can try with 465 also, I always used 587 and got success
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
            smtp.UseDefaultCredentials = false; // [3] Changed this
            smtp.Credentials = new NetworkCredential("spencernewell93@gmail.com", "pippin12");  // [4] Added this. Note, first parameter is NOT string.
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress("spencernewell93@gmail.com"));

            //Formatted mail body
            mail.IsBodyHtml = true;
            string st = "Test test 12";

            mail.Body = st;
            smtp.Send(mail);


            this.ScheduleService();
        }

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
