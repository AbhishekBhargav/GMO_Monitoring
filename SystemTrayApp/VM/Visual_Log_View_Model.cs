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
    class Visual_Log_View_Model
    {

        public ICommand Home
        {
            get
            {
                return new VLVM_Delgate
                {
                    Command = (Visual_Log_Model newpage) => newpage.Home()
                };
            }
        }

    }
}
