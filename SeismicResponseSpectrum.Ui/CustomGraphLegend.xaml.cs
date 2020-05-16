using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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
	/// Interaction logic for CustomGraphLegend.xaml
	/// </summary>
	public partial class CustomGraphLegend : UserControl
	{
		public CustomGraphLegend()
		{
			this.InitializeComponent();

		    this.DataContext = this.Context = new CustomGraphLegendDataContext();
		}

	    public CustomGraphLegendDataContext Context;

	    #region Items Property and Property Change Routed event

	    public static readonly DependencyProperty ItemsProperty
	        = DependencyProperty.Register(
	            "Items", typeof (FastObservableCollection<CustomGraphLegendItem>), typeof (CustomGraphLegend),
                new PropertyMetadata(GetDefault(), OnItemsChanged,
	                ItemsCoerceValue));

	    public FastObservableCollection<CustomGraphLegendItem> Items
	    {
	        get { return (FastObservableCollection<CustomGraphLegendItem>) GetValue(ItemsProperty); }
	        set { SetValue(ItemsProperty, value); }
	    }

	    public static readonly RoutedEvent ItemsChangedEvent
	        = EventManager.RegisterRoutedEvent(
	            "ItemsChanged",
	            RoutingStrategy.Direct,
	            typeof (RoutedPropertyChangedEventHandler<FastObservableCollection<CustomGraphLegendItem>>),
	            typeof (CustomGraphLegend));

	    private static object ItemsCoerceValue(DependencyObject d, object value)
	    {
	        var val = (FastObservableCollection<CustomGraphLegendItem>) value;
	        var obj = (CustomGraphLegend) d;


	        return value;
	    }

	    public event RoutedPropertyChangedEventHandler<FastObservableCollection<CustomGraphLegendItem>> ItemsChanged
	    {
	        add { AddHandler(ItemsChangedEvent, value); }
	        remove { RemoveHandler(ItemsChangedEvent, value); }
	    }

	    private static void OnItemsChanged(
	        DependencyObject d,
	        DependencyPropertyChangedEventArgs e)
	    {
	        var obj = d as CustomGraphLegend;
	        var args = new RoutedPropertyChangedEventArgs<FastObservableCollection<CustomGraphLegendItem>>(
	            (FastObservableCollection<CustomGraphLegendItem>) e.OldValue,
	            (FastObservableCollection<CustomGraphLegendItem>) e.NewValue);
	        args.RoutedEvent = CustomGraphLegend.ItemsChangedEvent;
	        obj.RaiseEvent(args);
	        
	    }


	    #endregion

	    #region ShowMaxAvgLines Property and Property Change Routed event

	    public static readonly DependencyProperty ShowMaxAvgLinesProperty
	        = DependencyProperty.Register(
	            "ShowMaxAvgLines", typeof (bool), typeof (CustomGraphLegend),
	            new PropertyMetadata(true, OnShowMaxAvgLinesChanged, ShowMaxAvgLinesCoerceValue));

	    public bool ShowMaxAvgLines
	    {
	        get { return (bool) GetValue(ShowMaxAvgLinesProperty); }
	        set { SetValue(ShowMaxAvgLinesProperty, value); }
	    }

	    public static readonly RoutedEvent ShowMaxAvgLinesChangedEvent
	        = EventManager.RegisterRoutedEvent(
	            "ShowMaxAvgLinesChanged",
	            RoutingStrategy.Direct,
	            typeof (RoutedPropertyChangedEventHandler<bool>),
	            typeof (CustomGraphLegend));

	    private static object ShowMaxAvgLinesCoerceValue(DependencyObject d, object value)
	    {
	        var val = (bool) value;
	        var obj = (CustomGraphLegend) d;


	        return value;
	    }

	    public event RoutedPropertyChangedEventHandler<bool> ShowMaxAvgLinesChanged
	    {
	        add { AddHandler(ShowMaxAvgLinesChangedEvent, value); }
	        remove { RemoveHandler(ShowMaxAvgLinesChangedEvent, value); }
	    }

	    private static void OnShowMaxAvgLinesChanged(
	        DependencyObject d,
	        DependencyPropertyChangedEventArgs e)
	    {
	        var obj = d as CustomGraphLegend;
	        var args = new RoutedPropertyChangedEventArgs<bool>(
	            (bool) e.OldValue,
	            (bool) e.NewValue);
	        args.RoutedEvent = CustomGraphLegend.ShowMaxAvgLinesChangedEvent;
	        obj.RaiseEvent(args);
	        
	    }


	    #endregion

	    private static FastObservableCollection<CustomGraphLegendItem> GetDefault()
	    {
	        var buf = new FastObservableCollection<CustomGraphLegendItem>();

	        buf.Add(new CustomGraphLegendItem() {TargetPen = new Pen(Brushes.Blue, 3), Title = "Example1"});

	        return buf;
	    }

	    public class CustomGraphLegendDataContext : INotifyPropertyChanged
	    {
	        #region INotifyPropertyChanged members and helpers

	        public event PropertyChangedEventHandler PropertyChanged;

	        private static bool AreEqualObjects(object obj1, object obj2)
	        {
	            var obj1Null = ReferenceEquals(obj1, null);
	            var obj2Null = ReferenceEquals(obj2, null);

	            if (obj1Null && obj2Null)
	                return true;

	            if (obj1Null || obj2Null)
	                return false;

	            if (obj1.GetType() != obj2.GetType())
	                return false;

	            if (ReferenceEquals(obj1, obj2))
	                return true;

	            return obj1.Equals(obj2);
	        }

	        private void OnPropertyChanged(params string[] propertyNames)
	        {
	            if (propertyNames == null)
	                return;

	            if (this.PropertyChanged != null)
	                foreach (var propertyName in propertyNames)
	                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	        }

	        #endregion

	        public CustomGraphLegendDataContext()
	        {
	            ChkChecked = true;
	        }

	        #region ChkChecked Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public bool ChkChecked
	        {
	            get { return chkChecked; }
	            set
	            {
	                if (AreEqualObjects(chkChecked, value))
	                    return;

	                var _fieldOldValue = chkChecked;

	                chkChecked = value;

	                CustomGraphLegendDataContext.OnChkCheckedChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

	                this.OnPropertyChanged("ChkChecked");
	            }
	        }

	        private bool chkChecked;

	        public EventHandler<PropertyValueChangedEventArgs<bool>> ChkCheckedChanged;

	        public static void OnChkCheckedChanged(object sender, PropertyValueChangedEventArgs<bool> e)
	        {
	            var obj = sender as CustomGraphLegendDataContext;

	            if (obj.ChkCheckedChanged != null)
	                obj.ChkCheckedChanged(obj, e);


	        }

	        #endregion
	    }
	    public class CustomGraphLegendItem : INotifyPropertyChanged
	    {
	        #region INotifyPropertyChanged members and helpers

	        public event PropertyChangedEventHandler PropertyChanged;

	        private static bool AreEqualObjects(object obj1, object obj2)
	        {
	            var obj1Null = ReferenceEquals(obj1, null);
	            var obj2Null = ReferenceEquals(obj2, null);

	            if (obj1Null && obj2Null)
	                return true;

	            if (obj1Null || obj2Null)
	                return false;

	            if (obj1.GetType() != obj2.GetType())
	                return false;

	            if (ReferenceEquals(obj1, obj2))
	                return true;

	            return obj1.Equals(obj2);
	        }

	        private void OnPropertyChanged(params string[] propertyNames)
	        {
	            if (propertyNames == null)
	                return;

	            if (this.PropertyChanged != null)
	                foreach (var propertyName in propertyNames)
	                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	        }

	        #endregion

	        #region TargetPen Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public Pen TargetPen
	        {
	            get { return targetPen; }
	            set
	            {
	                if (AreEqualObjects(targetPen, value))
	                    return;

	                var _fieldOldValue = targetPen;

	                targetPen = value;

	                CustomGraphLegendItem.OnTargetPenChanged(this, new PropertyValueChangedEventArgs<Pen>(_fieldOldValue, value));

	                this.OnPropertyChanged("TargetPen");
	            }
	        }

	        private Pen targetPen;

	        public EventHandler<PropertyValueChangedEventArgs<Pen>> TargetPenChanged;

	        public static void OnTargetPenChanged(object sender, PropertyValueChangedEventArgs<Pen> e)
	        {
	            var obj = sender as CustomGraphLegendItem;

	            if (obj.TargetPenChanged != null)
	                obj.TargetPenChanged(obj, e);


	        }

	        #endregion

	        #region TargetElement Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public UIElement TargetElement
	        {
	            get { return targetElement; }
	            set
	            {
	                if (AreEqualObjects(targetElement, value))
	                    return;

	                var _fieldOldValue = targetElement;

	                targetElement = value;

	                CustomGraphLegendItem.OnTargetElementChanged(this, new PropertyValueChangedEventArgs<UIElement>(_fieldOldValue, value));

	                this.OnPropertyChanged("TargetElement");
	            }
	        }

	        private UIElement targetElement;

	        public EventHandler<PropertyValueChangedEventArgs<UIElement>> TargetElementChanged;

	        public static void OnTargetElementChanged(object sender, PropertyValueChangedEventArgs<UIElement> e)
	        {
	            var obj = sender as CustomGraphLegendItem;

	            if (obj.TargetElementChanged != null)
	                obj.TargetElementChanged(obj, e);


	        }

	        #endregion

	        #region Title Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public string Title
	        {
	            get { return title; }
	            set
	            {
	                if (AreEqualObjects(title, value))
	                    return;

	                var _fieldOldValue = title;

	                title = value;

	                CustomGraphLegendItem.OnTitleChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

	                this.OnPropertyChanged("Title");
	            }
	        }

	        private string title;

	        public EventHandler<PropertyValueChangedEventArgs<string>> TitleChanged;

	        public static void OnTitleChanged(object sender, PropertyValueChangedEventArgs<string> e)
	        {
	            var obj = sender as CustomGraphLegendItem;

	            if (obj.TitleChanged != null)
	                obj.TitleChanged(obj, e);


	        }

	        #endregion


	        public static CustomGraphLegendItem Create(string title, params Pen[] pens)
	        {
	            throw new NotImplementedException();
	        }
	    }
	}
}