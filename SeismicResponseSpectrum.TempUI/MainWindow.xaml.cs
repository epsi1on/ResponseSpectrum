using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using SeismicResponseSpectrum.Ui;

namespace SeismicResponseSpectrum.TempUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ConsoleManager.Show();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ImportAccelerogramDialog();

            if (!dlg.ShowDialog().Value)
                return;

            var rec = dlg.Record;

            rec.Color = Colors.Red;
            rec.ComputeResponseSpectrums();
            
            ApplicationSettings.Current.ActiveRecords.Add(rec);

            CopyToClipboard(
                rec.GroundAccelerationHistory, rec.GroundVelocityHistory, rec.GroundDisplacementHistory
                //rec.AccelerationValues, rec.VelocityValues, rec.DisplacementValues
                );

            CopyToClipboard(
                //rec.GroundAccelerationHistory, rec.GroundVelocityHistory, rec.GroundDisplacementHistory
                rec.AccelerationValues, rec.VelocityValues, rec.DisplacementValues
                );

        }


        private void CopyToClipboard(params IList<Point2D>[] pts)
        {
            var sb = new StringBuilder();

            var n = pts[0].Count;

            for (int i = 0; i < n; i++)
            {
                sb.Append(pts[0][i].X);
                sb.Append("\t");

                for (int j = 0; j < pts.Length; j++)
                {
                    //sb.Append(pts[j][i].X);
                    
                    sb.Append(pts[j][i].Y);
                    sb.Append("\t");
                }
                sb.AppendLine();

            }
            foreach (var pts2 in pts)
            {

               // sb.AppendLine(string.Format("{0}\t{1}", pt.X, pt.Y));
            }

            Clipboard.SetText(sb.ToString());
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
