using System;
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

namespace SystemTrayApp
{
    /// <summary>
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : Page
    {
        ReadingsMonitoring ReadingsPage = new ReadingsMonitoring();
        NFS_Cleanup NFSCleanup = new NFS_Cleanup();
        
        public LandingPage()
        {
            InitializeComponent();
            
            
        }


        




        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ///e.Handled = true;
            if (e.ChangedButton == MouseButton.Left)
            {
                ///(sender as Path).Fill = shadeColor((sender as Path).Fill as SolidColorBrush, 20);
                if ((sender as Button) == RCM ) { this.NavigationService.Navigate(ReadingsPage); };
                if ((sender as Button) == NFSC) { this.NavigationService.Navigate(NFSCleanup); };
            }
        }

        private void Status_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                MainWindow._instance.DragMove();
            }
        }


        


    }


}
