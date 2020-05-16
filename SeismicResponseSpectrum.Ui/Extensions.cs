using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace SeismicResponseSpectrum.Ui
{
    public static class Extensions
    {
        public static int FirstIndexOf<T>(this IEnumerable<T> arr, Func<T, bool> condition)
        {
            var cnt = 0;

            foreach (var item in arr)
            {

                if (condition(item))
                    return cnt;

                cnt++;
            }


            return -1;
        }

        public static Rect GetAbsoltutePlacement(this FrameworkElement element, bool relativeToScreen = false)
        {
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }
            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }


        public static T GetChildOfType<T>(this DependencyObject depObj)
    where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static double Abs(this double val)
        {
            return Math.Abs(val);
        }

        /// <summary>
        /// Calculates the Maximum absolute value of the <see cref="vals"/>
        /// </summary>
        /// <param name="vals">The values.</param>
        public static double AbsMax(this IEnumerable<double> vals)
        {
            return Math.Max(
                vals.Max().Abs(),
                vals.Min().Abs());
        }


        public static double FastAbsMax(this double[] arr)
        {
            //reversed and unrolled version of: for(var i = 0; i < arr.Length; i ++)

            var buf = Math.Abs(arr[0]);

            for (int i = 0; i < arr.Length; i++)
            {
                if (Math.Abs(arr[i]) > buf)
                    buf = Math.Abs(arr[i]);
            }

            return buf;
        }


        public static bool IsModal(this Window window)
        {
            return (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);
        }

        public static void DispatcherInvoke(this DispatcherObject obj, Action act)
        {
            obj.Dispatcher.Invoke(DispatcherPriority.Normal, act);
        }


        public static void RemoveChildsOfType<T>(this IList lst)
        {
            for (int i = lst.Count - 1; i >= 0; i--)
            {
                if (lst[i] is T)
                    lst.RemoveAt(i);
            }
        }


        public static double GetLength(this Line line)
        {
            return Math.Sqrt((line.X2 - line.X1)*(line.X2 - line.X1) + (line.Y2 - line.Y1)*(line.Y2 - line.Y1));
        }
    }


    /// <summary>
    /// Extensions methos for using reflection to get / set member values
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the public or private member using reflection.
        /// </summary>
        /// <param name="obj">The source target.</param>
        /// <param name="memberName">Name of the field or property.</param>
        /// <returns>the value of member</returns>
        public static object GetMemberValue(this object obj, string memberName)
        {
            var memInf = GetMemberInfo(obj, memberName);

            if (memInf == null)
                throw new System.Exception("memberName");

            if (memInf is System.Reflection.PropertyInfo)
                return memInf.As<System.Reflection.PropertyInfo>().GetValue(obj, null);

            if (memInf is System.Reflection.FieldInfo)
                return memInf.As<System.Reflection.FieldInfo>().GetValue(obj);

            throw new System.Exception();
        }

        /// <summary>
        /// Gets the public or private member using reflection.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="memberName">Name of the field or property.</param>
        /// <returns>Old Value</returns>
        public static object SetMemberValue(this object obj, string memberName, object newValue)
        {
            var memInf = GetMemberInfo(obj, memberName);


            if (memInf == null)
                throw new System.Exception("memberName");

            var oldValue = obj.GetMemberValue(memberName);

            if (memInf is System.Reflection.PropertyInfo)
                memInf.As<System.Reflection.PropertyInfo>().SetValue(obj, newValue, null);
            else if (memInf is System.Reflection.FieldInfo)
                memInf.As<System.Reflection.FieldInfo>().SetValue(obj, newValue);
            else
                throw new System.Exception();

            return oldValue;
        }

        /// <summary>
        /// Gets the member info
        /// </summary>
        /// <param name="obj">source object</param>
        /// <param name="memberName">name of member</param>
        /// <returns>instanse of MemberInfo corresponsing to member</returns>
        private static System.Reflection.MemberInfo GetMemberInfo(object obj, string memberName)
        {
            var prps = new System.Collections.Generic.List<System.Reflection.PropertyInfo>();

            prps.Add(obj.GetType().GetProperty(memberName,
                                               System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance |
                                               System.Reflection.BindingFlags.FlattenHierarchy));
            prps = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(prps, i => !ReferenceEquals(i, null)));
            if (prps.Count != 0)
                return prps[0];

            var flds = new System.Collections.Generic.List<System.Reflection.FieldInfo>();

            flds.Add(obj.GetType().GetField(memberName,
                                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.FlattenHierarchy));

            //to add more types of properties

            flds = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(flds, i => !ReferenceEquals(i, null)));

            if (flds.Count != 0)
                return flds[0];

            return null;
        }

        [System.Diagnostics.DebuggerHidden]
        public static T As<T>(this object obj)
        {
            return (T)obj;
        }
    }
}
