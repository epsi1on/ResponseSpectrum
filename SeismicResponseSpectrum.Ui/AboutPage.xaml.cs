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

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void mainUrlBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://responsespectrum.codeplex.com/");
        }

        private void DiscussionTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            System.Diagnostics.Process.Start(@"https://responsespectrum.codeplex.com/discussions");
        }
    }
}
