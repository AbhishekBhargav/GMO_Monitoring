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
using GMO_Monitoring.Model;

namespace GMO_Monitoring
{
    /// <summary>
    /// Interaction logic for Login_Page.xaml
    /// </summary>
    public partial class Login_Page : Login_page_model
    {
        public Login_Page()
        {
            InitializeComponent();
            UN = Username;
            PW = Password;
            Login = Login_Button;            
            Loading = Spinner;
            App_Loading = App_spinner;
        }
    }
}
