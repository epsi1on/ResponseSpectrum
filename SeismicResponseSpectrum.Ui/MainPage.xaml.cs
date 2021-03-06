﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using FirstFloor.ModernUI.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Shapes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Xceed.Wpf.AvalonDock.Converters;
using Xceed.Wpf.Toolkit;
using CustomDraggablePoint=Microsoft.Research.DynamicDataDisplay.Charts.Shapes.DraggablePoint;
using MessageBox = System.Windows.MessageBox;

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public static MainPage Current;

        public MainPage()
        {
            Current = this;
            //Records = new ObservableCollection<SeismicRecordSpectrum>();

            InitializeComponent();
            //InitializeCharts();
            //InitializeSteps();

            /** /
            AccelerationCharts.Viewport.AutoFitToView = false;
            VelocityCharts.Viewport.AutoFitToView = false;
            DisplacementCharts.Viewport.AutoFitToView = false;
            /**/

            this.Loaded += OnLoaded;

            //VelocityCharts.Viewport.VisibleChanged += VelocityChartOnVisibleChanged;


            LayersItemsControl.ItemsSource = ApplicationSettings.Current.ActiveRecords;

        }

        private bool VelocityChartViewChanged = false;

       


        private static bool AskedTutorial = false;

        private void AskForTut()
        {
            return;
            //if (Properties.Settings.Default.TutorialShownYet)
            //    return;

            if (AskedTutorial)
                return;

            AskedTutorial = true;

            var v = new ModernDialog
            {
                Title = "Using Application",
                Content = "Its first time you are running application, would you like to have a 30 sec tour of application?"
            };

            v.Buttons = new Button[] {v.YesButton, v.NoButton};
            v.YesButton.Click += (sender, e) => v.DialogResult = true;
 
            var res = v.ShowDialog();

            
            if (res.HasValue)
                if (res.Value)
                    throw new NotImplementedException();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            new Task(() =>
            {
                Thread.Sleep(500);
                this.DispatcherInvoke(() => AskForTut());
            }).Start();
        }


        private void InitializeSteps()
        {
            /*
            var s1 = new TutorialStep("Add Record", "Click on the button to add an example record to work set"){Tag = 0};
            s1.Activated += (sender, args) => s1.TargetArea = FindControlLocation(btnAddRecord);

            var s2 = new TutorialStep("Navigate Through Charts", "Use mouse left button to move the chart") { Tag = 1 };
            //s2.Activated += (sender, args) => s2.TargetArea = FindControlLocation(VelocityCharts);

            var s3 = new TutorialStep("Change Spectrum", "Try to move this points with your mouse in order to change the spectrum") { Tag = 2 };
            s3.Activated += (sender, args) => s3.TargetArea = FindExampleDraggablePointLocation();

            var s4 = new TutorialStep("Stretch Chart", "Click on this button to fit chart content into visible chart area") { Tag = 3 };
            s4.Activated += (sender, args) => s4.TargetArea = FindControlLocation(accelerationStretchBtn);

            var s5 = new TutorialStep("Change Record Color", "Click here to change the color of record on charts") { Tag = 4 };
            s5.Activated += (sender, args) => s5.TargetArea = FindExampleColorPickerLocation();

            var s6 = new TutorialStep("Show Hide Record", "Click on this to show/hide the record in charts") { Tag = 5 };
            s6.Activated += (sender, args) => s6.TargetArea = FindExampleShowHideLocation();

            var s7 = new TutorialStep("Rename Record", "Double click on record name to rename the record, type new name and then hit the enter") { Tag = 6 };
            s7.Activated += (sender, args) => s7.TargetArea = FindExampleRenameTextblockLocation();

            var s8 = new TutorialStep("Menus", "There are three tabs in upper left of window, you can use them to change settings and see extra information about the application\r\nClick on any place of window to dismiss this message") { Tag = 7 };
            s8.TargetArea = new Rect(this.ActualHeight-100, this.ActualHeight-100, 10, 10);


            this.Tutorials.Steps.AddItems(s1, s2, s3, s4, s5, s6, s7);

            this.Tutorials.AllStepsDone += (sender, args) =>
            {
                var v = new ModernDialog
                {
                    Title = "",
                    Content = "tour done"
                };
                v.Buttons = new Button[] { v.OkButton};
                v.ShowDialog();

                ApplicationSettings.Current.ActiveRecords.RemoveAt(0);
                
                AccelerationCharts.Legend.Visibility = Visibility.Collapsed;
                VelocityCharts.Legend.Visibility = Visibility.Collapsed;
                DisplacementCharts.Legend.Visibility = Visibility.Collapsed;

                Properties.Settings.Default.TutorialShownYet = true;
                Properties.Settings.Default.Save();
            };
            */

        }

        #region Finding control locations

        private Rect FindControlLocation(FrameworkElement elm)
        {

            var elmPos = elm.PointToScreen(new Point());
            var thisPos = this.PointToScreen(new Point());

            //return new Point(elmPos.X - thisPos.X , elmPos.Y - thisPos.Y-elm.ActualHeight);
            var buf = elm.TransformToAncestor(this).Transform(new Point(0, 0)); ;

            return new Rect(buf, new Vector(elm.ActualWidth, elm.ActualHeight));
        }

        #endregion



        #region DampRatio Property and Property Change Routed event

        public static readonly DependencyProperty DampRatioProperty
            = DependencyProperty.Register(
                "DampRatio", typeof(double), typeof(MainPage),
                new PropertyMetadata(0.05, OnDampRatioChanged, DampRatioCoerceValue));

        public double DampRatio
        {
            get { return (double)GetValue(DampRatioProperty); }
            set { SetValue(DampRatioProperty, value); }
        }

        public static readonly RoutedEvent DampRatioChangedEvent
            = EventManager.RegisterRoutedEvent(
                "DampRatioChanged",
                RoutingStrategy.Direct,
                typeof(RoutedPropertyChangedEventHandler<double>),
                typeof(MainPage));

        private static object DampRatioCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            var obj = (MainPage)d;


            return value;
        }

        public event RoutedPropertyChangedEventHandler<double> DampRatioChanged
        {
            add { AddHandler(DampRatioChangedEvent, value); }
            remove { RemoveHandler(DampRatioChangedEvent, value); }
        }

        private static void OnDampRatioChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MainPage;
            var args = new RoutedPropertyChangedEventArgs<double>(
                (double)e.OldValue,
                (double)e.NewValue);
            args.RoutedEvent = MainPage.DampRatioChangedEvent;
            obj.RaiseEvent(args);

        }


        #endregion


        



        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (Tutorials.Visibility == Visibility.Visible)
            {
                if (Tutorials.CurrentStep != null)
                    if (Equals(Tutorials.CurrentStep.Tag, 0))
                    {
                        var rec = new SeismicRecordSpectrum();

                        #region acceleration time history

                        var accel = new double[]
                        {
                            0, 0, 0, -0.001, -0.001, -0.001, 0, 0, 0, 0, 0, 0.001, 0.003, 0.004, 0.004, 0.003, 0.002,
                            0.002,
                            0.001, -0.001, -0.003, -0.004, -0.005, -0.004, -0.001, 0.001, 0, -0.005, -0.008, -0.008,
                            -0.007,
                            -0.003, 0.006, 0.015, 0.017, 0.013, 0.008, 0.005, 0.003, -0.001, -0.011, -0.02, -0.023,
                            -0.015,
                            0, 0.008, 0.007, 0.006, 0.01, 0.021, 0.032, 0.027, 0.005, -0.015, -0.029, -0.037, -0.03,
                            0.001,
                            0.043, 0.071, 0.055, 0.005, -0.049, -0.069, -0.055, -0.025, -0.006, -0.013, -0.032, -0.045,
                            -0.038, -0.023, -0.018, -0.023, -0.019, -0.002, 0.015, 0.037, 0.073, 0.113, 0.133, 0.128,
                            0.097,
                            0.045, -0.007, -0.052, -0.073, -0.058, -0.014, 0.044, 0.08, 0.056, 0.005, -0.037, -0.068,
                            -0.076,
                            -0.07, -0.068, -0.078, -0.092, -0.085, -0.049, 0.005, 0.047, 0.053, 0.045, 0.03, 0.006,
                            -0.021,
                            -0.047, -0.049, -0.004, 0.058, 0.085, 0.077, 0.067, 0.054, 0.038, 0.006, -0.046, -0.092,
                            -0.118,
                            -0.116, -0.108, -0.105, -0.095, -0.072, -0.043, -0.018, -0.008, 0.003, 0.026, 0.057, 0.098,
                            0.111, 0.093, 0.083, 0.084, 0.092, 0.097, 0.078, 0.04, 0.015, 0.014, 0, -0.038, -0.06,
                            -0.056,
                            -0.057, -0.07, -0.091, -0.106, -0.096, -0.062, -0.026, 0, 0.025, 0.071, 0.14, 0.201, 0.204,
                            0.144, 0.052, -0.027, -0.069, -0.081, -0.056, -0.012, 0.039, 0.084, 0.098, 0.056, -0.031,
                            -0.101,
                            -0.121, -0.114, -0.109, -0.107, -0.092, -0.051, 0.005, 0.052, 0.066, 0.057, 0.037, 0.03,
                            0.044,
                            0.045, 0.026, 0.008, -0.001, 0.005, 0.034, 0.069, 0.077, 0.047, 0.001, -0.053, -0.105,
                            -0.153,
                            -0.184, -0.185, -0.161, -0.108, -0.032, 0.036, 0.088, 0.131, 0.159, 0.173, 0.158, 0.116,
                            0.083,
                            0.078, 0.082, 0.077, 0.068, 0.036, -0.02, -0.077, -0.137, -0.195, -0.216, -0.194, -0.143,
                            -0.087,
                            -0.047, -0.017, 0, 0.002, -0.004, -0.029, -0.055, -0.054, -0.005, 0.092, 0.188, 0.235, 0.22,
                            0.168, 0.106, 0.049, -0.005, -0.05, -0.075, -0.091, -0.097, -0.094, -0.077, -0.049, -0.039,
                            -0.068, -0.094, -0.071, -0.026, 0.012, 0.042, 0.059, 0.056, 0.053, 0.067, 0.095, 0.105,
                            0.101,
                            0.107, 0.117, 0.099, 0.036, -0.027, -0.074, -0.101, -0.093, -0.057, -0.034, -0.038, -0.035,
                            -0.022, -0.001, 0.031, 0.052, 0.056, 0.036, -0.005, -0.045, -0.066, -0.068, -0.068, -0.074,
                            -0.085, -0.086, -0.076, -0.04, 0.015, 0.057, 0.081, 0.101, 0.122, 0.121, 0.093, 0.063, 0.046,
                            0.057, 0.082, 0.066, 0.013, -0.028, -0.043, -0.055, -0.091, -0.137, -0.149, -0.125, -0.108,
                            -0.098, -0.089, -0.093, -0.079, -0.012, 0.084, 0.151, 0.155, 0.112, 0.067, 0.047, 0.05,
                            0.068,
                            0.089, 0.085, 0.051, 0.013, -0.014, -0.029, -0.063, -0.127, -0.189, -0.199, -0.162, -0.096,
                            -0.024, 0.036, 0.087, 0.12, 0.116, 0.084, 0.054, 0.057, 0.077, 0.091, 0.075, 0.034, -0.01,
                            -0.023, 0.009, 0.057, 0.07, 0.032, -0.035, -0.087, -0.094, -0.046, 0.029, 0.062, 0.024,
                            -0.047,
                            -0.104, -0.139, -0.144, -0.12, -0.098, -0.082, -0.074, -0.072, -0.071, -0.067, -0.05, -0.008,
                            0.039, 0.071, 0.101, 0.144, 0.198, 0.237, 0.228, 0.164, 0.084, 0.03, 0.008, 0.022, 0.043,
                            0.022,
                            -0.069, -0.192, -0.25, -0.2, -0.112, -0.051, -0.028, -0.027, -0.022, 0.013, 0.076, 0.132,
                            0.171,
                            0.179, 0.153, 0.109, 0.055, 0.015, -0.016, -0.056, -0.104, -0.126, -0.127, -0.143, -0.158,
                            -0.125, -0.056, -0.009, -0.028, -0.073, -0.084, -0.037, 0.054, 0.134, 0.179, 0.203, 0.218,
                            0.216,
                            0.186, 0.144, 0.1, 0.065, 0.034, -0.017, -0.101, -0.198, -0.268, -0.283, -0.244, -0.19,
                            -0.135,
                            -0.097, -0.07, -0.024, 0.034, 0.086, 0.113, 0.124, 0.13, 0.114, 0.078, 0.04, 0.018, 0.033,
                            0.066,
                            0.069, 0.027, -0.049, -0.115, -0.13, -0.096, -0.047, -0.016, -0.004, 0.01, 0.046, 0.087,
                            0.093,
                            0.048, -0.013, -0.056, -0.054, -0.011, 0.035, 0.048, 0.042, 0.04, 0.036, 0.01, -0.038,
                            -0.076,
                            -0.079, -0.035, 0.036, 0.079, 0.079, 0.054, 0.019, -0.011, -0.018, -0.005, 0.02, 0.043, 0.04,
                            0.006, -0.044, -0.083, -0.098, -0.091, -0.069, -0.037, -0.009, 0.002, -0.002, -0.022, -0.035,
                            -0.028, -0.008, 0.005, 0.018, 0.028, 0.04, 0.047, 0.031, 0, -0.015, -0.003, 0.017, 0.031,
                            0.038,
                            0.026, 0.008, -0.014, -0.036, -0.036, -0.013, 0.014, 0.024, 0.009, -0.022, -0.054, -0.078,
                            -0.066, -0.008, 0.069, 0.12, 0.124, 0.084, 0.024, -0.027, -0.048, -0.038, -0.004, 0.034,
                            0.058,
                            0.068, 0.066, 0.048, 0.013, -0.03, -0.056, -0.046, -0.019, 0.005, 0.009, -0.026, -0.095,
                            -0.161,
                            -0.193, -0.181, -0.139, -0.082, -0.027, 0.009, 0.017, 0.012, -0.002, -0.006, 0.023, 0.067,
                            0.101,
                            0.124, 0.14, 0.14, 0.134, 0.13, 0.126, 0.108, 0.08, 0.059, 0.041, 0.032, 0.032, 0.022,
                            -0.006,
                            -0.039, -0.073, -0.097, -0.092, -0.058, -0.02, -0.007, -0.017, -0.03, -0.034, -0.034, -0.026,
                            -0.002, 0.028, 0.054, 0.071, 0.072, 0.064, 0.071, 0.088, 0.09, 0.072, 0.054, 0.044, 0.04,
                            0.047,
                            0.056, 0.052, 0.035, 0.013, -0.009, -0.03, -0.063, -0.095, -0.113, -0.115, -0.114, -0.114,
                            -0.135, -0.168, -0.186, -0.183, -0.155, -0.111, -0.078, -0.071, -0.071, -0.056, -0.027,
                            -0.001,
                            0.028, 0.056, 0.071, 0.067, 0.049, 0.036, 0.041, 0.059, 0.088, 0.111, 0.096, 0.055, 0.017,
                            -0.015, -0.054, -0.096, -0.111, -0.104, -0.096, -0.093, -0.088, -0.066, -0.036, -0.02,
                            -0.026,
                            -0.031, -0.028, -0.022, -0.007, 0.008, 0.01, -0.003, -0.027, -0.051, -0.051, -0.028, -0.005,
                            0.004, -0.008, -0.018, -0.016, -0.013, -0.015, -0.016, 0.001, 0.042, 0.092, 0.141, 0.187,
                            0.2,
                            0.178, 0.124, 0.055, 0.015, 0.016, 0.025, 0.026, 0.035, 0.063, 0.089, 0.098, 0.107, 0.124,
                            0.139,
                            0.157, 0.192, 0.224, 0.226, 0.206, 0.165, 0.092, -0.014, -0.141, -0.261, -0.356, -0.412,
                            -0.417,
                            -0.397, -0.397, -0.433, -0.499, -0.552, -0.55, -0.479, -0.354, -0.198, -0.039, 0.089, 0.171,
                            0.229, 0.262, 0.271, 0.285, 0.334, 0.404, 0.478, 0.538, 0.57, 0.55, 0.458, 0.31, 0.115, -0.1,
                            -0.266, -0.371, -0.431, -0.454, -0.452, -0.472, -0.537, -0.596, -0.594, -0.534, -0.446,
                            -0.337,
                            -0.201, -0.05, 0.071, 0.147, 0.207, 0.28, 0.403, 0.573, 0.742, 0.884, 0.99, 1.031, 0.984,
                            0.859,
                            0.696, 0.571, 0.526, 0.514, 0.465, 0.336, 0.119, -0.142, -0.414, -0.654, -0.817, -0.893,
                            -0.874,
                            -0.747, -0.561, -0.403, -0.322, -0.306, -0.316, -0.338, -0.347, -0.312, -0.226, -0.081,
                            0.079,
                            0.205, 0.254, 0.204, 0.066, -0.08, -0.149, -0.14, -0.08, 0.029, 0.144, 0.186, 0.188, 0.266,
                            0.436, 0.616, 0.72, 0.71, 0.576, 0.369, 0.196, 0.09, 0.042, 0.054, 0.123, 0.207, 0.235,
                            0.187,
                            0.089, -0.058, -0.248, -0.428, -0.546, -0.571, -0.496, -0.368, -0.237, -0.152, -0.114,
                            -0.107,
                            -0.198, -0.388, -0.551, -0.574, -0.421, -0.16, 0.098, 0.268, 0.354, 0.422, 0.469, 0.477,
                            0.464,
                            0.471, 0.54, 0.675, 0.797, 0.835, 0.79, 0.668, 0.446, 0.127, -0.274, -0.713, -1.098, -1.382,
                            -1.524, -1.556, -1.547, -1.479, -1.358, -1.295, -1.248, -1.035, -0.557, 0.157, 0.933, 1.623,
                            1.936, 1.769, 1.464, 1.299, 1.156, 0.896, 0.66, 0.56, 0.534, 0.492, 0.327, 0.051, -0.191,
                            -0.339,
                            -0.47, -0.54, -0.459, -0.351, -0.362, -0.411, -0.35, -0.237, -0.196, -0.144, -0.028, 0.044,
                            0.001, -0.149, -0.362, -0.55, -0.57, -0.386, -0.117, 0.124, 0.205, 0.011, -0.333, -0.598,
                            -0.668,
                            -0.532, -0.247, 0.114, 0.433, 0.535, 0.332, 0.06, 0.006, 0.194, 0.398, 0.442, 0.385, 0.326,
                            0.25,
                            0.127, -0.021, -0.144, -0.191, -0.119, 0.05, 0.229, 0.319, 0.271, 0.1, -0.102, -0.212,
                            -0.205,
                            -0.112, 0.022, 0.172, 0.287, 0.317, 0.272, 0.178, 0.01, -0.208, -0.363, -0.415, -0.365,
                            -0.242,
                            -0.152, -0.184, -0.308, -0.465, -0.608, -0.686, -0.63, -0.414, -0.136, 0.078, 0.185, 0.209,
                            0.204, 0.215, 0.245, 0.279, 0.311, 0.331, 0.314, 0.264, 0.212, 0.164, 0.084, -0.038, -0.15,
                            -0.179, -0.165, -0.185, -0.203, -0.176, -0.106, -0.013, 0.086, 0.158, 0.17, 0.121, 0.062,
                            0.016,
                            -0.02, -0.036, 0.005, 0.109, 0.224, 0.278, 0.22, 0.07, -0.092, -0.197, -0.235, -0.198,
                            -0.082,
                            0.086, 0.238, 0.317, 0.278, 0.098, -0.143, -0.306, -0.331, -0.225, -0.025, 0.196, 0.383,
                            0.464,
                            0.406, 0.287, 0.204, 0.202, 0.252, 0.311, 0.355, 0.358, 0.307, 0.22, 0.123, 0.028, -0.067,
                            -0.166, -0.283, -0.431, -0.581, -0.688, -0.712, -0.65, -0.556, -0.518, -0.512, -0.427,
                            -0.259,
                            -0.092, 0.018, 0.104, 0.156, 0.163, 0.165, 0.19, 0.234, 0.279, 0.31, 0.331, 0.344, 0.304,
                            0.174,
                            0.035, -0.018, 0.012, 0.053, 0.057, 0.001, -0.07, -0.042, 0.073, 0.156, 0.191, 0.232, 0.27,
                            0.294, 0.33, 0.369, 0.365, 0.295, 0.186, 0.078, -0.012, -0.085, -0.146, -0.171, -0.181,
                            -0.245,
                            -0.351, -0.393, -0.329, -0.249, -0.234, -0.284, -0.359, -0.384, -0.3, -0.131, 0.023, 0.094,
                            0.112, 0.126, 0.16, 0.183, 0.15, 0.07, -0.048, -0.196, -0.361, -0.473, -0.472, -0.399,
                            -0.315,
                            -0.189, 0.01, 0.223, 0.361, 0.372, 0.27, 0.133, 0.054, 0.017, -0.013, -0.017, 0.021, 0.085,
                            0.139, 0.165, 0.147, 0.076, -0.025, -0.106, -0.149, -0.189, -0.217, -0.164, -0.031, 0.111,
                            0.206,
                            0.245, 0.243, 0.237, 0.25, 0.27, 0.27, 0.251, 0.247, 0.27, 0.289, 0.262, 0.175, 0.032,
                            -0.129,
                            -0.252, -0.28, -0.211, -0.12, -0.07, -0.061, -0.078, -0.121, -0.175, -0.222, -0.251, -0.24,
                            -0.19, -0.137, -0.136, -0.165, -0.15, -0.078, 0.005, 0.061, 0.092, 0.099, 0.084, 0.074,
                            0.075,
                            0.057, 0.015, -0.026, -0.054, -0.062, -0.057, -0.045, -0.006, 0.048, 0.071, 0.039, -0.014,
                            -0.04,
                            -0.033, -0.008, 0.013, 0.037, 0.083, 0.132, 0.131, 0.066, -0.018, -0.079, -0.088, -0.012,
                            0.132,
                            0.271, 0.362, 0.394, 0.393, 0.381, 0.315, 0.165, 0.039, 0.042, 0.126, 0.196, 0.193, 0.112, 0,
                            -0.119, -0.231, -0.317, -0.371, -0.399, -0.398, -0.388, -0.381, -0.373, -0.368, -0.371,
                            -0.369,
                            -0.319, -0.222, -0.108, 0.004, 0.101, 0.156, 0.167, 0.167, 0.197, 0.256, 0.316, 0.356, 0.362,
                            0.325, 0.249, 0.142, 0.028, -0.081, -0.181, -0.246, -0.259, -0.253, -0.276, -0.319, -0.345,
                            -0.353, -0.363, -0.365, -0.328, -0.233, -0.106, 0.013, 0.091, 0.117, 0.13, 0.172, 0.258,
                            0.339,
                            0.377, 0.399, 0.425, 0.425, 0.386, 0.346, 0.32, 0.298, 0.292, 0.315, 0.32, 0.247, 0.134,
                            0.042,
                            -0.031, -0.086, -0.101, -0.08, -0.034, 0.022, 0.064, 0.076, 0.055, 0.028, 0.006, -0.024,
                            -0.07,
                            -0.113, -0.144, -0.175, -0.213, -0.234, -0.214, -0.186, -0.205, -0.264, -0.309, -0.32,
                            -0.324,
                            -0.334, -0.348, -0.363, -0.362, -0.317, -0.225, -0.103, 0.01, 0.084, 0.123, 0.152, 0.189,
                            0.239,
                            0.295, 0.349, 0.405, 0.459, 0.494, 0.478, 0.393, 0.27, 0.154, 0.068, 0.019, 0.019, 0.055,
                            0.061,
                            -0.001, -0.107, -0.226, -0.32, -0.364, -0.355, -0.312, -0.264, -0.235, -0.216, -0.208,
                            -0.216,
                            -0.219, -0.186, -0.117, -0.038, 0.032, 0.087, 0.117, 0.129, 0.134, 0.154, 0.195, 0.238,
                            0.265,
                            0.274, 0.265, 0.239, 0.202, 0.159, 0.115, 0.065, 0.023, -0.006, -0.047, -0.09, -0.12, -0.149,
                            -0.191, -0.236, -0.278, -0.309, -0.313, -0.289, -0.255, -0.211, -0.157, -0.099, -0.033,
                            0.035,
                            0.115, 0.216, 0.316, 0.391, 0.443, 0.479, 0.486, 0.451, 0.391, 0.328, 0.271, 0.221, 0.18,
                            0.14,
                            0.088, 0.036, 0, -0.033, -0.063, -0.073, -0.076, -0.097, -0.13, -0.163, -0.2, -0.227, -0.223,
                            -0.204, -0.187, -0.162, -0.135, -0.131, -0.147, -0.179, -0.207, -0.195, -0.143, -0.091,
                            -0.067,
                            -0.065, -0.085, -0.107, -0.104, -0.077, -0.036, 0.017, 0.065, 0.08, 0.063, 0.034, 0.003,
                            -0.031,
                            -0.053, -0.052, -0.027, 0.016, 0.069, 0.11, 0.122, 0.096, 0.053, 0.023, 0.029, 0.05, 0.07,
                            0.092,
                            0.106, 0.095, 0.065, 0.043, 0.03, -0.003, -0.049, -0.076, -0.094, -0.118, -0.146, -0.167,
                            -0.171,
                            -0.156, -0.132, -0.107, -0.081, -0.065, -0.055, -0.044, -0.035, -0.035, -0.039, -0.031,
                            -0.002,
                            0.032, 0.05, 0.048, 0.028, 0.002, -0.015, -0.02, -0.016, -0.003, -0.007, -0.033, -0.045,
                            -0.034,
                            -0.024, -0.032, -0.07, -0.129, -0.186, -0.217, -0.212, -0.18, -0.144, -0.118, -0.101, -0.08,
                            -0.031, 0.03, 0.087, 0.163, 0.225, 0.251, 0.275, 0.306, 0.322, 0.313, 0.291, 0.268, 0.229,
                            0.193,
                            0.179, 0.176, 0.159, 0.111, 0.05, -0.008, -0.069, -0.115, -0.131, -0.13, -0.14, -0.164,
                            -0.177,
                            -0.178, -0.164, -0.12, -0.055, 0.029, 0.11, 0.191, 0.279, 0.331, 0.326, 0.294, 0.263, 0.248,
                            0.252, 0.268, 0.284, 0.276, 0.242, 0.177, 0.089, -0.01, -0.085, -0.11, -0.109, -0.11, -0.133,
                            -0.182, -0.228, -0.256, -0.272, -0.275, -0.259, -0.239, -0.21, -0.18, -0.164, -0.171, -0.192,
                            -0.218, -0.238, -0.247, -0.241, -0.219, -0.187, -0.153, -0.123, -0.102, -0.085, -0.059,
                            -0.027,
                            -0.005, -0.003, -0.013, -0.017, -0.007, 0.022, 0.059, 0.088, 0.114, 0.13, 0.137, 0.138, 0.14,
                            0.133, 0.123, 0.12, 0.12, 0.125, 0.126, 0.116, 0.099, 0.08, 0.053, 0.016, -0.016, -0.031,
                            -0.036,
                            -0.03, -0.019, -0.008, -0.001, -0.004, -0.022, -0.051, -0.078, -0.082, -0.058, -0.012, 0.035,
                            0.056, 0.056, 0.039, 0.018, 0, -0.02, -0.044, -0.064, -0.074, -0.079, -0.087, -0.086, -0.078,
                            -0.073, -0.062, -0.044, -0.025, 0.003, 0.034, 0.054, 0.064, 0.079, 0.094, 0.101, 0.112, 0.12,
                            0.116, 0.105, 0.099, 0.095, 0.083, 0.059, 0.025, -0.02, -0.066, -0.087, -0.077, -0.068,
                            -0.076,
                            -0.08, -0.075, -0.071, -0.062, -0.04, -0.009, 0.013, -0.002, -0.04, -0.065, -0.055, -0.02,
                            0.025,
                            0.068, 0.117, 0.177, 0.22, 0.228, 0.216, 0.195, 0.176, 0.161, 0.13, 0.08, 0.03, -0.006,
                            -0.024,
                            -0.044, -0.071, -0.097, -0.117, -0.131, -0.149, -0.167, -0.175, -0.164, -0.135, -0.099,
                            -0.055,
                            -0.013, 0.013, 0.027, 0.038, 0.052, 0.071, 0.097, 0.12, 0.137, 0.145, 0.146, 0.139, 0.118,
                            0.089,
                            0.061, 0.029, -0.006, -0.042, -0.077, -0.105, -0.122, -0.135, -0.147, -0.15, -0.148, -0.136,
                            -0.115, -0.096, -0.073, -0.043, -0.008, 0.023, 0.065, 0.106, 0.122, 0.126, 0.126, 0.12,
                            0.101,
                            0.073, 0.038, -0.004, -0.041, -0.063, -0.079, -0.077, -0.042, -0.001, 0.022, 0.033, 0.038,
                            0.04,
                            0.046, 0.049, 0.046, 0.041, 0.032, 0.04, 0.063, 0.067, 0.049, 0.031, 0.011, -0.022, -0.059,
                            -0.088, -0.099, -0.091, -0.063, -0.035, -0.021, -0.008, -0.002, -0.015, -0.026, -0.024,
                            -0.028,
                            -0.038, -0.048, -0.058, -0.06, -0.058, -0.061, -0.065, -0.067, -0.068, -0.063, -0.058,
                            -0.057,
                            -0.054, -0.036, -0.011, 0, -0.013, -0.034, -0.054, -0.068, -0.073, -0.07, -0.061, -0.039,
                            0.001,
                            0.046, 0.081, 0.109, 0.126, 0.136, 0.15, 0.162, 0.168, 0.165, 0.158, 0.148, 0.134, 0.118,
                            0.103,
                            0.089, 0.069, 0.044, 0.009, -0.03, -0.065, -0.09, -0.108, -0.123, -0.137, -0.149, -0.149,
                            -0.137,
                            -0.114, -0.084, -0.053, -0.029, -0.016, -0.007, -0.001, 0.003, 0.008, 0.016, 0.019, 0.018,
                            0.019,
                            0.027, 0.032, 0.038, 0.042, 0.038, 0.025, 0.012, 0.009, 0.009, 0, -0.007, -0.014, -0.018,
                            -0.022,
                            -0.033, -0.049, -0.068, -0.084, -0.089, -0.085, -0.077, -0.061, -0.047, -0.032, -0.013,
                            0.007,
                            0.029, 0.047, 0.057, 0.052, 0.036, 0.027, 0.027, 0.027, 0.023, 0.016, 0.02, 0.024, 0.012,
                            -0.003,
                            -0.01, -0.023, -0.046, -0.069, -0.087, -0.1, -0.102, -0.105, -0.106, -0.1, -0.088, -0.073,
                            -0.052, -0.026, -0.001, 0.012, 0.013, 0.009, 0.013, 0.031, 0.045, 0.043, 0.035, 0.034, 0.035,
                            0.034, 0.039, 0.052, 0.065, 0.082, 0.098, 0.107, 0.106, 0.102, 0.1, 0.097, 0.094, 0.092,
                            0.09,
                            0.088, 0.079, 0.063, 0.053, 0.05, 0.035, 0.01, -0.013, -0.027, -0.036, -0.046, -0.053,
                            -0.051,
                            -0.048, -0.046, -0.039, -0.029, -0.021, -0.012, 0, 0.012, 0.017, 0.023, 0.028, 0.026, 0.027,
                            0.035, 0.039, 0.039, 0.044, 0.048, 0.045, 0.042, 0.035, 0.026, 0.024, 0.024, 0.022, 0.021,
                            0.018,
                            0.006, -0.012, -0.032, -0.04, -0.051, -0.061, -0.06, -0.048, -0.023, 0.009, 0.037, 0.06,
                            0.071,
                            0.067, 0.06, 0.055, 0.056, 0.048, 0.032, 0.022, 0.02, 0.021, 0.026, 0.033, 0.037, 0.031,
                            0.012,
                            -0.015, -0.043, -0.07, -0.094, -0.116, -0.132, -0.137, -0.139, -0.141, -0.138, -0.131,
                            -0.125,
                            -0.117, -0.098, -0.082, -0.072, -0.061, -0.053, -0.05, -0.045, -0.03, -0.016, -0.004, 0.005,
                            0.012, 0.023, 0.029, 0.037, 0.044, 0.049, 0.06, 0.068, 0.072, 0.069, 0.058, 0.042, 0.026,
                            0.012,
                            -0.005, -0.024, -0.037, -0.046, -0.055, -0.062, -0.06, -0.055, -0.051, -0.045, -0.028,
                            -0.008,
                            0.006, 0.011, 0.011, 0.012, 0.021, 0.027, 0.026, 0.024, 0.027, 0.029, 0.024, 0.02, 0.02,
                            0.02,
                            0.019, 0.018, 0.014, 0.009, 0.007, 0.012, 0.016, 0.016, 0.014, 0.01, 0.003, 0, -0.004,
                            -0.007,
                            -0.003, 0, 0.001, 0.002, 0.005, 0.006, 0.006, 0.007, 0.008, 0.01, 0.014, 0.015, 0.011, 0.007,
                            0.004, 0.001, -0.002, -0.004, -0.005, -0.006, -0.007, -0.007, -0.004, -0.001, 0.003, 0.005,
                            0.008, 0.011, 0.014, 0.018, 0.022, 0.024, 0.026, 0.027, 0.025, 0.022, 0.019, 0.016, 0.014,
                            0.011,
                            0.007, 0.005, 0.002, -0.002, -0.004, -0.006, -0.008, -0.009, -0.009, -0.009, -0.009, -0.008,
                            -0.008, -0.007, -0.006, -0.005, -0.004, -0.003, -0.002, -0.001, -0.001, 0, 0, 0, 0, 0, 0, 0,
                            0,
                            0, 0, -0.001, -0.001, -0.001, -0.001, -0.001, -0.001, -0.001, -0.001, -0.001, -0.002, -0.002,
                            -0.002, -0.002, -0.002, -0.002, -0.002, -0.002, -0.002, -0.002, -0.002, -0.002, -0.002,
                            -0.002,
                            -0.002
                        };

                        #endregion

                        rec.Title = "Trinidad";
                        rec.GroundAcceleration = accel;
                        rec.DeltaT = 0.01;
                        rec.Color = Colors.Red;
                        rec.ComputeResponseSpectrums();
                        //rec.ColorChanged += TutorialRecordColorChanged;
                        //rec.TitleChanged+=TutorialRecordTitleChanged;
                        //rec.IsVisibleChanged+=TutorialRecordIsVisibleChanged;
                        ApplicationSettings.Current.ActiveRecords.Add(rec);

                        new Task(() =>
                        {
                            Thread.Sleep(50);
                            this.DispatcherInvoke(() => Tutorials.CurrentStep.MarkAsDone());
                        }).Start();



                    }

                return;
            }
            */

            var dlg = new ImportAccelerogramDialog();

            var val = dlg.ShowDialog();

            if (!val.HasValue)
                return;

            if (val.Value)
            {
                var rec = dlg.Record;
                rec.ComputeResponseSpectrums();
                ApplicationSettings.Current.ActiveRecords.Add(rec);
            }
        }




        public class MinSizeConverter:IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

     
    }
}
