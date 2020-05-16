using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using FirstFloor.ModernUI.Windows.Controls;

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {

        public static MainWindow Current;

        public MainWindow()
        {
            Current = this;
            InitializeComponent();
        }


        public void ShowWaitSign()
        {
            if (!Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(ShowWaitSign));
                return;
            }

            this.IsEnabled = false;
        }

        public void HideWaitSign()
        {
            if (!Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(HideWaitSign));
                return;
            }

            this.IsEnabled = true;
        }
     
    }
}
