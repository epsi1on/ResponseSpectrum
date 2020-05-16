using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SeismicResponseSpectrum.Ui
{
    internal class ShapeExtender
    {
        public static readonly DependencyProperty PenProperty = DependencyProperty.RegisterAttached(
            "Pen", typeof (Pen), typeof (ShapeExtender),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PenProperty_Changed))
            );

        private static void PenProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)

        {
            Shape shape = d as Shape;
            var pen = e.NewValue as Pen;

            if (pen != null)
                pen.Changed += (a, b) =>
                {
                    if (pen == shape.GetValue(PenProperty))
                    {
                        ApplyPen(shape, pen);
                    }
                };

            if (shape != null )
            {
                ApplyPen(shape, pen);
            }

        }

        


        private static void ApplyPen(Shape shape, Pen pen)
        {
            if (pen != null)
            {
                shape.Stroke = pen.Brush;

                shape.StrokeThickness = pen.Thickness;

                shape.StrokeDashCap = pen.DashCap;

                if (pen.DashStyle != null)

                {

                    shape.StrokeDashArray = pen.DashStyle.Dashes;

                    shape.StrokeDashOffset = pen.DashStyle.Offset;

                }

                shape.StrokeStartLineCap = pen.StartLineCap;

                shape.StrokeEndLineCap = pen.EndLineCap;

                shape.StrokeLineJoin = pen.LineJoin;

                shape.StrokeMiterLimit = pen.MiterLimit;
            }
        }

        public static void SetPen(Shape shape, Pen pen)
        {
            shape.SetValue(PenProperty, pen);
        }


        public static Pen GetPen(Shape shape)
        {
            return (Pen) shape.GetValue(PenProperty);
        }
    }
}