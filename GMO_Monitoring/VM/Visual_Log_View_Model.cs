using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GMO_Monitoring.Model;
using GMO_Monitoring.VM.Commands;

namespace GMO_Monitoring.VM
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
