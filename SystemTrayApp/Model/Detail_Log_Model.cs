using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Configuration;
using System.Windows;
using System.Runtime.Remoting.Messaging;

namespace SystemTrayApp.Model
{
    public class Detail_Log_Model:Page
    {       
        public Button Backb { get; set; }
        public Button Nextb { get; set; }
        public Button Previousb { get; set; }
        public DatePicker LogDatePickb { get; set; }
        public TextBox Logsb { get; set; }
        public TextBlock LogDateb { get; set; }

        delegate string NewDel(string path);

        public void Previous_Click()
        {
            LogDatePickb.SelectedDate = LogDatePickb.SelectedDate.Value.AddDays(-1);
        }

        public void Next_Click()
        {
            LogDatePickb.SelectedDate = LogDatePickb.SelectedDate.Value.AddDays(1);
        }

        public void Home()
        {
            Uri uri = new Uri("LandingPage.xaml", UriKind.Relative);
            ((MainWindow)Application.Current.MainWindow).MainFrame_Source(uri);
        }

        
        public void LogDatePick_MouseEnter()
        {
            LogDatePickb.Visibility = Visibility.Visible;
        }

        public void LogDatePick_MouseLeave()
        {
            LogDatePickb.Visibility = Visibility.Collapsed;

        }

        public void Delmethod()
        {
            NewDel nd = Getdata;

            try
            {

                if (LogDatePickb.SelectedDate == null) { LogDatePickb.SelectedDate = DateTime.Now; }
                string path = ConfigurationManager.AppSettings.Get(Name) + "\\Logs_" + LogDatePickb.SelectedDate.Value.Year + LogDatePickb.SelectedDate.Value.Month.ToString("00") + LogDatePickb.SelectedDate.Value.Day.ToString("00") + ".txt";
                IAsyncResult iar = nd.BeginInvoke(path, Updatedata, null);
            }catch (Exception exception) { Logsb.Text = exception.ToString(); }
        }

        static string Getdata(string path)
        {
            try { return System.IO.File.ReadAllText(path); } catch (Exception error) { return error.ToString(); }
        }

        private void Updatedata(IAsyncResult iasyncResult)
        {
            AsyncResult asyncResult = (AsyncResult)iasyncResult;
            NewDel nd = (NewDel)asyncResult.AsyncDelegate;
            string data = nd.EndInvoke(iasyncResult);

            Logsb.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                try
                {
                    Logsb.Text = data;
                }
                catch (Exception excep) { Console.WriteLine(excep); }
            }));

        }

        public string Config_Source
        {
            get
            {
                string prop = this.Name + "_Config";
                return ConfigurationManager.AppSettings[prop].ToString();
            }
        }

    }
}
