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
using System.Runtime.Remoting.Messaging;
using SystemTrayApp.VM;
using SystemTrayApp.Model;

namespace SystemTrayApp
{
    /// <summary>
    /// Interaction logic for ReadingsMonitoring.xaml
    /// </summary>
    public partial class NFS_Cleanup : Detail_Log_Model
    {
        public NFS_Cleanup()
        {
            InitializeComponent();
            Backb = Back;
            Nextb = Next;
            Previousb = Previous;
            LogDatePickb = LogDatePick;
            Logsb = Logs;
            LogDateb = LogDate;
            LS = Page_Loading;
            DGM_List = new List<DGM.UserControl1>();
            DGM_List.Add(ADConfigDG);
            DGM_List.Add(DConfigDG);
            Loaded += DL_Loaded;
            //oDelmethod();
        }

        
    }
}