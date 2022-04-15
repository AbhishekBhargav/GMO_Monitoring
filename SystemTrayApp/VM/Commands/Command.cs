using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemTrayApp.Model;

namespace SystemTrayApp.VM.Commands
{
      public class DLVM_Delgate : ICommand
    {
        public Func<bool> CanExecuteFun { get; set; }
        public Action<Detail_Log_Model> Command { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //return CanExecuteFun == null || CanExecuteFun();
        }

        public void Execute(object parameter)
        {
            Command(parameter as Detail_Log_Model);
        }
    }

    public class VLVM_Delgate : ICommand
    {
        public Func<bool> CanExecuteFun { get; set; }
        public Action<Visual_Log_Model> Command { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFun == null || CanExecuteFun();
        }

        public void Execute(object parameter)
        {
            Command(parameter as Visual_Log_Model);
        }
    }
}
