using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeismicResponseSpectrum.Ui
{
    public class DragBehavior
    {
        private Point elementStartPosition;
        private Point mouseStartPosition;
        private TranslateTransform transform = new TranslateTransform();

        public void OnAttached(Canvas obj)
        {
            var parent = obj;

            var tr = obj.Children.Cast<object>().FirstOrDefault(i => i is Transform);
            
            obj.RenderTransform = transform;

            obj.MouseLeftButtonDown += (sender, e) =>
            {
                elementStartPosition = obj.TranslatePoint(new Point(), parent);
                mouseStartPosition = e.GetPosition(parent);
                obj.CaptureMouse();
            };

            obj.MouseLeftButtonUp += (sender, e) =>
            {
                obj.ReleaseMouseCapture();
            };

            obj.MouseMove += (sender, e) =>
            {
                var diff = e.GetPosition(parent) - mouseStartPosition;
                if (obj.IsMouseCaptured)
                {
                    transform.X = diff.X;
                    transform.Y = diff.Y;
                }
            };
        }
    }
}
