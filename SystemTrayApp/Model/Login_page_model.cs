using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LoadingSpinnerControl;
using CredentialManagement;
using System.Configuration;

namespace SystemTrayApp.Model
{
    public class Status_Data
    {
        public Visibility Visibility { get; set; }
        public System.Windows.Media.Brush Status_Color { get; set; }

        public string Status { get; set; }
    }
    public class Login_page_model : Window
    {
        protected TextBox UN;
        protected PasswordBox PW;

        protected Button Login;
        protected CheckBox CB;
        protected LCC Loading;

        protected LCC App_Loading;

        

        public Status_Data Status_Data_Instance
        {
            get { return (Status_Data)GetValue(Status_Data_InstanceProperty); }
            set { SetValue(Status_Data_InstanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status_Data_Instance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Status_Data_InstanceProperty =
            DependencyProperty.Register("Status_Data_Instance", typeof(Status_Data), typeof(Login_page_model), new PropertyMetadata(new Status_Data() { Visibility=Visibility.Collapsed,Status_Color= System.Windows.Media.Brushes.Black,Status="Click to proceed"}));



        protected Status_Data Login_Status_Data_Async()
        {
            Dispatcher.Invoke(() =>
            {
                Loading.Enabled = true;
            });
            try
            {
                SearchResult results;
                DirectoryEntry de = Dispatcher.Invoke(() =>
                {
                   return new DirectoryEntry("LDAP://prod_dc1.amo.edf.local", UN.Text, PW.Password);
                });
                DirectorySearcher dsearch = new DirectorySearcher(de);
                results = dsearch.FindOne();                
                return Dispatcher.Invoke(() =>
                {
                    Loading.Enabled = false;
                    return new Status_Data() { Visibility = Visibility.Visible, Status_Color = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#3DCD58"), Status = "Login Success!!!" };
                });
            }
            catch(Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    Loading.Enabled=false;
                });
                return new Status_Data() { Visibility = Visibility.Visible, Status_Color = System.Windows.Media.Brushes.Red, Status = e.Message.ToString() };
            }            
            
        }

        public async Task Login_Click_Async()
        {
            Status_Data_Instance = new Status_Data() { Visibility = Visibility.Visible, Status_Color = System.Windows.Media.Brushes.Red, Status = "" };
            Status_Data_Instance = await Task.Run(() => Login_Status_Data_Async());
            if (Status_Data_Instance.Status_Color!= System.Windows.Media.Brushes.Red)
            {
                List<string> Access_Paths = ConfigurationManager.AppSettings.Get("Access_Path").Split(';').ToList<string>();
                foreach (string path in Access_Paths)
                {
                    using (var cred = new Credential())
                    {
                        cred.Target = path;
                        cred.Type = CredentialType.DomainPassword;
                        if (cred.Exists())
                        {
                            cred.Delete();
                        }
                        cred.Username = UN.Text;
                        cred.SecurePassword = PW.SecurePassword;
                        cred.PersistanceType = PersistanceType.Enterprise;
                        cred.Save();
                    }
                }
                
                await Load_Filewatcher_Async();
            }            
        }

        public async Task Load_Filewatcher_Async()
        {
            App_Loading.Enabled = true;
            Dictionary<string,string> status=await Task.Run(()=> ((App)Application.Current).Run_FilewatcherAsync()) ;
            
            if (bool.Parse(status["Status"]) != true)
            {
                Status_Data_Instance = new Status_Data() { Visibility = Visibility.Visible, Status_Color = System.Windows.Media.Brushes.Red, Status = status["Exception"] };
                App_Loading.Enabled = false;
            }
            else
            {
                App_Loading.Enabled = false;
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
                Close();
            }   

            
        }

    }
}
