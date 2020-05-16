using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Shapes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Win32;
using Xceed.Wpf.AvalonDock.Converters;
using Xceed.Wpf.Toolkit;
using CustomDraggablePoint = Microsoft.Research.DynamicDataDisplay.Charts.Shapes.DraggablePoint;
using MessageBox = System.Windows.MessageBox;

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Interaction logic for ThreeCharts.xaml
    /// </summary>
    public partial class ThreeCharts : UserControl
    {

        public static ThreeCharts Current;

        public ThreeCharts()
        {
            InitializeComponent();

            InitializeCharts();

            ApplicationSettings.Current.ActiveRecords.CollectionChanged += ActiveRecordsOnCollectionChanged;

            this.Loaded += OnLoaded;
            Current = this;

            stkLegends.Context.ChkCheckedChanged += (sender, e) =>
            {
                if (e.NewValue)
                    ShowMinMaxLines();
                else
                    HideMinMaxLines();
            };
        }


        /// <summary>
        /// Hides the extra stuff to be prepared for screen shot.
        /// </summary>
        private void HideExtraStuffToGetScreenshot()
        {
            stkLegends.Opacity = 0;
            
            AccelerationCharts.Children.Where(i => i is DraggablePoint)
                .Cast<DraggablePoint>()
                .ToList()
                .ForEach(i => i.Opacity = 0);

            var lineGraph =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "spect".Equals(i.Tag));

            lineGraph.Opacity = 0;

            stkLegends.Context.ChkChecked = false;

            btnExtraStuff.Opacity = 0.1;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //to just disable auto fit!
            var rc = AccelerationCharts.Visible;
            rc.X *= 0.99999;
            AccelerationCharts.Viewport.Visible = rc;

            /*
            rc = DisplacementCharts.Visible;
            rc.X *= 0.99999;
            DisplacementCharts.Viewport.Visible = rc;

            rc = VelocityCharts.Visible;
            rc.X *= 0.99999;
            VelocityCharts.Viewport.Visible = rc;
            */
        }

        private void ActiveRecordsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (SeismicRecordSpectrum rec in e.NewItems)
                        AddToCharts(rec);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (SeismicRecordSpectrum rec in e.OldItems)
                        RemoveFromCharts(rec);
                    break;
                    /*
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
                */
                case NotifyCollectionChangedAction.Reset:

                    foreach (SeismicRecordSpectrum rec in e.OldItems)
                        RemoveFromCharts(rec);
                    foreach (SeismicRecordSpectrum rec in e.NewItems)
                        AddToCharts(rec);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void AccelerationStretchButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FitToView(AccelerationCharts);
        }


        private void FitToView(ChartPlotter port)
        {
            var old = port.Viewport.AutoFitToView;
            port.Viewport.AutoFitToView = true;
            port.Viewport.FitToView();
            port.Viewport.AutoFitToView = old;
        }

        private bool SpectrumLineChanged = false;

        private void AccelerationCharts_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Tutorials.Visibility != Visibility.Visible)
                return;

            if (Tutorials.CurrentStep == null)
                return;

            if (!Equals(Tutorials.CurrentStep.Tag, 2))
                return;

            if (SpectrumLineChanged)
            {
                SpectrumLineChanged = false;
                Tutorials.CurrentStep.MarkAsDone();
            }

        }




        private void VelocityCharts_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Tutorials.Visibility != Visibility.Visible)
                return;

            if (Tutorials.CurrentStep == null)
                return;

            if (!Equals(Tutorials.CurrentStep.Tag, 1))
                return;

            if (VelocityChartViewChanged)
            {
                VelocityChartViewChanged = false;
                Tutorials.CurrentStep.MarkAsDone();
            }
        }

        private bool VelocityChartViewChanged = false;



        private void InitializeCharts()
        {
            AddSpectrumLines();


            UpdateSpectrumChart(AccelerationCharts);
            //UpdateSpectrumChart(DisplacementCharts);
            //UpdateSpectrumChart(VelocityCharts);

            AccelerationCharts.Legend.Visibility = Visibility.Collapsed;
            VelocityCharts.Legend.Visibility = Visibility.Collapsed;
            DisplacementCharts.Legend.Visibility = Visibility.Collapsed;


            /*
            var str3 = new StretchButton() { ToolTip = "Stretch chart into visible area", Plotter = AccelerationCharts };
            str3.PreviewMouseDown += Str1OnClick;
            str3.PreviewMouseMove += Str3OnPreviewMouseMove;
            str3.MouseMove+=Str3OnMouseMove;
            AccelerationCharts.GetPart<Grid>("PART_CentralGrid").Children.Add(str3);

            var str2 = new StretchButton() { ToolTip = "Stretch chart into visible area", Plotter = DisplacementCharts };
            str2.PreviewMouseDown += Str1OnClick;
            str2.PreviewMouseMove += Str3OnPreviewMouseMove;
            DisplacementCharts.GetPart<Grid>("PART_CentralGrid").Children.Add(str2);

            var str1 = new StretchButton() { ToolTip = "Stretch chart into visible area", Plotter = VelocityCharts };
            str1.PreviewMouseDown += Str1OnClick;
            str1.PreviewMouseMove += Str3OnPreviewMouseMove;
            VelocityCharts.GetPart<Grid>("PART_CentralGrid").Children.Add(str1);
            */
        }

        private void AddSpectrumLines()
        {
            /*
            var pv0 = new CustomDraggablePoint(new Point(0, 1));
            var pv1 = new CustomDraggablePoint(new Point(0.1, 2.5));
            var pv2 = new CustomDraggablePoint(new Point(0.5, 2.5));

            Canvas.SetZIndex(pv0, 10000 + 1);
            Canvas.SetZIndex(pv1, 10000 + 2);
            Canvas.SetZIndex(pv2, 10000 + 3);

            pv0.PositionChanged += OnDraggablePointPositionChanged;
            pv1.PositionChanged += OnDraggablePointPositionChanged;
            pv2.PositionChanged += OnDraggablePointPositionChanged;

            VelocityCharts.Children.Add(pv0);
            VelocityCharts.Children.Add(pv1);
            VelocityCharts.Children.Add(pv2);




            var pd0 = new CustomDraggablePoint(new Point(0, 1));
            var pd1 = new CustomDraggablePoint(new Point(.1, 2.5));
            var pd2 = new CustomDraggablePoint(new Point(.5, 2.5));

            Canvas.SetZIndex(pd0, 10000 + 1);
            Canvas.SetZIndex(pd1, 10000 + 2);
            Canvas.SetZIndex(pd2, 10000 + 3);

            pd0.PositionChanged += OnDraggablePointPositionChanged;
            pd1.PositionChanged += OnDraggablePointPositionChanged;
            pd2.PositionChanged += OnDraggablePointPositionChanged;


            DisplacementCharts.Children.Add(pd0);
            DisplacementCharts.Children.Add(pd1);
            DisplacementCharts.Children.Add(pd2);


            */

            var pa0 = new CustomDraggablePoint(new Point(0, 1));
            var pa1 = new CustomDraggablePoint(new Point(.1, 2.5));
            var pa2 = new CustomDraggablePoint(new Point(.5, 2.5));

            Canvas.SetZIndex(pa0, 1000 - 1);
            Canvas.SetZIndex(pa1, 1000 - 2);
            Canvas.SetZIndex(pa2, 1000 - 3);

            pa0.PositionChanged += OnDraggablePointPositionChanged;
            pa1.PositionChanged += OnDraggablePointPositionChanged;
            pa2.PositionChanged += OnDraggablePointPositionChanged;

            AccelerationCharts.Children.Add(pa0);
            AccelerationCharts.Children.Add(pa1);
            AccelerationCharts.Children.Add(pa2);
        }

        public void UpdateAverageAccelResponseLine()
        {
            var avgPts = new List<Point>();
            var maxPts = new List<Point>();

            var dt = ApplicationSettings.Current.SpectrumsDt;
            var tmax = ApplicationSettings.Current.SpectrumsTMax;

            var m = tmax/dt;

            var recs = ApplicationSettings.Current.ActiveRecords.Select(i => i.AccelerationSpectrum.ToArray()).ToArray();

            for (var i = 0; i < m; i++)
            {
                var max = 0.0;
                var sum = 0.0;

                foreach (var rec in recs)
                {
                    sum += rec[i].Y;
                    max = Math.Max(max, rec[i].Y);
                }

                avgPts.Add(new Point(i*dt, sum/recs.Length));
                maxPts.Add(new Point(i*dt, max));

            }



            var avgGr1 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "avgaccel1".Equals(i.Tag));

            var avgGr2 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "avgaccel2".Equals(i.Tag));


            var maxGr1 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "maxaccel1".Equals(i.Tag));

            var maxGr2 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "maxaccel2".Equals(i.Tag));



            if (avgGr1 == null)
            {

                {//for avgGr
                    var pen1 = new Pen(Brushes.Green, 4);
                    var pen2 = new Pen(Brushes.White, 2);

                    pen2.DashStyle = DashStyles.Dash;
                    pen1.DashCap = pen1.DashCap = PenLineCap.Round;

                    var source = new ObservableDataSource<Point>();
                    {
                        avgGr1 = AccelerationCharts.AddLineGraph(source, pen1, null);
                        avgGr2 = AccelerationCharts.AddLineGraph(source, pen2, null);
                    }

                    avgGr1.Tag = "avgaccel1";
                    avgGr2.Tag = "avgaccel2";

                    avgGr1.ToolTip = avgGr2.ToolTip = "Average of all records";

                    Canvas.SetZIndex(avgGr1, 10001);
                    Canvas.SetZIndex(avgGr2, 10002);
                }


                {//for maxGr
                    var pen1 = new Pen(Brushes.Red, 4);
                    var pen2 = new Pen(Brushes.White, 2);

                    pen2.DashStyle = DashStyles.Dash;
                    pen1.DashCap = pen1.DashCap = PenLineCap.Round;

                    var source = new ObservableDataSource<Point>();
                    {
                        maxGr1 = AccelerationCharts.AddLineGraph(source, pen1, null);
                        maxGr2 = AccelerationCharts.AddLineGraph(source, pen2, null);
                    }

                    maxGr1.Tag = "maxaccel1";
                    maxGr2.Tag = "maxaccel2";

                    maxGr1.ToolTip = maxGr2.ToolTip = "Maximum of all records";

                    Canvas.SetZIndex(maxGr1, 10003);
                    Canvas.SetZIndex(maxGr2, 10004);
                }

                StartBlinkForEver(avgGr2, maxGr2);
            }


            var avgSrc = avgGr1.DataSource as ObservableDataSource<Point>;
            var maxSrc = maxGr1.DataSource as ObservableDataSource<Point>;

            avgSrc.Collection.Clear();
            maxSrc.Collection.Clear();


            avgSrc.AppendMany(avgPts);
            maxSrc.AppendMany(maxPts);

            if (ApplicationSettings.Current.ActiveRecords.Count < 2)
            {
                    stkLegends.Visibility =
                        avgGr1.Visibility = avgGr2.Visibility = maxGr1.Visibility = maxGr2.Visibility = Visibility.Collapsed;    
                
            }
            else
            {
                if (stkLegends.Context.ChkChecked)
                    stkLegends.Visibility =
                        avgGr1.Visibility =
                            avgGr2.Visibility =
                                maxGr1.Visibility =
                                    maxGr2.Visibility = Visibility.Visible;
            }
        }


        public void ShowMinMaxLines()
        {
            var avgGr1 =
               AccelerationCharts.Children.Where(i => i is LineGraph)
                   .Cast<LineGraph>()
                   .FirstOrDefault(i => "avgaccel1".Equals(i.Tag));

            var avgGr2 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "avgaccel2".Equals(i.Tag));


            var maxGr1 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "maxaccel1".Equals(i.Tag));

            var maxGr2 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "maxaccel2".Equals(i.Tag));

            if (avgGr1 == null || avgGr2 == null || maxGr1 == null || maxGr2 == null)
                return;

            avgGr1.Visibility = avgGr2.Visibility = maxGr1.Visibility = maxGr2.Visibility = Visibility.Visible;
        }

        public void HideMinMaxLines()
        {
            var avgGr1 =
              AccelerationCharts.Children.Where(i => i is LineGraph)
                  .Cast<LineGraph>()
                  .FirstOrDefault(i => "avgaccel1".Equals(i.Tag));

            var avgGr2 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "avgaccel2".Equals(i.Tag));


            var maxGr1 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "maxaccel1".Equals(i.Tag));

            var maxGr2 =
                AccelerationCharts.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "maxaccel2".Equals(i.Tag));

            if (avgGr1 == null || avgGr2 == null || maxGr1 == null || maxGr2 == null)
                return;

            avgGr1.Visibility = avgGr2.Visibility = maxGr1.Visibility = maxGr2.Visibility = Visibility.Hidden;
        }

        private void StartBlinkForEver(params LineGraph[] lines)
        {
            var anim = new DoubleAnimation
            {
                From = 0.6,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.4)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            foreach (var ln in lines)
            {
                ln.BeginAnimation(LineGraph.OpacityProperty, anim);
                
            }
        }

        private void OnDraggablePointPositionChanged(object sender, PositionChangedEventArgs e)
        {
            var draggablePoint = sender as DraggablePoint;

            if (draggablePoint == null)
                return;

            var plotter = draggablePoint.Plotter as ChartPlotter;

            UpdateSpectrumChart(plotter);
        }

        private void UpdateSpectrumChart(ChartPlotter parentPlotter)
        {
            if (Tutorials.Visibility == Visibility.Visible)
                if (Tutorials.CurrentStep != null)
                    if (Equals(Tutorials.CurrentStep.Tag, 2))
                        SpectrumLineChanged = true;


            var allPoints = GetAllPoints(parentPlotter).OrderBy(i => i.X).ToList();

            if (allPoints.Count == 0)
                return;

            var splus1 = Math.Max(allPoints[1].Y, allPoints[2].Y);

            var m = (allPoints[0].Y - allPoints[1].Y) / (allPoints[0].X - allPoints[1].X);

            var x0 = 0.0;
            var y0 = -m * allPoints[0].X + allPoints[0].Y;

            var x1 = (splus1 - allPoints[0].Y) / m + allPoints[0].X;
            var y1 = splus1;





            var lineGraph =
                parentPlotter.Children.Where(i => i is LineGraph)
                    .Cast<LineGraph>()
                    .FirstOrDefault(i => "spect".Equals(i.Tag));

            if (lineGraph == null)
            {
                var pen = new Pen(Brushes.Black, 5);
                pen.DashStyle = DashStyles.Dash;
                pen.DashCap = PenLineCap.Round;

                var source = new ObservableDataSource<Point>();
                lineGraph = parentPlotter.AddLineGraph(source, pen, null);
                lineGraph.Tag = "spect";
                Canvas.SetZIndex(lineGraph, 10000);
            }


            var src = lineGraph.DataSource as ObservableDataSource<Point>;

            src.Collection.Clear();


            var pts = new List<Point>();

            pts.Add(new Point(x0, y0));
            pts.Add(new Point(x1, y1));
            pts.Add(new Point(allPoints[2].X, splus1));

            var tmax = ApplicationSettings.Current.SpectrumsTMax;
            var dt = ApplicationSettings.Current.SpectrumsDt;

            for (var t = allPoints[2].X; t < tmax; t += dt)
            {
                var x = (double)(allPoints[2].X / t);

                var y = splus1 * Math.Pow(x * x, 1 / 3.0);

                pts.Add(new Point(t, y));
            }

            src.AppendMany(pts);

        }


        private List<Point> GetAllPoints(ChartPlotter plotter)
        {
            var buf = new List<Point>();

            foreach (var child in plotter.Children)
                if (child is DraggablePoint)
                    buf.Add((child as DraggablePoint).Position);

            return buf;
        }



        private void DisplacementStretchButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FitToView(DisplacementCharts);
        }

        private void VelocityStretchButton_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            FitToView(VelocityCharts);
            if (Tutorials.Visibility != Visibility.Visible)
                return;

            if (Tutorials.CurrentStep == null)
                return;

            if (!Equals(Tutorials.CurrentStep.Tag, 3))
                return;

            Tutorials.CurrentStep.MarkAsDone();

        }



        private void RemoveFromCharts(SeismicRecordSpectrum rec)
        {
            foreach (var chart in AccelerationCharts.Children)
            {
                if (chart is LineGraph)
                {
                    if ((chart as LineGraph).DataSource is CustomObservableDataSource)
                    {
                        var src = ((chart as LineGraph).DataSource as CustomObservableDataSource);
                        if (ReferenceEquals(src.Record, rec))
                        {
                            AccelerationCharts.Children.Remove(chart);
                            break;
                        }
                    }
                }
            }


            foreach (var chart in VelocityCharts.Children)
            {
                if (chart is LineGraph)
                {
                    if ((chart as LineGraph).DataSource is CustomObservableDataSource)
                    {
                        var src = ((chart as LineGraph).DataSource as CustomObservableDataSource);
                        if (ReferenceEquals(src.Record, rec))
                        {
                            VelocityCharts.Children.Remove(chart);
                            break;
                        }
                    }
                }
            }


            foreach (var chart in DisplacementCharts.Children)
            {
                if (chart is LineGraph)
                {
                    if ((chart as LineGraph).DataSource is CustomObservableDataSource)
                    {
                        var src = ((chart as LineGraph).DataSource as CustomObservableDataSource);
                        if (ReferenceEquals(src.Record, rec))
                        {
                            DisplacementCharts.Children.Remove(chart);
                            break;
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Adds to charts.
        /// </summary>
        /// <param name="rec">The record.</param>
        private void AddToCharts(SeismicRecordSpectrum rec)
        {
            var pen = new Pen();

            BindingOperations.SetBinding(pen, Pen.ThicknessProperty,
                new Binding("ChartPenThickness") { Source = ApplicationSettings.Current });

            var brush = new SolidColorBrush();
            BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty,
                new Binding("Color") { Source = rec });

            pen.Brush = brush;

            var a = new CustomObservableDataSource(rec.AccelerationSpectrum);
            rec.AccelerationSpectrum.CollectionChanged += (sender, e) => this.DispatcherInvoke(() =>
            {
                a.Collection.Clear();
                a.AppendMany(sender as FastObservableCollection<Point2D>);
            });


            a.SetXMapping(i => i.X);
            a.SetYMapping(i => i.Y);
            a.Record = rec;

            var aGr = AccelerationCharts.AddLineGraph(a, pen, new PenDescription());
            aGr.DataContext = rec;
            aGr.SetBinding(LineGraph.VisibilityProperty, new Binding("IsVisible") { Converter = new BoolToVisibilityConverter() });


            var v = new CustomObservableDataSource(rec.SpeedSpectrum);
            rec.SpeedSpectrum.CollectionChanged += (sender, e) => this.DispatcherInvoke(() =>
            {
                v.Collection.Clear();
                v.AppendMany(sender as FastObservableCollection<Point2D>);
            });


            v.SetXMapping(i => i.X);
            v.SetYMapping(i => i.Y);
            v.Record = rec;

            var vGr = VelocityCharts.AddLineGraph(v, pen, new PenDescription());
            vGr.DataContext = rec;
            vGr.SetBinding(LineGraph.VisibilityProperty, new Binding("IsVisible") { Converter = new BoolToVisibilityConverter() });

            var d = new CustomObservableDataSource(rec.DisplacementSpectrum);
            rec.DisplacementSpectrum.CollectionChanged += (sender, e) => this.DispatcherInvoke(() =>
            {
                d.Collection.Clear();
                d.AppendMany(sender as FastObservableCollection<Point2D>);
            });


            d.SetXMapping(i => i.X);
            d.SetYMapping(i => i.Y);
            d.Record = rec;

            var dGr = DisplacementCharts.AddLineGraph(d, pen, new PenDescription());
            dGr.DataContext = rec;
            dGr.SetBinding(LineGraph.VisibilityProperty, new Binding("IsVisible") { Converter = new BoolToVisibilityConverter() });

        }

        public void RefreshUI()
        {
            UpdateAverageAccelResponseLine();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var dg = new ExportDiagramWindow();
                var img = Utils.LoadImage(this.GetPngImage(96));
                dg.Context.Preview = img;

                var res = dg.ShowDialog();

                if (!res.HasValue)
                    return;

                if (!res.Value)
                    return;

                var exportToPng = false;
                var filter = "";
                if ((exportToPng = dg.Context.ExportPng) == true)
                    filter = "Portable Network Graphics|*.png";
                else
                    filter = "eXtensible Application Markup Language|*.xaml";

                var dlg2 = new SaveFileDialog();

                dlg2.Filter = filter;

                var res2 = dlg2.ShowDialog();

                if (!res2.HasValue)
                    return;

                if (!res2.Value)
                    return;

                var address = dlg2.FileName;

                if (exportToPng)
                {
                    var bytes = this.GetPngImage(dg.Context.PngDpi);
                    System.IO.File.WriteAllBytes(address, bytes);
                    System.Diagnostics.Process.Start(address);
                }
                else
                {
                    var xaml = XamlWriter.Save(this);
                    var doc = new XmlDocument();
                    doc.LoadXml(xaml);
                    doc.Save(address);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HideExtraStuffToGetScreenshot();
        }
    }
}
