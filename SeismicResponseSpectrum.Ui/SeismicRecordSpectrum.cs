using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;

namespace SeismicResponseSpectrum.Ui
{
    public class SeismicRecordSpectrum : INotifyPropertyChanged
    {
        public SeismicRecordSpectrum()
        {
            AccelerationSpectrum = new FastObservableCollection<Point2D>();
            SpeedSpectrum = new FastObservableCollection<Point2D>();
            DisplacementSpectrum = new FastObservableCollection<Point2D>();


            DisplacementValuesForTripartiteSpectrum = new FastObservableCollection<Point2D>();

            DisplacementValues = new FastObservableCollection<Point2D>();
            VelocityValues = new FastObservableCollection<Point2D>();
            AccelerationValues = new FastObservableCollection<Point2D>();

            GroundAccelerationHistory = new List<Point2D>();
            GroundDisplacementHistory = new List<Point2D>();
            GroundVelocityHistory = new List<Point2D>();
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

        #region Color Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public Color Color
        {
            get { return color; }
            set
            {
                if (AreEqualObjects(color, value))
                    return;

                var _fieldOldValue = color;

                color = value;

                SeismicRecordSpectrum.OnColorChanged(this,
                    new PropertyValueChangedEventArgs<Color>(_fieldOldValue, value));

                this.OnPropertyChanged("Color");
            }
        }

        private Color color;

        public EventHandler<PropertyValueChangedEventArgs<Color>> ColorChanged;

        public static void OnColorChanged(object sender, PropertyValueChangedEventArgs<Color> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.ColorChanged != null)
                obj.ColorChanged(obj, e);
        }

        #endregion

        public double[] GroundAcceleration;

        #region DeltaT Property and field

        /// <summary>
        /// Gets or sets the delta t.
        /// </summary>
        /// <value>
        /// The delta t associated with time difference of two Successive members of <see cref="GroundAcceleration"/>.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double DeltaT
        {
            get { return deltaT; }
            set
            {
                if (AreEqualObjects(deltaT, value))
                    return;

                var _fieldOldValue = deltaT;

                deltaT = value;

                SeismicRecordSpectrum.OnDeltaTChanged(this,
                    new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("DeltaT");
            }
        }

        private double deltaT;

        public EventHandler<PropertyValueChangedEventArgs<double>> DeltaTChanged;

        public static void OnDeltaTChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.DeltaTChanged != null)
                obj.DeltaTChanged(obj, e);
        }

        #endregion

        #region TMax Property and field

        /// <summary>
        /// Gets or sets the t maximum associated with last member of <see cref="GroundAcceleration"/>.
        /// </summary>
        /// <value>
        /// The t maximum.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double TMax
        {
            get { return tMax; }
            set
            {
                if (AreEqualObjects(tMax, value))
                    return;

                var _fieldOldValue = tMax;

                tMax = value;

                SeismicRecordSpectrum.OnTMaxChanged(this,
                    new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("TMax");
            }
        }

        private double tMax;

        public EventHandler<PropertyValueChangedEventArgs<double>> TMaxChanged;

        public static void OnTMaxChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.TMaxChanged != null)
                obj.TMaxChanged(obj, e);
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

                SeismicRecordSpectrum.OnTitleChanged(this,
                    new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("Title");
            }
        }

        private string title;

        public EventHandler<PropertyValueChangedEventArgs<string>> TitleChanged;

        public static void OnTitleChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.TitleChanged != null)
                obj.TitleChanged(obj, e);
        }

        #endregion

        #region IsVisible Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (AreEqualObjects(isVisible, value))
                    return;

                var _fieldOldValue = isVisible;

                isVisible = value;

                SeismicRecordSpectrum.OnIsVisibleChanged(this,
                    new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                this.OnPropertyChanged("IsVisible");
            }
        }

        private bool isVisible = true;

        public EventHandler<PropertyValueChangedEventArgs<bool>> IsVisibleChanged;

        public static void OnIsVisibleChanged(object sender, PropertyValueChangedEventArgs<bool> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.IsVisibleChanged != null)
                obj.IsVisibleChanged(obj, e);
        }

        #endregion

        #region AccelerogramInformation Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public string AccelerogramInformation
        {
            get { return accelerogramInformation; }
            set
            {
                if (AreEqualObjects(accelerogramInformation, value))
                    return;

                var _fieldOldValue = accelerogramInformation;

                accelerogramInformation = value;

                SeismicRecordSpectrum.OnAccelerogramInformationChanged(this,
                    new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("AccelerogramInformation");
            }
        }

        private string accelerogramInformation;

        public EventHandler<PropertyValueChangedEventArgs<string>> AccelerogramInformationChanged;

        public static void OnAccelerogramInformationChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.AccelerogramInformationChanged != null)
                obj.AccelerogramInformationChanged(obj, e);
        }

        #endregion



        #region AccelerationSpectrum Property and field

        /// <summary>
        /// Gets the acceleration spectrum.
        /// </summary>
        /// <value>
        /// The acceleration spectrum.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<Point2D> AccelerationSpectrum
        {
            get { return accelerationSpectrum; }
            private set
            {
                if (AreEqualObjects(accelerationSpectrum, value))
                    return;

                var _fieldOldValue = accelerationSpectrum;

                accelerationSpectrum = value;

                SeismicRecordSpectrum.OnAccelerationSpectrumChanged(this,
                    new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("AccelerationSpectrum");
            }
        }

        private FastObservableCollection<Point2D> accelerationSpectrum;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>>
            AccelerationSpectrumChanged;

        public static void OnAccelerationSpectrumChanged(object sender,
            PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.AccelerationSpectrumChanged != null)
                obj.AccelerationSpectrumChanged(obj, e);
        }

        #endregion

        #region SpeedSpectrum Property and field

        /// <summary>
        /// Gets the speed spectrum.
        /// </summary>
        /// <value>
        /// The speed spectrum.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<Point2D> SpeedSpectrum
        {
            get { return speedSpectrum; }
            private set
            {
                if (AreEqualObjects(speedSpectrum, value))
                    return;

                var _fieldOldValue = speedSpectrum;

                speedSpectrum = value;

                SeismicRecordSpectrum.OnSpeedSpectrumChanged(this,
                    new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("SpeedSpectrum");
            }
        }

        private FastObservableCollection<Point2D> speedSpectrum;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>> SpeedSpectrumChanged;

        public static void OnSpeedSpectrumChanged(object sender,
            PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.SpeedSpectrumChanged != null)
                obj.SpeedSpectrumChanged(obj, e);
        }

        #endregion

        #region DisplacementSpectrum Property and field

        /// <summary>
        /// Gets the displacement spectrum.
        /// </summary>
        /// <value>
        /// The displacement spectrum.
        /// </value>
        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<Point2D> DisplacementSpectrum
        {
            get { return displacementSpectrum; }
            private set
            {
                if (AreEqualObjects(displacementSpectrum, value))
                    return;

                var _fieldOldValue = displacementSpectrum;

                displacementSpectrum = value;

                SeismicRecordSpectrum.OnDisplacementSpectrumChanged(this,
                    new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("DisplacementSpectrum");
            }
        }

        private FastObservableCollection<Point2D> displacementSpectrum;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>>
            DisplacementSpectrumChanged;

        public static void OnDisplacementSpectrumChanged(object sender,
            PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.DisplacementSpectrumChanged != null)
                obj.DisplacementSpectrumChanged(obj, e);
        }

        #endregion


        #region AccelerationValues Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)] 
        public FastObservableCollection<Point2D> AccelerationValues
        {
            get { return accelerationValues; }
            set
            {
                if (AreEqualObjects(accelerationValues, value))
                    return;

                var _fieldOldValue = accelerationValues;

                accelerationValues = value;

                SeismicRecordSpectrum.OnAccelerationValuesChanged(this, new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("AccelerationValues");
            }
        }

        private FastObservableCollection<Point2D> accelerationValues;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>> AccelerationValuesChanged;

        public static void OnAccelerationValuesChanged(object sender, PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.AccelerationValuesChanged != null)
                obj.AccelerationValuesChanged(obj, e);


        }

        #endregion

        #region VelocityValues Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<Point2D> VelocityValues
        {
            get { return velocityValues; }
            set
            {
                if (AreEqualObjects(velocityValues, value))
                    return;

                var _fieldOldValue = velocityValues;

                velocityValues = value;

                SeismicRecordSpectrum.OnVelocityValuesChanged(this, new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("VelocityValues");
            }
        }

        private FastObservableCollection<Point2D> velocityValues;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>> VelocityValuesChanged;

        public static void OnVelocityValuesChanged(object sender, PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.VelocityValuesChanged != null)
                obj.VelocityValuesChanged(obj, e);


        }

        #endregion

        #region DisplacementValues Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<Point2D> DisplacementValues
        {
            get { return displacementValues; }
            set
            {
                if (AreEqualObjects(displacementValues, value))
                    return;

                var _fieldOldValue = displacementValues;

                displacementValues = value;

                SeismicRecordSpectrum.OnDisplacementValuesChanged(this,
                    new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("DisplacementValues");
            }
        }

        private FastObservableCollection<Point2D> displacementValues;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>> DisplacementValuesChanged;

        public static void OnDisplacementValuesChanged(object sender,
            PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.DisplacementValuesChanged != null)
                obj.DisplacementValuesChanged(obj, e);
        }

        #endregion

        #region DisplacementValuesForTripartiteSpectrum Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public FastObservableCollection<Point2D> DisplacementValuesForTripartiteSpectrum
        {
            get { return displacementValuesForTripartiteSpectrum; }
            set
            {
                if (AreEqualObjects(displacementValuesForTripartiteSpectrum, value))
                    return;

                var _fieldOldValue = displacementValuesForTripartiteSpectrum;

                displacementValuesForTripartiteSpectrum = value;

                SeismicRecordSpectrum.OnDisplacementValuesForTripartiteSpectrumChanged(this, new PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("DisplacementValuesForTripartiteSpectrum");
            }
        }

        private FastObservableCollection<Point2D> displacementValuesForTripartiteSpectrum;

        public EventHandler<PropertyValueChangedEventArgs<FastObservableCollection<Point2D>>> DisplacementValuesForTripartiteSpectrumChanged;

        public static void OnDisplacementValuesForTripartiteSpectrumChanged(object sender, PropertyValueChangedEventArgs<FastObservableCollection<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.DisplacementValuesForTripartiteSpectrumChanged != null)
                obj.DisplacementValuesForTripartiteSpectrumChanged(obj, e);


        }

        #endregion




        #region GroundAccelerationHistory Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public List<Point2D> GroundAccelerationHistory
        {
            get { return groundAccelerationHistory; }
            set
            {
                if (AreEqualObjects(groundAccelerationHistory, value))
                    return;

                var _fieldOldValue = groundAccelerationHistory;

                groundAccelerationHistory = value;

                SeismicRecordSpectrum.OnGroundAccelerationHistoryChanged(this, new PropertyValueChangedEventArgs<List<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("GroundAccelerationHistory");
            }
        }

        private List<Point2D> groundAccelerationHistory;

        public EventHandler<PropertyValueChangedEventArgs<List<Point2D>>> GroundAccelerationHistoryChanged;

        public static void OnGroundAccelerationHistoryChanged(object sender, PropertyValueChangedEventArgs<List<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.GroundAccelerationHistoryChanged != null)
                obj.GroundAccelerationHistoryChanged(obj, e);


        }

        #endregion

        #region GroundVelocityHistory Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public List<Point2D> GroundVelocityHistory
        {
            get { return groundVelocityHistory; }
            set
            {
                if (AreEqualObjects(groundVelocityHistory, value))
                    return;

                var _fieldOldValue = groundVelocityHistory;

                groundVelocityHistory = value;

                SeismicRecordSpectrum.OnGroundVelocityHistoryChanged(this, new PropertyValueChangedEventArgs<List<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("GroundVelocityHistory");
            }
        }

        private List<Point2D> groundVelocityHistory;

        public EventHandler<PropertyValueChangedEventArgs<List<Point2D>>> GroundVelocityHistoryChanged;

        public static void OnGroundVelocityHistoryChanged(object sender, PropertyValueChangedEventArgs<List<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.GroundVelocityHistoryChanged != null)
                obj.GroundVelocityHistoryChanged(obj, e);


        }

        #endregion

        #region GroundDisplacementHistory Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public List<Point2D> GroundDisplacementHistory
        {
            get { return groundDisplacementHistory; }
            set
            {
                if (AreEqualObjects(groundDisplacementHistory, value))
                    return;

                var _fieldOldValue = groundDisplacementHistory;

                groundDisplacementHistory = value;

                SeismicRecordSpectrum.OnGroundDisplacementHistoryChanged(this, new PropertyValueChangedEventArgs<List<Point2D>>(_fieldOldValue, value));

                this.OnPropertyChanged("GroundDisplacementHistory");
            }
        }

        private List<Point2D> groundDisplacementHistory;

        public EventHandler<PropertyValueChangedEventArgs<List<Point2D>>> GroundDisplacementHistoryChanged;

        public static void OnGroundDisplacementHistoryChanged(object sender, PropertyValueChangedEventArgs<List<Point2D>> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.GroundDisplacementHistoryChanged != null)
                obj.GroundDisplacementHistoryChanged(obj, e);


        }

        #endregion


        #region PGA Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double PGA
        {
            get { return pGA; }
            set
            {
                if (AreEqualObjects(pGA, value))
                    return;

                var _fieldOldValue = pGA;

                pGA = value;

                SeismicRecordSpectrum.OnPGAChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("PGA");
            }
        }

        private double pGA;

        public EventHandler<PropertyValueChangedEventArgs<double>> PGAChanged;

        public static void OnPGAChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.PGAChanged != null)
                obj.PGAChanged(obj, e);


        }

        #endregion

        #region PGV Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double PGV
        {
            get { return pGV; }
            set
            {
                if (AreEqualObjects(pGV, value))
                    return;

                var _fieldOldValue = pGV;

                pGV = value;

                SeismicRecordSpectrum.OnPGVChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("PGV");
            }
        }

        private double pGV;

        public EventHandler<PropertyValueChangedEventArgs<double>> PGVChanged;

        public static void OnPGVChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.PGVChanged != null)
                obj.PGVChanged(obj, e);


        }

        #endregion

        #region PGD Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double PGD
        {
            get { return pGD; }
            set
            {
                if (AreEqualObjects(pGD, value))
                    return;

                var _fieldOldValue = pGD;

                pGD = value;

                SeismicRecordSpectrum.OnPGDChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("PGD");
            }
        }

        private double pGD;

        public EventHandler<PropertyValueChangedEventArgs<double>> PGDChanged;

        public static void OnPGDChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as SeismicRecordSpectrum;

            if (obj.PGDChanged != null)
                obj.PGDChanged(obj, e);


        }

        #endregion



        public List<LineGraph> Graphs = new List<LineGraph>();


        /// <summary>
        /// Computes the displacement, speed and acceleration response spectrum regarding <see cref="DampRatio"/>.
        /// </summary>
        public void ComputeResponseSpectrums()
        {
            #region accelerationSpectrum, speedSpectrum and displacementSpectrum

            var n = GroundAcceleration.Length;

            var ga = GroundAcceleration;
            var gv = new double[n];
            var gd = new double[n];

            var dt = deltaT;

            #region Calculating ground displacement history

            for (var i = 1; i < n; i++)
            {
                var tmp = (ga[i] - ga[i - 1])/deltaT;

                gv[i] = gv[i - 1] + tmp*dt*dt/2.0 + ga[i - 1]*dt;
                gd[i] = gd[i - 1] + gv[i - 1]*dt + tmp*dt*dt*dt/6.0 + ga[i - 1]*dt*dt/2.0;
            }


            for (int i = 0; i < n; i++)
            {
                groundAccelerationHistory.Add(new Point2D(i*dt, ga[i]));
                groundVelocityHistory.Add(new Point2D(i*dt, gv[i]));
                groundDisplacementHistory.Add(new Point2D(i*dt, gd[i]));
            }

            this.PGA = ga.AbsMax();

            #endregion

            var sDt = ApplicationSettings.Current.SpectrumsDt;
            var sTmax = ApplicationSettings.Current.SpectrumsTMax;

            var pga = this.PGA = ga.FastAbsMax();
            var pgv = this.pGV = gv.FastAbsMax();
            var pgd = this.pGD = gd.FastAbsMax();

            var n2 = (int) (sTmax/sDt);


            var accelSpectArr = new Point2D[n2 + 1];
            var speedSpectArr = new Point2D[n2 + 1];
            var dispSpectArr = new Point2D[n2 + 1];

            var accelValsArr = new Point2D[n2 + 1];
            var speedValsArr = new Point2D[n2 + 1];
            var dispValsArr = new Point2D[n2 + 1];

            //int i=0;

            accelSpectArr[0] = new Point2D(0, 1);
            speedSpectArr[0] = dispSpectArr[0] = new Point2D();

            accelValsArr[0] = new Point2D(0, pga);
            speedSpectArr[0] = dispSpectArr[0] = new Point2D();


            Parallel.For(1, n2 + 1, i =>
            {
                var T = sDt*i;
                var newDt = T/1000.0;

                var newArr = Utils.ChangeResolution(ga, this.deltaT, newDt);

                var systemDisp2 = CalcSdofDisplacementsV2(T, ApplicationSettings.Current.DampRatio, newDt, newArr);

                accelSpectArr[i] = new Point2D(T, systemDisp2.Item3/pga);
                speedSpectArr[i] = new Point2D(T, systemDisp2.Item2/pgv);
                dispSpectArr[i] = new Point2D(T, systemDisp2.Item1/pgd);

                accelValsArr[i] = new Point2D(T, systemDisp2.Item3);
                speedValsArr[i] = new Point2D(T, systemDisp2.Item2);
                dispValsArr[i] = new Point2D(T, systemDisp2.Item1);
            });

            this.accelerationSpectrum.Clear();
            this.speedSpectrum.Clear();
            this.displacementSpectrum.Clear();

            this.accelerationValues.Clear();
            this.velocityValues.Clear();
            this.displacementValues.Clear();


            this.accelerationSpectrum.AddItems(accelSpectArr);
            this.speedSpectrum.AddItems(speedSpectArr);
            this.displacementSpectrum.AddItems(dispSpectArr);

            this.accelerationValues.AddItems(accelValsArr);
            this.velocityValues.AddItems(speedValsArr);
            this.displacementValues.AddItems(dispValsArr);



            #endregion

            #region tripartite points

            var tripTs = Utils.GetTripartitePeriods();

            var disps = this.GetDisplacementSpectrum(tripTs.ToArray());


            var buf = new Point2D[disps.Length];

            for (var i = 0; i < disps.Length; i++)
            {
                var T = tripTs[i];
                var sd = disps[i];
                buf[i] = new Point2D(T, sd);
            }

            this.displacementValuesForTripartiteSpectrum.Clear();
            this.displacementValuesForTripartiteSpectrum.AddItems(buf);

            #endregion
        }


        /// <summary>
        /// Gets the accelerations spectrum values.
        /// </summary>
        /// <param name="ts">The periods.</param>
        /// <returns></returns>
        public double[] GetDisplacementSpectrum(double[] ts)
        {
            var dispVals = new double[ts.Length];

            Parallel.For(0, ts.Length, i =>
                //for (int i = 0; i < ts.Length; i++)
            {
                //for (var i = 1; i <= n2; i ++)
                //for (var T = sDt; T <= sTmax; T += sDt)

                var T = ts[i];
                var newDt = T/1000.0;

                var ga = GroundAcceleration;

                var newArr = GroundAcceleration;

                //if (this.DeltaT > newDt)
                newArr = Utils.ChangeResolution(ga, this.deltaT, newDt);

                var systemDisp2 = CalcSdofDisplacementsV2(T, ApplicationSettings.Current.DampRatio, newDt, newArr);

                var d = systemDisp2.Item1;

                dispVals[i] = d; //new Point2D(T, A);
            }
                );


            return dispVals;
        }

        /// <summary>
        /// Calculates the displacement history of SDoF system under given ground acceleration <see cref="ga" />.
        /// </summary>
        /// <param name="T">The period of SDoF system.</param>
        /// <param name="zetta">The damping ratio of SDoF system.</param>
        /// <param name="dt">The Dt associated with <see cref="ga" />.</param>
        /// <param name="ga">The ground acceleration history.</param>
        /// <returns>
        /// A tuple where:
        /// Item1 is A and is acceleration history of SDoF system
        /// Item2 is V and is velocity history of SDoF system
        /// Item3 is D and is displacement history of SDoF system
        /// </returns>
        public static Tuple<double[], double[], double[]> CalcSdofDisplacements(double T, double zetta,
            double dt, double[] ga)
        {
            var n = ga.Length;

            var u = DoubleArrayPool.GetArray(n); //for storing d(t)
            var ud = DoubleArrayPool.GetArray(n); //for storing v(t)
            var udd = DoubleArrayPool.GetArray(n); //for storing a(t)
            var p = DoubleArrayPool.GetArray(n); //for storing p(t) caused from ground motion

            double k, c;

            if (T <= 0)
                throw new ArgumentException("T");

            const double m = 1.0;

            k = 4*Math.PI*Math.PI*m/(T*T*(1 - zetta*zetta));
            c = 2*zetta*Math.Sqrt(k*m);

            var wn2 = Math.Sqrt(k/m);
            var T2 = 2*Math.PI/(wn2*(1 - zetta*zetta));
            var zetta2 = c/(2*m*wn2);


            const double betta = 1/6.0;
            const double gama = 1/2.0;

            for (int i = 0; i < n; i++)
                p[i] = -ga[i]*m;


            udd[0] = p[0]/m; // initialization

            var det = (betta*k*dt*dt + c*gama*dt + m)/m;

            double uddi = 0, uddi1 = 0, udi = 0, udi1 = 0, ui = 0, ui1 = 0;

            for (var i = 0; i < n - 1; i++)
            {
                var t1 = -udd[i]*(betta - 0.5)*dt*dt + ud[i]*dt + u[i];
                var t2 = ud[i] - dt*udd[i]*(gama - 1);
                var t3 = -ga[i + 1];

                u[i + 1] = betta*dt*dt*t3 + (t1*(m + c*dt*gama))/m - (betta*c*dt*dt*t2)/m;
                ud[i + 1] = (t2*(betta*k*dt*dt + m))/m + dt*gama*t3 - (dt*gama*k*t1)/m;
                udd[i + 1] = t3 - (c*t2)/m - (k*t1)/m;

                u[i + 1] /= det;
                ud[i + 1] /= det;
                udd[i + 1] /= det;
            }

            //Array.Copy(ga, u, n);
            //Array.Copy(ga, ud, n);
            //Array.Copy(ga, udd, n);

            return new Tuple<double[], double[], double[]>(u, ud, udd);
        }


        public static Tuple<double, double, double> CalcSdofDisplacementsV2
            (double T, double zetta, double dt, double[] ga)
        {
            var n = ga.Length;

            double k, c;

            if (T <= 0)
                throw new ArgumentException("T");

            const double m = 1.0;

            k = 4*Math.PI*Math.PI*m/(T*T*(1 - zetta*zetta));
            c = 2*zetta*Math.Sqrt(k*m);

            const double betta = 1/4.0;
            const double gama = 1/2.0;

            var invDet = m/(betta*k*dt*dt + c*gama*dt + m);

            double uddi = -ga[0]*m,
                uddi1 = 0,
                udi = 0,
                udi1 = 0,
                ui = 0,
                ui1 = 0;

            var maxU = 0.0;
            var maxUd = 0.0;
            var maxUdd = 0.0;

            var n2 = n - 1;


            for (var i = 0; i < n2; i++)
            {
                var t1 = -uddi*(betta - 0.5)*dt*dt + udi*dt + ui;
                var t2 = udi - dt*uddi*(gama - 1);
                var t3 = -ga[i + 1];

                ui1 = betta*dt*dt*t3 + (t1*(m + c*dt*gama)) - (betta*c*dt*dt*t2);
                udi1 = (t2*(betta*k*dt*dt + m)) + dt*gama*t3 - (dt*gama*k*t1);
                uddi1 = t3 - (c*t2) - (k*t1);

                ui1 *= invDet;
                udi1 *= invDet;
                uddi1 *= invDet;

                maxU = Math.Max(Math.Abs(ui1), maxU);
                maxUd = Math.Max(Math.Abs(udi1), maxUd);
                maxUdd = Math.Max(Math.Abs(uddi1 + ga[i + 1]), maxUdd);

                ui = ui1;
                udi = udi1;
                uddi = uddi1;
            }

            return new Tuple<double, double, double>(maxU, maxUd, maxUdd);
        }


        public static Tuple<double, double, double> CalcSdofDisplacementsV3(double T, double zetta,
            double dt, double[] ga, int gaCount)
        {
            var n = gaCount;

            double k, c;

            if (T <= 0)
                throw new ArgumentException("T");

            const double m = 1.0;

            k = 4*Math.PI*Math.PI*m/(T*T*(1 - zetta*zetta));
            c = 2*zetta*Math.Sqrt(k*m);

            const double betta = 1/4.0;
            const double gama = 1/2.0;

            var invDet = m/(betta*k*dt*dt + c*gama*dt + m);

            double uddi = -ga[0]*m, uddi1 = 0, udi = 0, udi1 = 0, ui = 0, ui1 = 0;

            var maxU = 0.0;
            var maxUd = 0.0;
            var maxUdd = 0.0;

            var n2 = n - 1;


            for (var i = 0; i < n2; i++)
            {
                var t1 = -uddi*(betta - 0.5)*dt*dt + udi*dt + ui;
                var t2 = udi - dt*uddi*(gama - 1);
                var t3 = -ga[i + 1];

                ui1 = betta*dt*dt*t3 + (t1*(m + c*dt*gama)) - (betta*c*dt*dt*t2);
                udi1 = (t2*(betta*k*dt*dt + m)) + dt*gama*t3 - (dt*gama*k*t1);
                uddi1 = t3 - (c*t2) - (k*t1);

                ui1 *= invDet;
                udi1 *= invDet;
                uddi1 *= invDet;

                maxU = Math.Max(Math.Abs(ui1), maxU);
                maxUd = Math.Max(Math.Abs(udi1), maxUd);
                maxUdd = Math.Max(Math.Abs(uddi1 + ga[i + 1]), maxUdd);

                ui = ui1;
                udi = udi1;
                uddi = uddi1;
            }

            return new Tuple<double, double, double>(maxU, maxUd, maxUdd);
        }
    }
}