using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SeismicResponseSpectrum.Ui
{
    public class TutorialStep : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialStep"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="memo">The memo.</param>
        public TutorialStep(string title, string memo)
        {
            this.title = title;
            this.memo = memo;
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

                TutorialStep.OnTitleChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("Title");
            }
        }

        private string title;

        public EventHandler<PropertyValueChangedEventArgs<string>> TitleChanged;

        public static void OnTitleChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as TutorialStep;

            if (obj.TitleChanged != null)
                obj.TitleChanged(obj, e);


        }

        #endregion

        #region Memo Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public string Memo
        {
            get { return memo; }
            set
            {
                if (AreEqualObjects(memo, value))
                    return;

                var _fieldOldValue = memo;

                memo = value;

                TutorialStep.OnMemoChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("Memo");
            }
        }

        private string memo;

        public EventHandler<PropertyValueChangedEventArgs<string>> MemoChanged;

        public static void OnMemoChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as TutorialStep;

            if (obj.MemoChanged != null)
                obj.MemoChanged(obj, e);


        }

        #endregion

        /*
        #region ApexLocation Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public Point ApexLocation
        {
            get { return apexLocation; }
            set
            {
                if (AreEqualObjects(apexLocation, value))
                    return;

                var _fieldOldValue = apexLocation;

                apexLocation = value;

                TutorialStep.OnApexLocationChanged(this, new PropertyValueChangedEventArgs<Point>(_fieldOldValue, value));

                this.OnPropertyChanged("ApexLocation");
            }
        }

        private Point apexLocation;

        public EventHandler<PropertyValueChangedEventArgs<Point>> ApexLocationChanged;

        public static void OnApexLocationChanged(object sender, PropertyValueChangedEventArgs<Point> e)
        {
            var obj = sender as TutorialStep;

            if (obj.ApexLocationChanged != null)
                obj.ApexLocationChanged(obj, e);


        }

        #endregion
        */

        #region TargetArea Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public Rect TargetArea
        {
            get { return targetArea; }
            set
            {
                if (AreEqualObjects(targetArea, value))
                    return;

                var _fieldOldValue = targetArea;

                targetArea = value;

                TutorialStep.OnTargetAreaChanged(this, new PropertyValueChangedEventArgs<Rect>(_fieldOldValue, value));

                this.OnPropertyChanged("TargetArea");
            }
        }

        private Rect targetArea;

        public EventHandler<PropertyValueChangedEventArgs<Rect>> TargetAreaChanged;

        public static void OnTargetAreaChanged(object sender, PropertyValueChangedEventArgs<Rect> e)
        {
            var obj = sender as TutorialStep;

            if (obj.TargetAreaChanged != null)
                obj.TargetAreaChanged(obj, e);


        }

        #endregion



        #region Image Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public ImageSource Image
        {
            get { return image; }
            set
            {
                if (AreEqualObjects(image, value))
                    return;

                var _fieldOldValue = image;

                image = value;

                TutorialStep.OnImageChanged(this, new PropertyValueChangedEventArgs<ImageSource>(_fieldOldValue, value));

                this.OnPropertyChanged("Image");
            }
        }

        private ImageSource image;

        public EventHandler<PropertyValueChangedEventArgs<ImageSource>> ImageChanged;

        public static void OnImageChanged(object sender, PropertyValueChangedEventArgs<ImageSource> e)
        {
            var obj = sender as TutorialStep;

            if (obj.ImageChanged != null)
                obj.ImageChanged(obj, e);


        }

        #endregion

        #region Tag Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public object Tag
        {
            get { return tag; }
            set
            {
                if (AreEqualObjects(tag, value))
                    return;

                var _fieldOldValue = tag;

                tag = value;

                TutorialStep.OnTagChanged(this, new PropertyValueChangedEventArgs<object>(_fieldOldValue, value));

                this.OnPropertyChanged("Tag");
            }
        }

        private object tag;

        public EventHandler<PropertyValueChangedEventArgs<object>> TagChanged;

        public static void OnTagChanged(object sender, PropertyValueChangedEventArgs<object> e)
        {
            var obj = sender as TutorialStep;

            if (obj.TagChanged != null)
                obj.TagChanged(obj, e);


        }

        #endregion

        #region Activated event

        public event EventHandler Activated
        {
            [MethodImpl(MethodImplOptions.Synchronized)] add { OnActivatedAdd(this, value); }
            [MethodImpl(MethodImplOptions.Synchronized)] remove { OnActivatedRemove(this, value); }
        }

        private EventHandler activated;

        public static void OnActivatedAdd(object sender, EventHandler handler)
        {
            var obj = sender as TutorialStep;
            obj.activated = (EventHandler) Delegate.Combine(obj.activated, handler);
        }

        public static void OnActivatedRemove(object sender, EventHandler handler)
        {
            var obj = sender as TutorialStep;
            obj.activated = (EventHandler) Delegate.Remove(obj.activated, handler);
        }

        #endregion

        #region StepDone event

        public event EventHandler StepDone
        {
            [MethodImpl(MethodImplOptions.Synchronized)] add { OnStepDoneAdd(this, value); }
            [MethodImpl(MethodImplOptions.Synchronized)] remove { OnStepDoneRemove(this, value); }
        }

        private EventHandler stepDone;

        public static void OnStepDoneAdd(object sender, EventHandler handler)
        {
            var obj = sender as TutorialStep;
            obj.stepDone = (EventHandler) Delegate.Combine(obj.stepDone, handler);
        }

        public static void OnStepDoneRemove(object sender, EventHandler handler)
        {
            var obj = sender as TutorialStep;
            obj.stepDone = (EventHandler) Delegate.Remove(obj.stepDone, handler);
        }

        #endregion


        public void MarkAsDone()
        {
            if (this.stepDone != null)
                this.stepDone(this, null);
        }

        public void MarkAsActivated()
        {
            if (this.activated != null)
                this.activated(this, null);
        }
    }
}
