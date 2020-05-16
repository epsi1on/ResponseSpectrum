using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Interaction logic for ImportAccelerogramDialog.xaml
    /// </summary>
    public partial class ImportAccelerogramDialog : Window
    {
        public ImportAccelerogramDialog()
        {
            InitializeComponent();
            this.DataContext = this.Context = new ImportAccelerogramDataContext();
            txtFirstLine.GotFocus += (sender, args) => Context.IsFirstLineFocused = true;
            txtFirstLine.LostFocus += (sender, args) => Context.IsFirstLineFocused = false;

            //txtLastLine.GotFocus += (sender, args) => Context.IsLastLineFocused = true;
            //txtLastLine.LostFocus += (sender, args) => Context.IsLastLineFocused = false;
            //this.Loaded+=OnLoaded;
        }

        public new bool? ShowDialog()
        {
            var dlg = new OpenFileDialog();

            if (!dlg.ShowDialog().Value)
                return false;

            var file = dlg.FileName;

            var fileContent = System.IO.File.ReadAllText(file);

            //fileContent = fileContent.Replace("\r\n", "\r");
            //fileContent = fileContent.Replace('\n', '\r');

            //fileContent = Regex.Replace(fileContent, @"^[\r]*", "", RegexOptions.Multiline);

            Context.AccelerationFileContent = fileContent;
            Context.RecordName = System.IO.Path.GetFileNameWithoutExtension(file);

            //try
            {
                Context.SmartDetectParameters();

                
            }
            //catch (Exception ex)
            {
            }
            

            return base.ShowDialog();
        }

        


        public Brush HighLightBackgroundBrush = Brushes.LightGreen;

        public SeismicRecordSpectrum Record { get; private set; }

        private ImportAccelerogramDataContext Context;

        private class ImportAccelerogramDataContext : INotifyPropertyChanged,IDataErrorInfo
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

                    ImportAccelerogramDataContext.OnFirstLineChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                    this.OnPropertyChanged("FirstLine");
                }
            }

            private int firstLine;

            public EventHandler<PropertyValueChangedEventArgs<int>> FirstLineChanged;

            public static void OnFirstLineChanged(object sender, PropertyValueChangedEventArgs<int> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.FirstLineChanged != null)
                    obj.FirstLineChanged(obj, e);

                if (obj.IsFirstLineFocused)
                {
                    obj.HighlightedLineNumber = -1;
                    obj.HighlightedLineNumber = e.NewValue;
                }

                obj.RegenerateErrorMessage();
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

                    ImportAccelerogramDataContext.OnLastLineChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                    this.OnPropertyChanged("LastLine");
                }
            }

            private int lastLine;

            public EventHandler<PropertyValueChangedEventArgs<int>> LastLineChanged;

            public static void OnLastLineChanged(object sender, PropertyValueChangedEventArgs<int> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.LastLineChanged != null)
                    obj.LastLineChanged(obj, e);

                obj.RegenerateErrorMessage();
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

                    ImportAccelerogramDataContext.OnTimeStepDtChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                    this.OnPropertyChanged("TimeStepDt");
                }
            }

            private double timeStepDt;

            public EventHandler<PropertyValueChangedEventArgs<double>> TimeStepDtChanged;

            public static void OnTimeStepDtChanged(object sender, PropertyValueChangedEventArgs<double> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.TimeStepDtChanged != null)
                    obj.TimeStepDtChanged(obj, e);

                obj.RegenerateErrorMessage();
            }

            #endregion

            #region ScalingFactor Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public double ScalingFactor
            {
                get { return scalingFactor; }
                set
                {
                    if (AreEqualObjects(scalingFactor, value))
                        return;

                    var _fieldOldValue = scalingFactor;

                    scalingFactor = value;

                    ImportAccelerogramDataContext.OnScalingFactorChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

                    this.OnPropertyChanged("ScalingFactor");
                }
            }

            private double scalingFactor=1.0;

            public EventHandler<PropertyValueChangedEventArgs<double>> ScalingFactorChanged;

            public static void OnScalingFactorChanged(object sender, PropertyValueChangedEventArgs<double> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.ScalingFactorChanged != null)
                    obj.ScalingFactorChanged(obj, e);

                obj.RegenerateErrorMessage();
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

                    ImportAccelerogramDataContext.OnAccelerationColumnChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                    this.OnPropertyChanged("AccelerationColumn");
                }
            }

            private int accelerationColumn=2;

            public EventHandler<PropertyValueChangedEventArgs<int>> AccelerationColumnChanged;

            public static void OnAccelerationColumnChanged(object sender, PropertyValueChangedEventArgs<int> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.AccelerationColumnChanged != null)
                    obj.AccelerationColumnChanged(obj, e);

                obj.RegenerateErrorMessage();
                obj.OnPropertyChanged(SensitiveProperties);
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

                    ImportAccelerogramDataContext.OnTimeColumnChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                    this.OnPropertyChanged("TimeColumn");
                }
            }

            private int timeColumn=1;

            public EventHandler<PropertyValueChangedEventArgs<int>> TimeColumnChanged;

            public static void OnTimeColumnChanged(object sender, PropertyValueChangedEventArgs<int> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.TimeColumnChanged != null)
                    obj.TimeColumnChanged(obj, e);

                obj.RegenerateErrorMessage();
                obj.OnPropertyChanged(SensitiveProperties);
            }

            #endregion


            #region SinlgeAcceleration Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public bool SinlgeAcceleration
            {
                get { return sinlgeAcceleration; }
                set
                {
                    if (AreEqualObjects(sinlgeAcceleration, value))
                        return;

                    var _fieldOldValue = sinlgeAcceleration;

                    sinlgeAcceleration = value;

                    ImportAccelerogramDataContext.OnSinlgeAccelerationChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                    this.OnPropertyChanged("SinlgeAcceleration");
                }
            }

            private bool sinlgeAcceleration=true;

            public EventHandler<PropertyValueChangedEventArgs<bool>> SinlgeAccelerationChanged;

            public static void OnSinlgeAccelerationChanged(object sender, PropertyValueChangedEventArgs<bool> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.SinlgeAccelerationChanged != null)
                    obj.SinlgeAccelerationChanged(obj, e);
                obj.OnPropertyChanged(SensitiveProperties);

            }

            #endregion

            #region TimeAndAcceleration Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public bool TimeAndAcceleration
            {
                get { return timeAndAcceleration; }
                set
                {
                    if (AreEqualObjects(timeAndAcceleration, value))
                        return;

                    var _fieldOldValue = timeAndAcceleration;

                    timeAndAcceleration = value;

                    ImportAccelerogramDataContext.OnTimeAndAccelerationChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                    this.OnPropertyChanged("TimeAndAcceleration");
                }
            }

            private bool timeAndAcceleration;

            public EventHandler<PropertyValueChangedEventArgs<bool>> TimeAndAccelerationChanged;

            public static void OnTimeAndAccelerationChanged(object sender, PropertyValueChangedEventArgs<bool> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.TimeAndAccelerationChanged != null)
                    obj.TimeAndAccelerationChanged(obj, e);

                obj.OnPropertyChanged(SensitiveProperties);
            }

            #endregion

            #region MultipleAcceleration Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public bool MultipleAcceleration
            {
                get { return multipleAcceleration; }
                set
                {
                    if (AreEqualObjects(multipleAcceleration, value))
                        return;

                    var _fieldOldValue = multipleAcceleration;

                    multipleAcceleration = value;

                    ImportAccelerogramDataContext.OnMultipleAccelerationChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                    this.OnPropertyChanged("MultipleAcceleration");
                }
            }

            private bool multipleAcceleration;

            public EventHandler<PropertyValueChangedEventArgs<bool>> MultipleAccelerationChanged;

            public static void OnMultipleAccelerationChanged(object sender, PropertyValueChangedEventArgs<bool> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.MultipleAccelerationChanged != null)
                    obj.MultipleAccelerationChanged(obj, e);

                obj.OnPropertyChanged(SensitiveProperties);
            }

            #endregion

            #region AccelerationFileContent Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public string AccelerationFileContent
            {
                get { return accelerationFileContent; }
                set
                {
                    if (AreEqualObjects(accelerationFileContent, value))
                        return;

                    var _fieldOldValue = accelerationFileContent;

                    accelerationFileContent = value;

                    ImportAccelerogramDataContext.OnAccelerationFileContentChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                    this.OnPropertyChanged("AccelerationFileContent");
                }
            }

            private string accelerationFileContent;

            public EventHandler<PropertyValueChangedEventArgs<string>> AccelerationFileContentChanged;

            public static void OnAccelerationFileContentChanged(object sender, PropertyValueChangedEventArgs<string> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.AccelerationFileContentChanged != null)
                    obj.AccelerationFileContentChanged(obj, e);


            }

            #endregion


            #region IsFirstLineFocused Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public bool IsFirstLineFocused
            {
                get { return isFirstLineFocused; }
                set
                {
                    if (AreEqualObjects(isFirstLineFocused, value))
                        return;

                    var _fieldOldValue = isFirstLineFocused;

                    isFirstLineFocused = value;

                    ImportAccelerogramDataContext.OnIsFirstLineFocusedChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                    this.OnPropertyChanged("IsFirstLineFocused");
                }
            }

            private bool isFirstLineFocused;

            public EventHandler<PropertyValueChangedEventArgs<bool>> IsFirstLineFocusedChanged;

            public static void OnIsFirstLineFocusedChanged(object sender, PropertyValueChangedEventArgs<bool> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.IsFirstLineFocusedChanged != null)
                    obj.IsFirstLineFocusedChanged(obj, e);

                if (e.NewValue)
                    obj.HighlightedLineNumber = obj.FirstLine;
                else
                    obj.HighlightedLineNumber = -1;
            }

            #endregion

            #region IsLastLineFocused Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public bool IsLastLineFocused
            {
                get { return isLastLineFocused; }
                set
                {
                    if (AreEqualObjects(isLastLineFocused, value))
                        return;

                    var _fieldOldValue = isLastLineFocused;

                    isLastLineFocused = value;

                    ImportAccelerogramDataContext.OnIsLastLineFocusedChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                    this.OnPropertyChanged("IsLastLineFocused");
                }
            }

            private bool isLastLineFocused;

            public EventHandler<PropertyValueChangedEventArgs<bool>> IsLastLineFocusedChanged;

            public static void OnIsLastLineFocusedChanged(object sender, PropertyValueChangedEventArgs<bool> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.IsLastLineFocusedChanged != null)
                    obj.IsLastLineFocusedChanged(obj, e);

                if (e.NewValue)
                    obj.HighlightedLineNumber = obj.LastLine;
                else
                    obj.HighlightedLineNumber = -1;
            }

            #endregion


            #region HighlightedLineNumber Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public int HighlightedLineNumber
            {
                get { return highlightedLineNumber; }
                set
                {
                    if (AreEqualObjects(highlightedLineNumber, value))
                        return;

                    var _fieldOldValue = highlightedLineNumber;

                    highlightedLineNumber = value;

                    ImportAccelerogramDataContext.OnHighlightedLineNumberChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                    this.OnPropertyChanged("HighlightedLineNumber");
                }
            }

            private int highlightedLineNumber = -1;

            public EventHandler<PropertyValueChangedEventArgs<int>> HighlightedLineNumberChanged;

            public static void OnHighlightedLineNumberChanged(object sender, PropertyValueChangedEventArgs<int> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.HighlightedLineNumberChanged != null)
                    obj.HighlightedLineNumberChanged(obj, e);


            }

            #endregion

            #region Error Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public string Error
            {
                get { return error; }
                set
                {
                    if (AreEqualObjects(error, value))
                        return;

                    var _fieldOldValue = error;

                    error = value;

                    ImportAccelerogramDataContext.OnErrorChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                    this.OnPropertyChanged("Error");
                }
            }

            private string error;

            public EventHandler<PropertyValueChangedEventArgs<string>> ErrorChanged;

            public static void OnErrorChanged(object sender, PropertyValueChangedEventArgs<string> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.ErrorChanged != null)
                    obj.ErrorChanged(obj, e);


            }

            #endregion

            #region HaveError Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public bool HaveError
            {
                get { return haveError; }
                set
                {
                    if (AreEqualObjects(haveError, value))
                        return;

                    var _fieldOldValue = haveError;

                    haveError = value;

                    ImportAccelerogramDataContext.OnHaveErrorChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                    this.OnPropertyChanged("HaveError");
                }
            }

            private bool haveError;

            public EventHandler<PropertyValueChangedEventArgs<bool>> HaveErrorChanged;

            public static void OnHaveErrorChanged(object sender, PropertyValueChangedEventArgs<bool> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.HaveErrorChanged != null)
                    obj.HaveErrorChanged(obj, e);


            }

            #endregion

            #region RecordName Property and field

            [Obfuscation(Exclude = true, ApplyToMembers = false)]
            public string RecordName
            {
                get { return recordName; }
                set
                {
                    if (AreEqualObjects(recordName, value))
                        return;

                    var _fieldOldValue = recordName;

                    recordName = value;

                    ImportAccelerogramDataContext.OnRecordNameChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                    this.OnPropertyChanged("RecordName");
                }
            }

            private string recordName;

            public EventHandler<PropertyValueChangedEventArgs<string>> RecordNameChanged;

            public static void OnRecordNameChanged(object sender, PropertyValueChangedEventArgs<string> e)
            {
                var obj = sender as ImportAccelerogramDataContext;

                if (obj.RecordNameChanged != null)
                    obj.RecordNameChanged(obj, e);


            }

            #endregion


            public void SmartDetectParameters()
            {
                var lines = Regex.Split(accelerationFileContent, "\r\n|\r|\n");
                var dtBias = 0.1;

                for (var i = 0; i < lines.Length; i++)
                {
                    if (i > 10)
                        break;

                    
                    var line = lines[i].ToLower();

                    var splitted = System.Text.RegularExpressions.Regex.Split(line, @"\s+");

                    if(firstLine==0)
                        if (AreAllMembersDoubleVals(splitted))
                        {
                            var numbers = splitted.Count(IsConvertibleToDouble);

                            if (numbers == 1)
                                sinlgeAcceleration = true;

                            if (numbers == 2)
                            {
                                timeAndAcceleration = true;

                            }

                            if (numbers > 2)
                                multipleAcceleration = true;

                            firstLine = i;
                        }


                    if(timeStepDt==0.0)
                        if (splitted.Any(j => j.Contains("dt")))
                        {
                            var index = splitted.FirstIndexOf(j => j.Contains("dt"));
                            var tmp = splitted.Skip(index).Select(j=>j.Replace(",","")).Where(IsConvertibleToDouble).FirstOrDefault(j => double.Parse(j) < dtBias);

                            if (!string.IsNullOrEmpty(tmp) )
                            {
                                timeStepDt  = double.Parse(tmp);
                            }
                        }
                }

                lastLine = lines.Length;



                this.OnPropertyChanged("FirstLine", "SinlgeAcceleration", "MultipleAcceleration", "TimeAndAcceleration", "TimeStepDt");
            }


            private static bool AreAllMembersDoubleVals(string[] arr)
            {
                var buf = true;
                var tmp = 0.0;

                foreach (var str in arr)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    if (!double.TryParse(str, out tmp))
                        return false;
                }

                return true;
            }

            private static bool IsConvertibleToDouble(string str)
            {
                var tmp = 0.0;

                return double.TryParse(str, out tmp);
            }

            public string this[string columnName]
            {
                get
                {
                    switch (columnName)
                    {
                        case "FirstLine":
                            if (firstLine < 0) return "Invalid Value";
                            break;

                        case "LastLine":
                            if (lastLine <= firstLine || lastLine <= 0) return "Invalid Value";
                            break;

                        case "TimeStepDt":
                            if(!timeAndAcceleration) 
                                if (timeStepDt < 0||Math.Abs(timeStepDt) < 1e-6)
                                    return "Invalid Value";
                            break;
                        case "ScalingFactor":
                            if (ScalingFactor<=0)
                                    return "Invalid Value";
                            break;
                        case "AccelerationColumn":
                            if (!timeAndAcceleration) break;
                            if (AccelerationColumn <= 0) return "Invalid Value";
                            if (AccelerationColumn == timeColumn) return "Value should not equal to Time Column";
                            break;
                        case "RecordName":
                            if (string.IsNullOrEmpty(recordName))
                                return "Invalid Value";
                            break;
                            
                        case "TimeColumn":
                            if (!timeAndAcceleration) break;
                            if (TimeColumn <= 0) return "Invalid Value";
                            if (TimeColumn == accelerationColumn) return "Value should not equal to Acceleration Column";
                            break;

                    }

                    return "";
                }
            }


            private static readonly string[] SensitiveProperties = new string[]
            {
                "FirstLine", "LastLine", "TimeStepDt",
                "ScalingFactor", "AccelerationColumn", "TimeColumn"
            };


            private void RegenerateErrorMessage()
            {
                if (SensitiveProperties.Any(i => !string.IsNullOrEmpty(this[i])))
                {
                    HaveError = true;
                    Error = "Invalid value(s)";
                }
                else
                {
                    HaveError = false;
                    Error = "";
                }
            }

            public SeismicRecordSpectrum GenerateRecord()
            {
                var g = 10.0;//in m/s^2

                var buf = new SeismicRecordSpectrum();

                var lines = Regex.Split(accelerationFileContent, "\r\n|\r|\n");

                var accels = new List<double>();
                var times = new List<double>();

                var lastLine2 = Math.Min(lastLine, lines.Length);

                for (var i = firstLine; i < lastLine2; i++)
                {
                    if (sinlgeAcceleration)
                    {
                        if (IsConvertibleToDouble(lines[i]))
                            accels.Add(double.Parse(lines[i].Trim()));
                    }

                    if (timeAndAcceleration)
                    {
                        var splitted = Regex.Split(lines[i].Trim(), @"\s+");

                        if (splitted.Length > 1)
                        {
                            var accelVal = GetIthColumn(splitted, accelerationColumn - 1);
                            var timeVal = GetIthColumn(splitted, timeColumn - 1);

                            accels.Add(double.Parse(accelVal));
                            times.Add(double.Parse(timeVal));
                        }
                    }

                    if (multipleAcceleration)
                    {
                        var splitted = System.Text.RegularExpressions.Regex.Split(lines[i].Trim(), @"\s+");

                        if (splitted.Length != 0)
                            AddAccelerationsValuesToList(splitted, accels);
                    }
                }


                if (timeAndAcceleration)
                {
                    var d0 = times[1] - times[0];

                    for (int i = 0; i < times.Count-1; i++)
                    {
                        if (Math.Abs(times[i + 1] - times[i] - d0) > 1e-6)
                            throw new Exception(string.Format("Dt should be constant ({0}'th record)",i+1));
                    }
                    buf.DeltaT = d0;
                }
                else
                {
                    buf.DeltaT = this.timeStepDt;    
                }
                
                
                for (int i = 0; i < accels.Count; i++)
                    accels[i] = accels[i]*scalingFactor*g;

                buf.GroundAcceleration = accels.ToArray();
                buf.TMax = accels.Count*timeStepDt;
                buf.Title = recordName;
                buf.Color = PickBrush();

                return buf;
            }

            private Color PickBrush()
            {
                return GetRandomDarkColor();
            }

            private Color GetRandomDarkColor()
            {
                var result = Colors.Transparent;

                Random rnd = new Random();

                Type brushesType = typeof(Colors);

                PropertyInfo[] properties = brushesType.GetProperties();


                while (true)
                {
                    int random = rnd.Next(properties.Length);
                    result = (Color) properties[random].GetValue(null, null);

                    if (result.A == 255 && result.R + result.G + result.B < 600)
                        return result;
                }
            }

        }

        private static void AddAccelerationsValuesToList(string[] arr, List<double> lst)
        {
            double tmp;

            foreach (var s in arr)
            {
                if(!string.IsNullOrEmpty(s))
                    if(double.TryParse(s,out tmp))
                        lst.Add(tmp);
            }
        }

        private static string GetIthColumn(string[] arr, int i)
        {
            var cnt = 0;
            for (int j = 0; j < arr.Length; j++)
            {
                if (!string.IsNullOrEmpty(arr[i].Trim()))
                {
                    if (i == cnt)
                        return arr[j];

                    cnt++;
                }
            }

            throw new Exception();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
# if !DEBUG
            try
#endif
            {

                this.Record = Context.GenerateRecord();
                if (this.IsModal())
                    this.DialogResult = true;
            }
# if !DEBUG
            catch (Exception ex)

            {
                Context.Error = ex.Message;
            }
#endif
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsModal())
                this.DialogResult = false;

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
