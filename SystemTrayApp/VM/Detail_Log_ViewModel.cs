using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using SystemTrayApp.Model;
using SystemTrayApp.VM.Commands;

namespace SystemTrayApp.VM
{
    class Detail_Log_ViewModel
    {        
        public ICommand Previous
        {
            get
            {
                return new DLVM_Delgate
                {
                    Command = (Detail_Log_Model newpage) => newpage.Previous_Click()
                };
            }
        }

        public ICommand Next
        {
            get
            {
                return new DLVM_Delgate
                {
                    Command = (Detail_Log_Model newpage) => newpage.Next_Click()
                };
            }
        }

        public ICommand Home
        {
            get
            {
                return new DLVM_Delgate
                {
                    Command = (Detail_Log_Model newpage) => newpage.Home()
                };
            }
        }

        public ICommand DatePicker_Visible
        {
            get
            {
                return new DLVM_Delgate
                {
                    Command = (Detail_Log_Model newpage) => newpage.LogDatePick_MouseEnter()
                };
            }
        }

        public ICommand DatePicker_InVisible
        {
            get
            {
                return new DLVM_Delgate
                {
                    Command = (Detail_Log_Model newpage) => newpage.LogDatePick_MouseLeave()
                };
            }
        }

        public ICommand Update_Log
        {
            get
            {
                return new DLVM_Delgate
                {
                    Command =  (Detail_Log_Model newpage) => newpage.DL_Loaded(new Button(),new System.Windows.RoutedEventArgs() { RoutedEvent=ButtonBase.ClickEvent})
                };
            }
        }

    }
}
