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
    /// Interaction logic for CustomPopup.xaml
    /// </summary>
    public partial class CustomPopup : UserControl
    {
        public CustomPopup()
        {
            InitializeComponent();
        }

        #region Title Property and Property Change Routed event

        public static readonly DependencyProperty TitleProperty
            = DependencyProperty.Register(
                "Title", typeof (string), typeof (CustomPopup),
                new PropertyMetadata(null, OnTitleChanged, TitleCoerceValue));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly RoutedEvent TitleChangedEvent
            = EventManager.RegisterRoutedEvent(
                "TitleChanged",
                RoutingStrategy.Direct,
                typeof (RoutedPropertyChangedEventHandler<string>),
                typeof (CustomPopup));

        private static object TitleCoerceValue(DependencyObject d, object value)
        {
            var val = (string) value;
            var obj = (CustomPopup) d;


            return value;
        }

        public event RoutedPropertyChangedEventHandler<string> TitleChanged
        {
            add { AddHandler(TitleChangedEvent, value); }
            remove { RemoveHandler(TitleChangedEvent, value); }
        }

        private static void OnTitleChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = d as CustomPopup;
            var args = new RoutedPropertyChangedEventArgs<string>(
                (string) e.OldValue,
                (string) e.NewValue);
            args.RoutedEvent = CustomPopup.TitleChangedEvent;
            obj.RaiseEvent(args);
            
        }


        #endregion

        #region Memo Property and Property Change Routed event

        public static readonly DependencyProperty MemoProperty
            = DependencyProperty.Register(
                "Memo", typeof (string), typeof (CustomPopup),
                new PropertyMetadata(null, OnMemoChanged, MemoCoerceValue));

        public string Memo
        {
            get { return (string) GetValue(MemoProperty); }
            set { SetValue(MemoProperty, value); }
        }

        public static readonly RoutedEvent MemoChangedEvent
            = EventManager.RegisterRoutedEvent(
                "MemoChanged",
                RoutingStrategy.Direct,
                typeof (RoutedPropertyChangedEventHandler<string>),
                typeof (CustomPopup));

        private static object MemoCoerceValue(DependencyObject d, object value)
        {
            var val = (string) value;
            var obj = (CustomPopup) d;


            return value;
        }

        public event RoutedPropertyChangedEventHandler<string> MemoChanged
        {
            add { AddHandler(MemoChangedEvent, value); }
            remove { RemoveHandler(MemoChangedEvent, value); }
        }

        private static void OnMemoChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var obj = d as CustomPopup;
            var args = new RoutedPropertyChangedEventArgs<string>(
                (string) e.OldValue,
                (string) e.NewValue);
            args.RoutedEvent = CustomPopup.MemoChangedEvent;
            obj.RaiseEvent(args);
            
        }


        #endregion

        public void UpdateTitleMemoBindings()
        {
            this.GetBindingExpression(CustomPopup.TitleProperty).UpdateSource();
            this.GetBindingExpression(CustomPopup.MemoProperty).UpdateSource();

            txtMemo.GetBindingExpression(TextBlock.TextProperty).UpdateSource();
            txtTitle.GetBindingExpression(TextBlock.TextProperty).UpdateSource();
        }
    }
}
