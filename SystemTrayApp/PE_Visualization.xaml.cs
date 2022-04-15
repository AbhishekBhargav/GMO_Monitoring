using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BarChart;
using SystemTrayApp.Model;

namespace SystemTrayApp
{
    /// <summary>
    /// Interaction logic for PE_Visualization.xaml
    /// </summary>
    public partial class PE_Visualization : Visual_Log_Model
    {
        public PE_Visualization()
        {
            InitializeComponent();
            Barb = bar;
            //Bargraph b = bar;
            //this.DataContext = this;
            //bar.Items.CollectionChanged += (e, v) => b.Loop();
            
        }
    }
}
