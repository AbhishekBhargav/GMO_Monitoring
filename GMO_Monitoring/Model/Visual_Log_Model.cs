using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BarChart;

namespace GMO_Monitoring.Model
{
    public class Visual_Log_Model:Page
    {
        public Button Backb { get; set; }
        public Bargraph Barb { get; set; }

        public void Home()
        {
            Uri uri = new Uri("LandingPage.xaml", UriKind.Relative);
            ((MainWindow)Application.Current.MainWindow).MainFrame_Source(uri);
        }
    }
}
