using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeismicResponseSpectrum.Ui
{
    [DebuggerDisplay("{X},{Y}")]
    public struct Point2D
    {
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X, Y;
    }
}
