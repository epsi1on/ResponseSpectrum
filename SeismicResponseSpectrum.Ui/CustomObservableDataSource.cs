using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace SeismicResponseSpectrum.Ui
{
    public class CustomObservableDataSource : ObservableDataSource<Point2D>
    {
        public CustomObservableDataSource(IEnumerable<Point2D> data) : base(data)
        {
        }

        public SeismicRecordSpectrum Record { get; set; }

        public CustomObservableDataSource()
        {
        }
    }
}