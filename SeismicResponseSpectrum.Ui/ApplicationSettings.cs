using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Represents an application wide settings class
    /// </summary>
    public class ApplicationSettings : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ApplicationSettings"/> class from being created.
        /// </summary>
        private ApplicationSettings()
        {
            ActiveRecords = new FastObservableCollection<SeismicRecordSpectrum>();

        }

        #endregion


        /// <summary>
        /// Loads the last saved settings.
        /// </summary>
        /// <returns></returns>
        private static ApplicationSettings LoadLastSettings()
        {
            var buf = new ApplicationSettings();
            //TODO: fill buf from saved data
            return buf;
        }

        public static ApplicationSettings Current = LoadLastSettings();


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



        #region SpectrumsDt Property and field

        /// <summary>
        /// Gets or sets the Δt associated with calculations of spectrums.
        /// </summary>
        /// <value>
        /// The Δt which will be used for calculating each point of a spectrum.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double SpectrumsDt
        {
            get { return spectrumsDt; }
            set
            {
                if (AreEqualObjects(spectrumsDt, value))
                    return;

                var _fieldOldValue = spectrumsDt;

                spectrumsDt = value;

                ApplicationSettings.OnSpectrumsDtChanged(this,
                    new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("SpectrumsDt");
            }
        }

        private double spectrumsDt = 0.01;

        public EventHandler<PropertyValueChangedEventArgs<double>> SpectrumsDtChanged;

        public static void OnSpectrumsDtChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.SpectrumsDtChanged != null)
                obj.SpectrumsDtChanged(obj, e);

            obj.ReloadAllUiCharts();
        }

        #endregion

        #region SpectrumsTMax Property and field

        /// <summary>
        /// Gets or sets the maximum T which will be used in calculating spectrums.
        /// </summary>
        /// <value>
        /// The Maximum T which will be used for creating a spectrum.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double SpectrumsTMax
        {
            get { return spectrumsTMax; }
            set
            {
                if (AreEqualObjects(spectrumsTMax, value))
                    return;

                var _fieldOldValue = spectrumsTMax;

                spectrumsTMax = value;

                ApplicationSettings.OnSpectrumsTMaxChanged(this,
                    new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("SpectrumsTMax");
            }
        }

        private double spectrumsTMax = 5.0;

        public EventHandler<PropertyValueChangedEventArgs<double>> SpectrumsTMaxChanged;

        public static void OnSpectrumsTMaxChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.SpectrumsTMaxChanged != null)
                obj.SpectrumsTMaxChanged(obj, e);

            obj.ReloadAllUiCharts();
        }

        #endregion

        #region ChartPenThickness Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double ChartPenThickness
        {
            get { return chartPenThickness; }
            set
            {
                if (AreEqualObjects(chartPenThickness, value))
                    return;

                var _fieldOldValue = chartPenThickness;

                chartPenThickness = value;

                ApplicationSettings.OnChartPenThicknessChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("ChartPenThickness");
            }
        }

        private double chartPenThickness=3;

        public EventHandler<PropertyValueChangedEventArgs<double>> ChartPenThicknessChanged;

        public static void OnChartPenThicknessChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.ChartPenThicknessChanged != null)
                obj.ChartPenThicknessChanged(obj, e);


        }

        #endregion

        #region DampRatio Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double DampRatio
        {
            get { return dampRatio; }
            set
            {
                if (AreEqualObjects(dampRatio, value))
                    return;

                var _fieldOldValue = dampRatio;

                dampRatio = value;

                ApplicationSettings.OnDampRatioChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("DampRatio");
            }
        }

        private double dampRatio=0.05;

        public EventHandler<PropertyValueChangedEventArgs<double>> DampRatioChanged;

        public static void OnDampRatioChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.DampRatioChanged != null)
                obj.DampRatioChanged(obj, e);

            obj.ReloadAllUiCharts();
        }

        #endregion

        #region TripartiteMinFrequency Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double TripartiteMinFrequency
        {
            get { return tripartiteMinFrequency; }
            set
            {
                if (AreEqualObjects(tripartiteMinFrequency, value))
                    return;

                var _fieldOldValue = tripartiteMinFrequency;

                tripartiteMinFrequency = value;

                ApplicationSettings.OnTripartiteMinFrequencyChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("TripartiteMinFrequency");
            }
        }

        private double tripartiteMinFrequency = 0.05;

        public EventHandler<PropertyValueChangedEventArgs<double>> TripartiteMinFrequencyChanged;

        public static void OnTripartiteMinFrequencyChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.TripartiteMinFrequencyChanged != null)
                obj.TripartiteMinFrequencyChanged(obj, e);


        }

        #endregion

        #region TripartiteMaxFrequency Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double TripartiteMaxFrequency
        {
            get { return tripartiteMaxFrequency; }
            set
            {
                if (AreEqualObjects(tripartiteMaxFrequency, value))
                    return;

                var _fieldOldValue = tripartiteMaxFrequency;

                tripartiteMaxFrequency = value;

                ApplicationSettings.OnTripartiteMaxFrequencyChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("TripartiteMaxFrequency");
            }
        }

        private double tripartiteMaxFrequency = 20;

        public EventHandler<PropertyValueChangedEventArgs<double>> TripartiteMaxFrequencyChanged;

        public static void OnTripartiteMaxFrequencyChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.TripartiteMaxFrequencyChanged != null)
                obj.TripartiteMaxFrequencyChanged(obj, e);


        }

        #endregion

        #region TripartiteMinSv Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double TripartiteMinSv
        {
            get { return tripartiteMinSv; }
            set
            {
                if (AreEqualObjects(tripartiteMinSv, value))
                    return;

                var _fieldOldValue = tripartiteMinSv;

                tripartiteMinSv = value;

                ApplicationSettings.OnTripartiteMinSvChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("TripartiteMinSv");
            }
        }

        private double tripartiteMinSv = 0.005;

        public EventHandler<PropertyValueChangedEventArgs<double>> TripartiteMinSvChanged;

        public static void OnTripartiteMinSvChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.TripartiteMinSvChanged != null)
                obj.TripartiteMinSvChanged(obj, e);


        }

        #endregion

        #region ActiveRecords Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<SeismicRecordSpectrum> ActiveRecords
        {
            get { return activeRecords; }
            set
            {
                if (AreEqualObjects(activeRecords, value))
                    return;

                var _fieldOldValue = activeRecords;

                activeRecords = value;

                ApplicationSettings.OnActiveRecordsChanged(this, new PropertyValueChangedEventArgs<FastObservableCollection<SeismicRecordSpectrum>>(_fieldOldValue, value));

                this.OnPropertyChanged("ActiveRecords");
            }
        }

        private FastObservableCollection<SeismicRecordSpectrum> activeRecords;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<SeismicRecordSpectrum>>> ActiveRecordsChanged;

        public static void OnActiveRecordsChanged(object sender, PropertyValueChangedEventArgs<FastObservableCollection<SeismicRecordSpectrum>> e)
        {
            var obj = sender as ApplicationSettings;

            if (obj.ActiveRecordsChanged != null)
                obj.ActiveRecordsChanged(obj, e);

            if (e.NewValue != null)
                e.NewValue.CollectionChanged += obj.recordsOnCollectionChanged;

            if (e.OldValue != null)
                e.OldValue.CollectionChanged -= obj.recordsOnCollectionChanged;

        }

        private void recordsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ThreeCharts.Current.UpdateAverageAccelResponseLine();
        }

        #endregion



        private void ReloadAllUiCharts()
        {
            var tks = new Task(() =>
            {
                MainWindow.Current.ShowWaitSign();

                foreach (var rec in ActiveRecords)
                    rec.ComputeResponseSpectrums();

                if (ThreeCharts.Current != null)
                {
                    Application.Current.DispatcherInvoke(() => ThreeCharts.Current.RefreshUI());
                }

                if (TripartiteSpectrum.Current != null)
                {
                    Application.Current.DispatcherInvoke(() => TripartiteSpectrum.Current.RefreshUI());
                }


                
                MainWindow.Current.HideWaitSign();

                //MessageBox.Show("ssss");
            });

            tks.Start();
        }


        public static ObjectPool<double[]> CurrentDoubleArrayPool;

    }
}
