using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace SeismicResponseSpectrum.Ui
{
    /// <summary>
    /// Represent a class who find best position for annotation rectangle near by target area
    /// </summary>
    public class AnnotateRectanglePositionFinder
    {
        public Rect TargetArea;
        public double AnnotationHeight;
        public double AnnotationWidth;
        public double DesiredDistance;

        public Rect AvailableArea;

        /// <summary>
        /// Gets the length of line inside to <see cref="r"/>.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="alfa">The Alfa in degrees.</param>
        /// <returns></returns>
        private double GetGama(double w,double h, double alfa)
        {
            var arad = (alfa) * Math.PI / 180;

            var tgAlfa = Math.Abs(Math.Tan(arad));

            if (tgAlfa < h / w)
            {
                return w / 2 * Math.Sqrt(1 + tgAlfa * tgAlfa);
            }
            else
            {
                var cotg = Math.Cos(arad) / Math.Sin(arad);
                return h / 2 * Math.Sqrt(1 + cotg * cotg);
            }
        }

        private bool NEquals(double v1, double v2)
        {
            return Math.Abs(v1 - v2) < 1;
        }

        public Rect GetBestPlacementForAnnotationBox()
        {
            var alfas = new List<double>() {0, 180, 90, 270};
            alfas.AddRange(Enumerable.Range(0, 360).Select(i => (double) i));

            var c = new Point(TargetArea.X + TargetArea.Width/2, TargetArea.Y + TargetArea.Height/2);


            foreach (var alfa in alfas)
            {
                var r = GetGama(AnnotationWidth, AnnotationHeight, alfa) +
                        GetGama(TargetArea.Width, TargetArea.Height, alfa) +
                        DesiredDistance;


                var x = Math.Cos(alfa * Math.PI / 180) * r;
                var y = Math.Sin(alfa * Math.PI / 180) * r;

                var v = new Vector(x, y);

                var p = c + v - 0.5*new Vector(AnnotationWidth/2, AnnotationHeight/2);

                if (IsOkPlacementForAnnotation(p))
                    return new Rect(p.X, p.Y, AnnotationWidth, AnnotationHeight);
            }

            return new Rect(AvailableArea.Width/2 + AvailableArea.X, AvailableArea.Height/2 + AvailableArea.Y,
                AnnotationWidth, AnnotationHeight);
        }

        private bool IsOkPlacementForAnnotation(Point pt)
        {
            var annotationRect = new Rect(pt, new Vector(AnnotationWidth, AnnotationHeight));

            if (!AvailableArea.Contains(annotationRect))
                return false;

            if (TargetArea.IntersectsWith(annotationRect))
                return false;


            return true;
        }

       
    }
}
