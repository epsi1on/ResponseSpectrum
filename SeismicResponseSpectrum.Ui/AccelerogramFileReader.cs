using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SeismicResponseSpectrum.Ui
{
    public class AccelerogramFileReader:INotifyPropertyChanged
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

        #region FileContent Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public string FileContent
        {
            get { return fileContent; }
            set
            {
                if (AreEqualObjects(fileContent, value))
                    return;

                var _fieldOldValue = fileContent;

                fileContent = value;

                AccelerogramFileReader.OnFileContentChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("FileContent");
            }
        }

        private string fileContent;

        public EventHandler<PropertyValueChangedEventArgs<string>> FileContentChanged;

        public static void OnFileContentChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.FileContentChanged != null)
                obj.FileContentChanged(obj, e);


        }

        #endregion

        #region FirstLine Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public int FirstLine
        {
            get { return firstLine; }
            set
            {
                if (AreEqualObjects(firstLine, value))
                    return;

                var _fieldOldValue = firstLine;

                firstLine = value;

                AccelerogramFileReader.OnFirstLineChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                this.OnPropertyChanged("FirstLine");
            }
        }

        private int firstLine;

        public EventHandler<PropertyValueChangedEventArgs<int>> FirstLineChanged;

        public static void OnFirstLineChanged(object sender, PropertyValueChangedEventArgs<int> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.FirstLineChanged != null)
                obj.FirstLineChanged(obj, e);


        }

        #endregion

        #region LastLine Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public int LastLine
        {
            get { return lastLine; }
            set
            {
                if (AreEqualObjects(lastLine, value))
                    return;

                var _fieldOldValue = lastLine;

                lastLine = value;

                AccelerogramFileReader.OnLastLineChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                this.OnPropertyChanged("LastLine");
            }
        }

        private int lastLine;

        public EventHandler<PropertyValueChangedEventArgs<int>> LastLineChanged;

        public static void OnLastLineChanged(object sender, PropertyValueChangedEventArgs<int> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.LastLineChanged != null)
                obj.LastLineChanged(obj, e);


        }

        #endregion

        #region TimeStepDt Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public double TimeStepDt
        {
            get { return timeStepDt; }
            set
            {
                if (AreEqualObjects(timeStepDt, value))
                    return;

                var _fieldOldValue = timeStepDt;

                timeStepDt = value;

                AccelerogramFileReader.OnTimeStepDtChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                this.OnPropertyChanged("TimeStepDt");
            }
        }

        private double timeStepDt;

        public EventHandler<PropertyValueChangedEventArgs<double>> TimeStepDtChanged;

        public static void OnTimeStepDtChanged(object sender, PropertyValueChangedEventArgs<double> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.TimeStepDtChanged != null)
                obj.TimeStepDtChanged(obj, e);


        }

        #endregion

        #region AccelerationColumn Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public int AccelerationColumn
        {
            get { return accelerationColumn; }
            set
            {
                if (AreEqualObjects(accelerationColumn, value))
                    return;

                var _fieldOldValue = accelerationColumn;

                accelerationColumn = value;

                AccelerogramFileReader.OnAccelerationColumnChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                this.OnPropertyChanged("AccelerationColumn");
            }
        }

        private int accelerationColumn;

        public EventHandler<PropertyValueChangedEventArgs<int>> AccelerationColumnChanged;

        public static void OnAccelerationColumnChanged(object sender, PropertyValueChangedEventArgs<int> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.AccelerationColumnChanged != null)
                obj.AccelerationColumnChanged(obj, e);


        }

        #endregion

        #region TimeColumn Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public int TimeColumn
        {
            get { return timeColumn; }
            set
            {
                if (AreEqualObjects(timeColumn, value))
                    return;

                var _fieldOldValue = timeColumn;

                timeColumn = value;

                AccelerogramFileReader.OnTimeColumnChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                this.OnPropertyChanged("TimeColumn");
            }
        }

        private int timeColumn;

        public EventHandler<PropertyValueChangedEventArgs<int>> TimeColumnChanged;

        public static void OnTimeColumnChanged(object sender, PropertyValueChangedEventArgs<int> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.TimeColumnChanged != null)
                obj.TimeColumnChanged(obj, e);


        }

        #endregion

        #region ContentType Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public ContentAcceleratinsFormat ContentType
        {
            get { return contentType; }
            set
            {
                if (AreEqualObjects(contentType, value))
                    return;

                var _fieldOldValue = contentType;

                contentType = value;

                AccelerogramFileReader.OnContentTypeChanged(this, new PropertyValueChangedEventArgs<ContentAcceleratinsFormat>(_fieldOldValue, value));

                this.OnPropertyChanged("ContentType");
            }
        }

        private ContentAcceleratinsFormat contentType;

        public EventHandler<PropertyValueChangedEventArgs<ContentAcceleratinsFormat>> ContentTypeChanged;

        public static void OnContentTypeChanged(object sender, PropertyValueChangedEventArgs<ContentAcceleratinsFormat> e)
        {
            var obj = sender as AccelerogramFileReader;

            if (obj.ContentTypeChanged != null)
                obj.ContentTypeChanged(obj, e);


        }

        #endregion


        /// <summary>
        /// Gets information as response spectrum.
        /// </summary>
        /// <returns></returns>
        public SeismicRecordSpectrum ToSeismicRecordSpectrum()
        {
            var buf = new SeismicRecordSpectrum();

            var lines = Regex.Split(fileContent, "\r\n|\r|\n");

            var values = new List<double>();


            for (var i = firstLine - 1; i < lastLine; i++)
            {
                if (contentType == ContentAcceleratinsFormat.SingleAccelerationValuesPerLine ||
                    contentType == ContentAcceleratinsFormat.MultipleAccelerationsPerLine)
                {
                    var vals = lines[i].Split(' ');

                    double tmp;

                    foreach (var val in vals)
                        if (!string.IsNullOrEmpty(val))
                            values.Add(GetAccelerationValueInSi(val));
                }

            }

            buf.DeltaT = this.TimeStepDt;
            buf.GroundAcceleration = values.ToArray();

            return buf;
        }

        /// <summary>
        /// Gets the acceleration value in SI system (m/s62).
        /// </summary>
        /// <param name="acceleration">The acceleration text.</param>
        /// <returns>Acceleration value in m/s^2</returns>
        private double GetAccelerationValueInSi(string acceleration)
        {
            //By default it is assumed that acceleration values are in g (standard gravity of earth, equals to 9.81 m/s2)
            var val = double.Parse(acceleration)*9.80665;
            return val;
        }

        /// <summary>
        /// Represents the format of accelerations of points in the file
        /// </summary>
        public enum ContentAcceleratinsFormat
        {
            [Description("Single Acceleration Value Per Line")]
            SingleAccelerationValuesPerLine,
            
            [Description("Time & Acceleration Values Per Line")]
            TimeAndAccelerationValuesPerLine,
            
            [Description("Multiple Acceleration Values Per Line")]
            MultipleAccelerationsPerLine
        }
    }
}
