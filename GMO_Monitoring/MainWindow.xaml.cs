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

namespace GMO_Monitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            Uri Landing = new Uri("LandingPage.xaml", UriKind.Relative);
            InitializeComponent();            
            this.MainFrame.Source = Landing;
            

        }

        public void MainFrame_Source(Uri path)
        {
            this.MainFrame.Source = path;
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

        private void Drag_MouseLeftButtonDown(object sender ,MouseEventArgs e)
        {
            Main.DragMove();
        }

        private void Drag_MouseEnter(object sender, MouseEventArgs e)
        {
            Drag.Opacity = 1;

        }

        private void Drag_MouseLeave(object sender, MouseEventArgs e)
        {
            Drag.Opacity = 0.3;

        }


    }
}
