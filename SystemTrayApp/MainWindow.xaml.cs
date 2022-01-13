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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Uri Landing = new Uri("LandingPage.xaml", UriKind.Relative);
        public static MainWindow _instance;
        

        public MainWindow()
        {
            InitializeComponent();

            _instance = this;
            this.MainFrame.Source = Landing;
             

        }






        private void Minimise_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();

        }

        private void Minimise_MouseLeave(object sender, MouseEventArgs e)
        {
            Minimise.Opacity = 0.3;
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Close.Opacity = 0.3;
        }

        private void Minimise_MouseEnter(object sender, MouseEventArgs e)
        {
            Minimise.Opacity = 1;
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Close.Opacity = 1;

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content.ToString() == "SystemTrayApp.LandingPage") { Back.Visibility = Visibility.Collapsed; }
            else { Back.Visibility = Visibility.Visible; };

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = Landing;
        }


    }
}
