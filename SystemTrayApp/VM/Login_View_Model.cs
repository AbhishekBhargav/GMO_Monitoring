using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemTrayApp.Model;
using SystemTrayApp.VM.Commands;

namespace SystemTrayApp.VM
{
    class Login_View_Model
    {
        public ICommand Login_Command
        {
            get
            {
                return new LPVM_Delegate
                {
                    Command = async (Login_page_model newpage) => await newpage.Login_Click_Async()
                };
            }
        }
    }

    
}
