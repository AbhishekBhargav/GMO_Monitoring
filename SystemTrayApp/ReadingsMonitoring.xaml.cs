using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemTrayApp
{
    /// <summary>
    /// Interaction logic for ReadingsMonitoring.xaml
    /// </summary>
    public partial class ReadingsMonitoring : Page
    {
        public ReadingsMonitoring()
        {
            InitializeComponent();
            LogDatePick.SelectedDate = DateTime.Today;
            try
            {
                Logs.Text = System.IO.File.ReadAllText(ConfigurationManager.AppSettings.Get("ReadingCollection Logs") +"\\Logs_" + LogDatePick.SelectedDate.Value.Year + LogDatePick.SelectedDate.Value.Month.ToString("00") + LogDatePick.SelectedDate.Value.Day.ToString("00") + ".txt");
            }
            catch (Exception error) { Logs.Text = error.ToString(); }




        }



        private void LogDate_MouseEnter(object sender, MouseEventArgs e)
        {
            LogDatePick.Visibility = Visibility.Visible;
        }

        private void LogDatePick_MouseLeave(object sender, MouseEventArgs e)
        {
            LogDatePick.Visibility = Visibility.Collapsed;

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            LogDatePick.SelectedDate = LogDatePick.SelectedDate.Value.AddDays(-1);

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            LogDatePick.SelectedDate = LogDatePick.SelectedDate.Value.AddDays(1);

        }

        private void LogDatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Logs.Text = System.IO.File.ReadAllText(ConfigurationManager.AppSettings.Get("ReadingCollection Logs") + "\\Logs_" + LogDatePick.SelectedDate.Value.Year + LogDatePick.SelectedDate.Value.Month.ToString("00") + LogDatePick.SelectedDate.Value.Day.ToString("00") + ".txt");
            }
            catch (Exception error) { Logs.Text = error.ToString(); }
        }
    }
}