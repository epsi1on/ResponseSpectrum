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
using System.Windows.Shapes;

namespace SeismicResponseSpectrum.Ui
{
	/// <summary>
	/// Interaction logic for ExportDiagramWindow.xaml
	/// </summary>
	public partial class ExportDiagramWindow : Window
	{
		public ExportDiagramWindow()
		{
			this.InitializeComponent();

		    this.DataContext = this.Context = new ExportDiagramDataContext();
		}

	    public ExportDiagramDataContext Context = new ExportDiagramDataContext();


	    public class ExportDiagramDataContext : INotifyPropertyChanged
	    {
	        public ExportDiagramDataContext()
	        {
	            ExportPng = true;
	            PngDpi = 200;
	        }

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

	        #region Preview Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public ImageSource Preview
	        {
	            get { return preview; }
	            set
	            {
	                if (AreEqualObjects(preview, value))
	                    return;

	                var _fieldOldValue = preview;

	                preview = value;

	                ExportDiagramDataContext.OnPreviewChanged(this, new PropertyValueChangedEventArgs<ImageSource>(_fieldOldValue, value));

	                this.OnPropertyChanged("Preview");
	            }
	        }

	        private ImageSource preview;

	        public EventHandler<PropertyValueChangedEventArgs<ImageSource>> PreviewChanged;

	        public static void OnPreviewChanged(object sender, PropertyValueChangedEventArgs<ImageSource> e)
	        {
	            var obj = sender as ExportDiagramDataContext;

	            if (obj.PreviewChanged != null)
	                obj.PreviewChanged(obj, e);


	        }

	        #endregion

	        #region ExportPng Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public bool ExportPng
	        {
	            get { return exportPng; }
	            set
	            {
	                if (AreEqualObjects(exportPng, value))
	                    return;

	                var _fieldOldValue = exportPng;

	                exportPng = value;

	                ExportDiagramDataContext.OnExportPngChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

	                this.OnPropertyChanged("ExportPng");
	            }
	        }

	        private bool exportPng;

	        public EventHandler<PropertyValueChangedEventArgs<bool>> ExportPngChanged;

	        public static void OnExportPngChanged(object sender, PropertyValueChangedEventArgs<bool> e)
	        {
	            var obj = sender as ExportDiagramDataContext;

	            if (obj.ExportPngChanged != null)
	                obj.ExportPngChanged(obj, e);


	        }

	        #endregion

	        #region ExportXaml Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public bool ExportXaml
	        {
	            get { return exportXaml; }
	            set
	            {
	                if (AreEqualObjects(exportXaml, value))
	                    return;

	                var _fieldOldValue = exportXaml;

	                exportXaml = value;

	                ExportDiagramDataContext.OnExportXamlChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

	                this.OnPropertyChanged("ExportXaml");
	            }
	        }

	        private bool exportXaml;

	        public EventHandler<PropertyValueChangedEventArgs<bool>> ExportXamlChanged;

	        public static void OnExportXamlChanged(object sender, PropertyValueChangedEventArgs<bool> e)
	        {
	            var obj = sender as ExportDiagramDataContext;

	            if (obj.ExportXamlChanged != null)
	                obj.ExportXamlChanged(obj, e);


	        }

	        #endregion

	        #region PngDpi Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public int PngDpi
	        {
	            get { return pngDpi; }
	            set
	            {
	                if (AreEqualObjects(pngDpi, value))
	                    return;

	                var _fieldOldValue = pngDpi;

	                pngDpi = value;

	                ExportDiagramDataContext.OnPngDpiChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

	                this.OnPropertyChanged("PngDpi");
	            }
	        }

	        private int pngDpi;

	        public EventHandler<PropertyValueChangedEventArgs<int>> PngDpiChanged;

	        public static void OnPngDpiChanged(object sender, PropertyValueChangedEventArgs<int> e)
	        {
	            var obj = sender as ExportDiagramDataContext;

	            if (obj.PngDpiChanged != null)
	                obj.PngDpiChanged(obj, e);

	            obj.SetPngDpiQuality();
	        }

	        #endregion

	        private void SetPngDpiQuality()
	        {
	            DpiQuality = pngDpi.ToString();
	        }

	        #region DpiQuality Property and field

	        [Obfuscation(Exclude = true, ApplyToMembers = false)]
	        public string DpiQuality
	        {
	            get { return dpiQuality; }
	            set
	            {
	                if (AreEqualObjects(dpiQuality, value))
	                    return;

	                var _fieldOldValue = dpiQuality;

	                dpiQuality = value;

	                ExportDiagramDataContext.OnDpiQualityChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

	                this.OnPropertyChanged("DpiQuality");
	            }
	        }

	        private string dpiQuality;

	        public EventHandler<PropertyValueChangedEventArgs<string>> DpiQualityChanged;

	        public static void OnDpiQualityChanged(object sender, PropertyValueChangedEventArgs<string> e)
	        {
	            var obj = sender as ExportDiagramDataContext;

	            if (obj.DpiQualityChanged != null)
	                obj.DpiQualityChanged(obj, e);


	        }

	        #endregion
	    }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
	}
}