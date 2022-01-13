using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace SystemTrayApp
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class NotifyIconViewModel
    {
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null,
                    
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.MainWindow.Close(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }


        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand {CommandAction = () => Application.Current.Shutdown()};
            }
        }


        
    }


    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null  || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class Landingmethod
    {
        public SolidColorBrush ShadeColor(object sender, sbyte percent)
        {
            
            Byte R = Byte.Parse((sender as Button).Background.ToString().Substring(3, 2), System.Globalization.NumberStyles.HexNumber);

            Byte G = Byte.Parse((sender as Button).Background.ToString().Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            Byte B = Byte.Parse((sender as Button).Background.ToString().Substring(7, 2), System.Globalization.NumberStyles.HexNumber);

            R = Convert.ToByte((R != 0) ? Math.Max(0, Math.Min((R + percent), 255)) : R);
            G = Convert.ToByte((G != 0) ? Math.Max(0, Math.Min((G + percent), 255)) : G);
            B = Convert.ToByte((B != 0) ? Math.Max(0, Math.Min((B + percent), 255)) : B);


            string RR = R.ToString("X2");
            string GG = G.ToString("X2");
            string BB = B.ToString("X2");


            return new BrushConverter().ConvertFromString(("#FF" + RR + GG + BB)) as SolidColorBrush;
        }
    }


}