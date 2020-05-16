using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SeismicResponseSpectrum.Ui;

namespace SeismicResponseSpectrum.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new AccelerogramFileReader();
            reader.FileContent = System.IO.File.ReadAllText(@"C:\temp\manjil.txt");
            reader.TimeStepDt = 0.02;
            reader.FirstLine = 5;
            reader.LastLine = 540;

            var rec = reader.ToSeismicRecordSpectrum();

            var sw = Stopwatch.StartNew();
            rec.ComputeResponseSpectrums();
            sw.Stop();
            System.Console.WriteLine("Calculating spectrums took {0} ms", sw.ElapsedMilliseconds);

            System.Console.ReadKey();
        }
    }
}
