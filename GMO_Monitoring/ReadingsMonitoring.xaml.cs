﻿using System;
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
using GMO_Monitoring.VM;
using GMO_Monitoring.Model;

namespace GMO_Monitoring
{
    /// <summary>
    /// Interaction logic for ReadingsMonitoring.xaml
    /// </summary>
    public partial class ReadingsMonitoring : Detail_Log_Model
    {
        public ReadingsMonitoring()
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
            DGM_List.Add(ConfigDG);
            Loaded += DL_Loaded;
        }        
    }
}