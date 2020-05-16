using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

namespace SeismicResponseSpectrum.Ui
{
	/// <summary>
	/// Interaction logic for SeismicRecordControl.xaml
	/// </summary>
	public partial class SeismicRecordControl : UserControl
	{
		public SeismicRecordControl()
		{
			this.InitializeComponent();
		}


	    #region IsInEditMode Property and Property Change Routed event

	    public static readonly DependencyProperty IsInEditModeProperty
	        = DependencyProperty.Register(
	            "IsInEditMode", typeof (bool), typeof (SeismicRecordControl),
	            new PropertyMetadata(false, OnIsInEditModeChanged, IsInEditModeCoerceValue));

	    public bool IsInEditMode
	    {
	        get { return (bool) GetValue(IsInEditModeProperty); }
	        set { SetValue(IsInEditModeProperty, value); }
	    }

	    public static readonly RoutedEvent IsInEditModeChangedEvent
	        = EventManager.RegisterRoutedEvent(
	            "IsInEditModeChanged",
	            RoutingStrategy.Direct,
	            typeof (RoutedPropertyChangedEventHandler<bool>),
	            typeof (SeismicRecordControl));

	    private static object IsInEditModeCoerceValue(DependencyObject d, object value)
	    {
	        var val = (bool) value;
	        var obj = (SeismicRecordControl) d;


	        return value;
	    }

	    public event RoutedPropertyChangedEventHandler<bool> IsInEditModeChanged
	    {
	        add { AddHandler(IsInEditModeChangedEvent, value); }
	        remove { RemoveHandler(IsInEditModeChangedEvent, value); }
	    }

	    private static void OnIsInEditModeChanged(
	        DependencyObject d,
	        DependencyPropertyChangedEventArgs e)
	    {
	        var obj = d as SeismicRecordControl;
	        var args = new RoutedPropertyChangedEventArgs<bool>(
	            (bool) e.OldValue,
	            (bool) e.NewValue);
	        args.RoutedEvent = SeismicRecordControl.IsInEditModeChangedEvent;
	        obj.RaiseEvent(args);

	        if (true == (bool) e.NewValue)
	        {
                Focus(obj.txtTitle);
	            obj.txtTitle.Select(0,obj.txtTitle.Text.Length);
	        }
	    }


        public static void Focus(UIElement element)
        {

            if (!element.Focus())
            {

                element.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(delegate()
                {

                    element.Focus();

                }));

            }

        }


	    #endregion

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
                if(e.ClickCount==2)
                    if (!this.IsInEditMode)
                        this.IsInEditMode = true;
        }

        private void txtTitle_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.IsInEditMode = false;
                txtTitle.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Focus(chkVisible);
            }

            if (e.Key == Key.Escape)
            {
                this.IsInEditMode = false;
                txtTitle.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                Focus(chkVisible);
            }
        }

        private void txtTitle_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsInEditMode)
                this.IsInEditMode = false;
        }

	}
}