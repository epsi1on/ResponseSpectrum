using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
using System.Xml;
using Epsi1on.MathAndGeometric.Geometry;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Win32;
using Xceed.Wpf.AvalonDock.Converters;

namespace SeismicResponseSpectrum.Ui
{
    [Serializable]
    public class GridlineInfo
    {
        public double F;
        public double Sv;
        public double Sd;
        public double Sa;
    }

    /// <summary>
    /// Interaction logic for TripartiteSpectrum.xaml
    /// </summary>
    public partial class TripartiteSpectrum : UserControl
    {
        public static TripartiteSpectrum Current;

        public TripartiteSpectrum()
        {
            this.InitializeComponent();

            this.SizeChanged += OnSizeChanged;

            ApplicationSettings.Current.ActiveRecords.CollectionChanged += RecordsOnCollectionChanged;


            this.MouseLeftButtonDown += OnMouseLeftButtonDown;
            this.MouseLeftButtonUp += OnMouseLeftButtonUp;
            this.MouseMove += OnMouseMove;

            TripartiteSpectrum.Current = this;
        }

        #region panning

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.IsMouseCaptured)
                return;

            var t1 = GetTranslateTrans(SpectrumsCanvas);

            var v = last - e.GetPosition(SpectrumsCanvas);

            t1.X -= v.X;
            t1.Y -= v.Y;

            {
                SetTransformationParameters();

                f0 = sf0*Math.Pow(10, (-t1.X*sc));
                f1 = sf1*Math.Pow(10, (-t1.X*sc));
                sv0 = ssv0*Math.Pow(10, (t1.Y*sc));

                SetTransformationParameters();

                GridsCanvas.Children.Clear();
                AddGridLines();
            }

            last = e.GetPosition(SpectrumsCanvas);
        }


        private Point last;

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            last = e.GetPosition(SpectrumsCanvas);

            this.CaptureMouse();
        }

        private TranslateTransform GetTranslateTrans(UIElement elm)
        {
            var tr = elm.RenderTransform;

            if (tr is TranslateTransform)
                return tr as TranslateTransform;

            var trg = tr as TransformGroup;

            if (trg == null)
                return null;

            var ttr = trg.Children.FirstOrDefault(i => i is TranslateTransform);

            return (TranslateTransform) ttr;
        }

        #endregion

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            RefreshUI();
        }

        /*
	    #region Records Property and Property Change Routed event

	    public static readonly DependencyProperty RecordsProperty
	        = DependencyProperty.Register(
	            "Records", typeof (FastObservableCollection<SeismicRecordSpectrum>), typeof (TripartiteSpectrum),
	            new PropertyMetadata(new FastObservableCollection<SeismicRecordSpectrum>(), OnRecordsChanged, RecordsCoerceValue));

	    public FastObservableCollection<SeismicRecordSpectrum> Records
	    {
	        get { return (FastObservableCollection<SeismicRecordSpectrum>) GetValue(RecordsProperty); }
	        set { SetValue(RecordsProperty, value); }
	    }

	    public static readonly RoutedEvent RecordsChangedEvent
	        = EventManager.RegisterRoutedEvent(
	            "RecordsChanged",
	            RoutingStrategy.Direct,
	            typeof (RoutedPropertyChangedEventHandler<FastObservableCollection<SeismicRecordSpectrum>>),
	            typeof (TripartiteSpectrum));

	    private static object RecordsCoerceValue(DependencyObject d, object value)
	    {
	        var val = (FastObservableCollection<SeismicRecordSpectrum>) value;
	        var obj = (TripartiteSpectrum) d;


	        return value;
	    }

	    public event RoutedPropertyChangedEventHandler<FastObservableCollection<SeismicRecordSpectrum>> RecordsChanged
	    {
	        add { AddHandler(RecordsChangedEvent, value); }
	        remove { RemoveHandler(RecordsChangedEvent, value); }
	    }

	    private static void OnRecordsChanged(
	        DependencyObject d,
	        DependencyPropertyChangedEventArgs e)
	    {
	        var obj = d as TripartiteSpectrum;
	        var args = new RoutedPropertyChangedEventArgs<FastObservableCollection<SeismicRecordSpectrum>>(
	            (FastObservableCollection<SeismicRecordSpectrum>) e.OldValue,
	            (FastObservableCollection<SeismicRecordSpectrum>) e.NewValue);
	        args.RoutedEvent = TripartiteSpectrum.RecordsChangedEvent;
	        obj.RaiseEvent(args);

            var newVal = e.NewValue as FastObservableCollection<SeismicRecordSpectrum>;
            var oldVal = e.OldValue as FastObservableCollection<SeismicRecordSpectrum>;


            if (newVal != null)
                newVal.CollectionChanged += obj.RecordsOnCollectionChanged;
            
            if (oldVal != null)
                oldVal.CollectionChanged -= obj.RecordsOnCollectionChanged;
	    }

        

        #endregion
        */

        private void RecordsOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            SpectrumsCanvas.Children.Clear();
            RefreshUI();
        }

        #region Transformation Parameters

        /**/

        /// <summary>
        /// Starting Min frequency
        /// </summary>
        private double sf0;

        /// <summary>
        /// Starting Max frequency
        /// </summary>
        private double sf1;

        /// <summary>
        /// Starting Min Sv
        /// </summary>
        private double ssv0;

        /**/

        /// <summary>
        /// Current Min frequency
        /// </summary>
        private double f0;

        /// <summary>
        /// Current Max frequency
        /// </summary>
        private double f1;

        /// <summary>
        /// Current Min Sv
        /// </summary>
        private double sv0;


        /// <summary>
        /// Min Sd
        /// </summary>
        private double sd0;

        /// <summary>
        /// Min Sa
        /// </summary>
        private double sa0;

        /// <summary>
        /// W (width)
        /// </summary>
        private double w;

        /// <summary>
        /// H (height)
        /// </summary>
        private double h;

        /// <summary>
        /// Sc (scale constant)
        /// </summary>
        private double sc;

        /// <summary>
        /// Max diagonal length!
        /// </summary>
        private double dp;

        //private double? OverridedF0 = null, OverridedF1 = null, OverridedSv0 = null;

        private void ResetTrnasformParameters()
        {
            f0 = f1 = sv0 = 0;
        }

        private void SetTransformationParameters()
        {
            w = GridsCanvas.ActualWidth;
            h = GridsCanvas.ActualHeight;

            sf0 = 0.9*ApplicationSettings.Current.TripartiteMinFrequency;
            sf1 = 1.1*ApplicationSettings.Current.TripartiteMaxFrequency;

            ssv0 = ApplicationSettings.Current.TripartiteMinSv;

            if (f0 == 0) f0 = sf0;
            if (f1 == 0) f1 = sf1;
            if (sv0 == 0) sv0 = ssv0;


            var logsv0 = Math.Log10(sv0);
            var logf0 = Math.Log10(f0);
            var logf1 = Math.Log10(f1);

            sc = (logf1 - logf0)/w;

            var log2pi = Math.Log10(2*Math.PI);

            var logsd0 = logsv0 - (logf1 + log2pi);
            var logsa0 = logsv0 + (logf0 + log2pi);

            sd0 = Math.Pow(10, logsd0);
            sa0 = Math.Pow(10, logsa0);

            dp = sc*Math.Max(h, w);
        }

        #region transformation of coordinates

        /// <summary>
        /// get f and Sv, return X and Y
        /// </summary>
        /// <param name="fsvs">f and Sv of points</param>
        /// <returns>X and Y</returns>
        private Point[] fn1(Point[] fsvs)
        {
            var buf = new Point[fsvs.Length];

            for (int i = 0; i < fsvs.Length; i++)
            {
                var fsv = fsvs[i];
                buf[i] = new Point(1/sc*(log(fsv.X/f0)), 1/sc*(Math.Log10(fsv.Y/sv0)));
            }

            return buf;
        }

        /// <summary>
        /// get Sd and Sa, return X and Y
        /// </summary>
        /// <param name="sdsa">Sa Sd.</param>
        /// <returns></returns>
        private Point fn2(Point sdsa)
        {
            var f = 0.5/Math.PI*Math.Sqrt(sdsa.Y/sdsa.X);
            var sv = Math.Sqrt(sdsa.X*sdsa.Y);

            return new Point(1/sc*(log(f/f0)), 1/sc*(Math.Log10(sv/sv0)));
        }

        #endregion

        private double log(double v)
        {
            return Math.Log10(v);
        }

        #endregion

        #region Adding grid lines and labels

        private List<Line> FGridLines = new List<Line>();
        private List<Line> SvGridLines = new List<Line>();
        private List<Line> SdGridLines = new List<Line>();
        private List<Line> SaGridLines = new List<Line>();

        private void AddGridLines()
        {
            if (this.ActualHeight == 0 || this.ActualWidth == 0)
                return;


            SaGridLines.Clear();
            SvGridLines.Clear();
            SdGridLines.Clear();
            FGridLines.Clear();


            var majorFs = GetMajorLogTicks(f0, f1);
            var minorFs = GetMinorLogTicks(f0, f1);

            var sv1 = sv0*Math.Pow(10, sc*h);
            var majorSvs = GetMajorLogTicks(sv0, sv1);
            var minorSvs = GetMinorLogTicks(sv0, sv1);

            var sd1 = sd0*Math.Pow(10, 2*dp);
            var majorSds = GetMajorLogTicks(sd0, sd1);
            var minorSds = GetMinorLogTicks(sd0, sd1);

            var sa1 = sa0*Math.Pow(10, 2*dp);
            var majorSas = GetMajorLogTicks(sa0, sa1);
            var minorSas = GetMinorLogTicks(sa0, sa1);

            //txtPos.Text = sv1.ToString();

            {
                //Vertical lines

                foreach (var f in majorFs)
                {
                    var ln = AddVerticalLine(1 / sc * log(f / f0));

                   


                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MajorGridLinesColor);
                        ln.StrokeThickness = this.MajorGridLinesThickness;

                        ln.Tag = new GridlineInfo() { F = f };
                        FGridLines.Add(ln);
                        ln.ToolTip = f.ToString();
                    }
                }

                foreach (var f in minorFs)
                {
                    var ln = AddVerticalLine(1 / sc * log(f / f0));

                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MinorGridLinesColor);
                        ln.StrokeThickness = this.MinorGridLinesThickness;
                    }

                }
            }

            {
//Horizontal lines
                foreach (var sv in majorSvs)
                {
                    var ln = AddHorizontalLine(1 / sc * log(sv / sv0));
                   


                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MajorGridLinesColor);
                        ln.StrokeThickness = this.MajorGridLinesThickness;

                        ln.Tag = new GridlineInfo() { Sv = sv };
                        SvGridLines.Add(ln);
                    }
                }

                foreach (var sv in minorSvs)
                {
                    var ln = AddHorizontalLine(1 / sc * log(sv / sv0));

                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MinorGridLinesColor);
                        ln.StrokeThickness = this.MinorGridLinesThickness;
                    }
                }
            }


            {
//Sd lines
                foreach (var sd in majorSds)
                {
                    var ln = AddDiagonalIncreasingLine(fn2(new Point(sd, sa0)));

                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MajorGridLinesColor);
                        ln.StrokeThickness = this.MajorGridLinesThickness;

                        ln.Tag = new GridlineInfo() { Sd = sd };
                        SdGridLines.Add(ln);
                    }
                }

                foreach (var sd in minorSds)
                {
                    var ln = AddDiagonalIncreasingLine(fn2(new Point(sd, sa0)));
                    
                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MinorGridLinesColor);
                        ln.StrokeThickness = this.MinorGridLinesThickness;
                    }
                }
            }


            {
//Sa lines
                foreach (var sa in majorSas)
                {
                    var ln = AddDiagonalDecreasingLine(fn2(new Point(sd0, sa)));

                    if (ln != null)
                    {
                        ln.Stroke = new SolidColorBrush(this.MajorGridLinesColor);
                        ln.StrokeThickness = this.MajorGridLinesThickness;

                        ln.Tag = new GridlineInfo() { Sa = sa };
                        SaGridLines.Add(ln);
                    }
                }


                foreach (var sa in minorSas)
                {
                    var ln = AddDiagonalDecreasingLine(fn2(new Point(sd0, sa)));

                    if (ln != null)
                    {

                        ln.Stroke = new SolidColorBrush(this.MinorGridLinesColor);
                        ln.StrokeThickness = this.MinorGridLinesThickness;
                    }
                }
            }

            AddGridLineLables();
        }


        private void AddGridLineLables()
        {
            #region verticals

            foreach (var ln in FGridLines)
            {
                var inf = ln.Tag as GridlineInfo;

                if (inf == null)
                    continue;

                var f = inf.F;

                var pos = new Point(ln.X1, Math.Max(ln.Y1, ln.Y2));

                var txt = new TextBlock();
                //txt.Background = Brushes.White;
                //txt.RenderTransformOrigin = new Point(0.5, 0.5);
                //txt.LayoutTransform = new RotateTransform(90);

                txt.TextAlignment = TextAlignment.Right;
                txt.HorizontalAlignment = HorizontalAlignment.Center;

                txt.Text = f.ToString();

                txt.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                txt.Arrange(new Rect(0, 0, 1000, 1000));

                Canvas.SetLeft(txt, pos.X - txt.ActualWidth/2);
                Canvas.SetTop(txt, pos.Y);

                GridsCanvas.Children.Add(txt);
            }

            #endregion

            #region horizontals

            foreach (var ln in SvGridLines)
            {
                var inf = ln.Tag as GridlineInfo;

                if (inf == null)
                    continue;

                var sv = inf.Sv;

                var pos = new Point(ln.X1, Math.Min(ln.Y1, ln.Y2));

                var txt = new TextBlock();

                //txt.TextAlignment = TextAlignment.Left;
                txt.HorizontalAlignment = HorizontalAlignment.Center;
                txt.VerticalAlignment = VerticalAlignment.Center;

                txt.Text = sv.ToString();

                txt.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                txt.Arrange(new Rect(0, 0, 1000, 1000));

                Canvas.SetLeft(txt, pos.X - txt.ActualWidth);
                Canvas.SetTop(txt, pos.Y - txt.ActualHeight/2);

                GridsCanvas.Children.Add(txt);
            }

            #endregion

            #region Sas

            var maxLengthSd = SdGridLines.OrderBy(i => i.GetLength()).LastOrDefault();

            {
                foreach (var saln in SaGridLines)
                {
                    var seg1 =
                        new LineSegment2D(
                            new Epsi1on.MathAndGeometric.Geometry.Point2D(maxLengthSd.X1, maxLengthSd.Y1),
                            new Epsi1on.MathAndGeometric.Geometry.Point2D(maxLengthSd.X2, maxLengthSd.Y2));


                    var seg2 = new LineSegment2D(
                        new Epsi1on.MathAndGeometric.Geometry.Point2D(saln.X1, saln.Y1),
                        new Epsi1on.MathAndGeometric.Geometry.Point2D(saln.X2, saln.Y2));

                    var collisionStat = LineSegment2D.GetIntersection(seg1, seg2);

                    if (collisionStat.LocusType != LocusType.OnePointIntersection)
                        continue;

                    var collision = collisionStat.IntersectionPoint.Value;

                    var tb = new TextBlock();
                    tb.Text = ((GridlineInfo) saln.Tag).Sa.ToString();

                    Canvas.SetLeft(tb, collision.X);
                    Canvas.SetTop(tb, collision.Y);
                    tb.LayoutTransform = new RotateTransform(45);
                    tb.Background = Brushes.White;

                    GridsCanvas.Children.Add(tb);
                }
            }

            #endregion

            #region Sds

            var maxLengthSa = SaGridLines.OrderBy(i => i.GetLength()).LastOrDefault();

            {
                foreach (var sdln in SdGridLines)
                {
                    var seg1 =
                        new LineSegment2D(
                            new Epsi1on.MathAndGeometric.Geometry.Point2D(maxLengthSa.X1, maxLengthSa.Y1),
                            new Epsi1on.MathAndGeometric.Geometry.Point2D(maxLengthSa.X2, maxLengthSa.Y2));


                    var seg2 = new LineSegment2D(
                        new Epsi1on.MathAndGeometric.Geometry.Point2D(sdln.X1, sdln.Y1),
                        new Epsi1on.MathAndGeometric.Geometry.Point2D(sdln.X2, sdln.Y2));

                    var collisionStat = LineSegment2D.GetIntersection(seg1, seg2);

                    if (collisionStat.LocusType != LocusType.OnePointIntersection)
                        continue;

                    var collision = collisionStat.IntersectionPoint.Value;

                    var tb = new TextBlock();
                    tb.Text = ((GridlineInfo) sdln.Tag).Sd.ToString();

                    //tb.TextAlignment = TextAlignment.Right;
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.VerticalAlignment = VerticalAlignment.Center;

                    //tb.Text = f.ToString();

                    tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    tb.Arrange(new Rect(0, 0, 1000, 1000));

                    Canvas.SetLeft(tb, collision.X);
                    //var hhh = ;
                    Canvas.SetTop(tb, collision.Y - tb.ActualHeight*0.71);
                    //tb.RenderTransformOrigin = new Point(0, 0);
                    tb.RenderTransform = new RotateTransform(-45);
                    tb.Background = Brushes.White;

                    GridsCanvas.Children.Add(tb);
                }
            }

            #endregion

            {
                //for acceleration
                var x = Math.Max(maxLengthSd.X1, maxLengthSd.X2);
                var y = Math.Min(maxLengthSd.Y1, maxLengthSd.Y2);

                var txt = new TextBlock();
                txt.Text = "Pseudo Acceleration m/s2";
                txt.LayoutTransform = new RotateTransform(-45);
                txt.RenderTransformOrigin = new Point(0, 1);
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;

                //tb.Text = f.ToString();

                txt.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                txt.Arrange(new Rect(0, 0, 1000, 1000));

                Canvas.SetLeft(txt, x - 0.7071*txt.ActualWidth - 0.707*txt.ActualHeight -1.41*txt.ActualHeight);
                Canvas.SetTop(txt, y );
                txt.Background = Brushes.White;
                GridsCanvas.Children.Add(txt);

            }



            {
                //for displacement
                var x = Math.Min(maxLengthSa.X1, maxLengthSa.X2);
                var y = Math.Min(maxLengthSa.Y1, maxLengthSa.Y2);

                var txt = new TextBlock();
                txt.Text = "Displacement m";
                txt.LayoutTransform = new RotateTransform(45);
                Canvas.SetLeft(txt, x);
                Canvas.SetTop(txt, y + 14);
                txt.Background = Brushes.White;
                GridsCanvas.Children.Add(txt);

            }
        }

        #endregion

        private void ResetRenderTransforms(UIElement elm)
        {
            var tr = new TransformGroup();
            tr.Children.Add(new ScaleTransform(1, 1));
            tr.Children.Add(new SkewTransform(0, 0));
            tr.Children.Add(new RotateTransform(0));
            tr.Children.Add(new TranslateTransform(0, 0));
            elm.RenderTransform = tr;
        }

        private void AddSpectrums()
        {
            var records = ApplicationSettings.Current.ActiveRecords;


            ResetRenderTransforms(SpectrumsCanvas);
            ResetTrnasformParameters();
            SetTransformationParameters();

            foreach (var rec in records)
            {
                var pts = new Point[rec.DisplacementValuesForTripartiteSpectrum.Count];


                for (int i = 0; i < pts.Length; i++)
                {
                    var tsd = rec.DisplacementValuesForTripartiteSpectrum[i];
                    var T = tsd.X;
                    var sd = tsd.Y;
                    var omega = Math.PI*2/T;
                    var sv = omega*sd;
                    pts[i] = new Point((1/T), (sv));
                }

                var convertedPoints = fn1(pts);

                var path = GetGeometry(convertedPoints);

                BindingOperations.SetBinding(path, Shape.StrokeThicknessProperty,
                    new Binding("ChartPenThickness") {Source = ApplicationSettings.Current});

                var brush = new SolidColorBrush();
                BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty,
                    new Binding("Color") {Source = rec});

                path.Stroke = brush;

                BindingOperations.SetBinding(path, UIElement.VisibilityProperty,
                    new Binding("IsVisible") {Source = rec, Converter = new BoolToVisibilityConverter()});


                SpectrumsCanvas.Children.Add(path);
                //path.RenderTransform = SpectrumsCanvas.RenderTransform;
            }
        }


        private Color MajorGridLinesColor = Colors.Black;
        private Color MinorGridLinesColor = Colors.Gray;

        private double MajorGridLinesThickness = 1;
        private double MinorGridLinesThickness = 0.5;


        public void RefreshUI()
        {
            ResetTrnasformParameters();
            SetTransformationParameters();
            ResetRenderTransforms(SpectrumsCanvas);

            GridsCanvas.Children.Clear();
            AddGridLines();


            SpectrumsCanvas.Children.RemoveChildsOfType<Path>();
            AddSpectrums();
        }

        private Path GetGeometry(Point[] arr, bool dashDot = false)
        {
            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;

            var geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;


            using (StreamGeometryContext ctx = geometry.Open())
            {
                var stroke = true;

                var pt = arr[0];
                pt.Y = NormalizeY(pt.Y);

                ctx.BeginFigure(pt, true, false);

                for (int i = 1; i < arr.Length; i++)
                {
                    pt = arr[i];
                    pt.Y = NormalizeY(pt.Y);
                    ctx.LineTo(pt, stroke, false);

                    if (dashDot)
                        stroke = !stroke;
                }
            }
            //geometry.Freeze();

            myPath.Data = geometry;

            return myPath;
        }

        private static double[] GetMajorLogTicks(double start, double end)
        {
            var n1 = (int) Math.Floor(Math.Log(start)) - 1;
            var n2 = (int) Math.Floor(Math.Log(end)) + 10;

            var arr =
                //new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
                new double[] {1, 2, 5};
            //new double[] { 1, Math.Pow(10, 0.25), Math.Pow(10, 0.5), Math.Pow(10, 0.75) };

            var fs = new List<double>();

            for (var i = n1; i <= n2; i++)
            {
                var vs = arr.Select(j => Math.Pow(10, i)*j);

                fs.AddRange(vs.Where(j => j >= start && j <= end));
            }

            return fs.ToArray();
        }

        private static double[] GetMinorLogTicks(double start, double end)
        {
            var n1 = (int) Math.Floor(Math.Log(start)) - 1;
            var n2 = (int) Math.Floor(Math.Log(end)) + 10;

            var arr =
                new double[] {3, 4, 6, 7, 8, 9};
            //new double[] { 1, 2, 5 };
            //new double[] { 1, Math.Pow(10, 0.25), Math.Pow(10, 0.5), Math.Pow(10, 0.75) };

            var fs = new List<double>();

            for (var i = n1; i <= n2; i++)
            {
                var vs = arr.Select(j => Math.Pow(10, i)*j);

                fs.AddRange(vs.Where(j => j >= start && j <= end));
            }

            return fs.ToArray();
        }

        #region grid line helpers

        private double NormalizeY(double y)
        {
            return h - y;
        }

        private Line AddDiagonalIncreasingLine(Point pt)
        {
            var h = GridsCanvas.ActualHeight;
            var w = GridsCanvas.ActualWidth;

            var x0 = pt.X;
            var y0 = pt.Y;

            var pts = new Point[]
            {
                new Point(0, -x0 + y0),
                new Point(x0 - y0, 0),
                new Point(w, w - x0 + y0),
                new Point(h + x0 - y0, h),
            };


            var e = 1e-6;

            var cnt = new Func<Point, bool>(pnt => pnt.X > -e && pnt.X < w + e && pnt.Y > -e && pnt.Y < h + e);

            var ptss = pts.Where(i => cnt(i)).ToList();

            if (ptss.Count < 2)
                return null;


            var p1 = ptss[0];

            var p2 = ptss[1];

            var ln = new Line() {Stroke = Brushes.Black};

            ln.X1 = p1.X;
            ln.X2 = p2.X;

            ln.Y1 = NormalizeY(p1.Y);
            ln.Y2 = NormalizeY(p2.Y);

            GridsCanvas.Children.Add(ln);

            return ln;
        }

        private Line AddDiagonalDecreasingLine(Point pt)
        {
            var h = GridsCanvas.ActualHeight;
            var w = GridsCanvas.ActualWidth;

            var x0 = pt.X;
            var y0 = pt.Y;


            var pts = new Point[]
            {
                new Point(0, x0 + y0),
                new Point(x0 + y0, 0),
                new Point(w, -w + x0 + y0),
                new Point(-h + x0 + y0, h),
            };

            var e = 1e-6;

            var cnt = new Func<Point, bool>(pnt => pnt.X > -e && pnt.X < w + e && pnt.Y > -e && pnt.Y < h + e);

            var ptss = pts.Where(i => cnt(i)).ToList();

            if (ptss.Count < 2)
                return null;


            var p1 = ptss[0];

            var p2 = ptss[1];

            var ln = new Line() {Stroke = Brushes.Black};

            ln.X1 = p1.X;
            ln.X2 = p2.X;

            ln.Y1 = NormalizeY(p1.Y);
            ln.Y2 = NormalizeY(p2.Y);

            GridsCanvas.Children.Add(ln);

            return ln;
        }

        private Line AddHorizontalLine(double y)
        {
            var ln = new Line() {Stroke = Brushes.Black};

            ln.X1 = 0;
            ln.X2 = w;

            ln.Y1 = NormalizeY(y);
            ln.Y2 = NormalizeY(y);

            GridsCanvas.Children.Add(ln);

            return ln;
        }


        private Line AddVerticalLine(double x)
        {
            var ln = new Line() {Stroke = Brushes.Black};

            ln.X1 = x;
            ln.X2 = x;

            ln.Y1 = 0;
            ln.Y2 = h;

            GridsCanvas.Children.Add(ln);

            return ln;
        }

        #endregion

        private void TripartiteSpectrum_OnMouseMove(object sender, MouseEventArgs e)
        {
            var loc = e.GetPosition(GridsCanvas);
        }

        private void ExportMenuItem_OnClick(object sender, RoutedEventArgs e)
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
    }
}