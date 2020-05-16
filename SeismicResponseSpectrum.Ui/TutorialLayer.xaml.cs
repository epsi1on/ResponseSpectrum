using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for TutorialLayer.xaml
    /// </summary>
    public partial class TutorialLayer : UserControl
    {
        public TutorialLayer()
        {
            InitializeComponent();
        }


        #region Steps Property and Property Change Routed event

        public static readonly DependencyProperty StepsProperty
            = DependencyProperty.Register(
                "Steps", typeof (FastObservableCollection<TutorialStep>), typeof (TutorialLayer),
                new PropertyMetadata(new FastObservableCollection<TutorialStep>(), OnStepsChanged, StepsCoerceValue));

        public FastObservableCollection<TutorialStep> Steps
        {
            get { return (FastObservableCollection<TutorialStep>) GetValue(StepsProperty); }
            set { SetValue(StepsProperty, value); }
        }

        public static readonly RoutedEvent StepsChangedEvent
            = EventManager.RegisterRoutedEvent(
                "StepsChanged",
                RoutingStrategy.Direct,
                typeof (RoutedPropertyChangedEventHandler<FastObservableCollection<TutorialStep>>),
                typeof (TutorialLayer));

        private static object StepsCoerceValue(DependencyObject d, object value)
        {
            var val = (FastObservableCollection<TutorialStep>) value;
            var obj = (TutorialLayer) d;


            return value;
        }

        public event RoutedPropertyChangedEventHandler<FastObservableCollection<TutorialStep>> StepsChanged
        {
            add { AddHandler(StepsChangedEvent, value); }
            remove { RemoveHandler(StepsChangedEvent, value); }
        }

        private static void OnStepsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TutorialLayer;
            var args = new RoutedPropertyChangedEventArgs<FastObservableCollection<TutorialStep>>(
                (FastObservableCollection<TutorialStep>) e.OldValue,
                (FastObservableCollection<TutorialStep>) e.NewValue);
            args.RoutedEvent = TutorialLayer.StepsChangedEvent;
            obj.RaiseEvent(args);
            
        }


        #endregion

        #region CurrentStep Property and Property Change Routed event

        public static readonly DependencyProperty CurrentStepProperty
            = DependencyProperty.Register(
                "CurrentStep", typeof (TutorialStep), typeof (TutorialLayer),
                new PropertyMetadata(null, OnCurrentStepChanged, CurrentStepCoerceValue));

        public TutorialStep CurrentStep
        {
            get { return (TutorialStep) GetValue(CurrentStepProperty); }
            set { SetValue(CurrentStepProperty, value); }
        }

        public static readonly RoutedEvent CurrentStepChangedEvent
            = EventManager.RegisterRoutedEvent(
                "CurrentStepChanged",
                RoutingStrategy.Direct,
                typeof (RoutedPropertyChangedEventHandler<TutorialStep>),
                typeof (TutorialLayer));

        private static object CurrentStepCoerceValue(DependencyObject d, object value)
        {
            var val = (TutorialStep) value;
            var obj = (TutorialLayer) d;


            return value;
        }

        public event RoutedPropertyChangedEventHandler<TutorialStep> CurrentStepChanged
        {
            add { AddHandler(CurrentStepChangedEvent, value); }
            remove { RemoveHandler(CurrentStepChangedEvent, value); }
        }

        private static void OnCurrentStepChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TutorialLayer;
            var args = new RoutedPropertyChangedEventArgs<TutorialStep>(
                (TutorialStep) e.OldValue,
                (TutorialStep) e.NewValue);
            args.RoutedEvent = TutorialLayer.CurrentStepChangedEvent;
            obj.RaiseEvent(args);
            obj.CurrentStepChange((TutorialStep) e.OldValue, (TutorialStep) e.NewValue);
        }


        #endregion

        #region AllStepsDone event

        public event EventHandler AllStepsDone
        {
            [MethodImpl(MethodImplOptions.Synchronized)] add { OnAllStepsDoneAdd(this, value); }
            [MethodImpl(MethodImplOptions.Synchronized)] remove { OnAllStepsDoneRemove(this, value); }
        }

        private EventHandler allStepsDone;

        public static void OnAllStepsDoneAdd(object sender, EventHandler handler)
        {
            var obj = sender as TutorialLayer;
            obj.allStepsDone = (EventHandler) Delegate.Combine(obj.allStepsDone, handler);
        }

        public static void OnAllStepsDoneRemove(object sender, EventHandler handler)
        {
            var obj = sender as TutorialLayer;
            obj.allStepsDone = (EventHandler) Delegate.Remove(obj.allStepsDone, handler);
        }

        #endregion

        #region PopupLocation Property and Property Change Routed event

        public static readonly DependencyProperty PopupLocationProperty
            = DependencyProperty.Register(
                "PopupLocation", typeof (Point), typeof (TutorialLayer),
                new PropertyMetadata(new Point(), OnPopupLocationChanged, PopupLocationCoerceValue));

        public Point PopupLocation
        {
            get { return (Point) GetValue(PopupLocationProperty); }
            set { SetValue(PopupLocationProperty, value); }
        }

        public static readonly RoutedEvent PopupLocationChangedEvent
            = EventManager.RegisterRoutedEvent(
                "PopupLocationChanged",
                RoutingStrategy.Direct,
                typeof (RoutedPropertyChangedEventHandler<Point>),
                typeof (TutorialLayer));

        private static object PopupLocationCoerceValue(DependencyObject d, object value)
        {
            var val = (Point) value;
            var obj = (TutorialLayer) d;


            return value;
        }

        public event RoutedPropertyChangedEventHandler<Point> PopupLocationChanged
        {
            add { AddHandler(PopupLocationChangedEvent, value); }
            remove { RemoveHandler(PopupLocationChangedEvent, value); }
        }

        private static void OnPopupLocationChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TutorialLayer;
            var args = new RoutedPropertyChangedEventArgs<Point>(
                (Point) e.OldValue,
                (Point) e.NewValue);
            args.RoutedEvent = TutorialLayer.PopupLocationChangedEvent;
            obj.RaiseEvent(args);
            
        }


        #endregion

        private void ResetPopupLocation()
        {
            mainPopup.DataContext = CurrentStep;
            
            mainPopup.UpdateTitleMemoBindings();
            mainPopup.UpdateLayout();

            var fn = new AnnotateRectanglePositionFinder();

            fn.AvailableArea = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            fn.TargetArea = CurrentStep.TargetArea;
            fn.AnnotationWidth = mainPopup.ActualWidth;
            fn.AnnotationHeight = mainPopup.ActualHeight;
            fn.DesiredDistance = Math.Min(Math.Min(fn.TargetArea.Width, fn.TargetArea.Height), 50);

            var rc = fn.GetBestPlacementForAnnotationBox();

            PopupLocation = new Point(rc.X, rc.Y);
        }

        private void CurrentStepChange(TutorialStep old, TutorialStep neww)
        {
            if (neww != null)
            {
                neww.MarkAsActivated();
                neww.StepDone += CurrentStepOnStepDone;
            }

            if (old != null)
            {
                old.StepDone -= CurrentStepOnStepDone;
            }

            if (neww != null)
                ResetPopupLocation();
        }

        private void CurrentStepOnStepDone(object sender, EventArgs eventArgs)
        {
            var idx = Steps.IndexOf(CurrentStep);

            if (idx < Steps.Count-1)
            {
                CurrentStep = Steps[idx+1];
            }
            else
            {
                this.Visibility = Visibility.Collapsed;

                if (this.allStepsDone != null)
                    this.allStepsDone(this, null);
            }
        }

        public void Start()
        {
            this.Visibility = Visibility.Visible;

            this.CurrentStep = this.Steps[0];
         
        }

    }
}
