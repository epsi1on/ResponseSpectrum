using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace SeismicResponseSpectrum.Ui
{
    public static class Utils
    {
        public static double[] ChangeResolution(double[] arr, double dt, double desiredDt)
        {
            var n = arr.Length;
            var m = (int)((n - 1) * dt / desiredDt);
            var buf =
                //DoubleArrayPool.GetArray(m);
                new double[m];

            var idt = 1.0 / dt;

            for (double i = 0.0; i < m; i++)
            {
                var t = i * desiredDt;
                var j = (int)(t * idt);
                var d = t - j * dt;
                var v1 = arr[j];
                var v2 = arr[j + 1];
                var v = v1 + (v2 - v1) * d * idt;
                buf[(int)i] = v;
            }

            return buf;
        }


        public static int ChangeResolutionV2(double[] sourceArray, double[] destArray, double dt, double desiredDt)
        {
            var n = sourceArray.Length;
            var m = (int) ((n - 1)*dt/desiredDt);
            var buf = destArray;

            var idt = 1.0/dt;

            for (double i = 0.0; i < m; i++)
            {
                var t = i*desiredDt;
                var j = (int) (t*idt);
                var d = t - j*dt;
                var v1 = sourceArray[j];
                var v2 = sourceArray[j + 1];
                var v = v1 + (v2 - v1)*d*idt;
                buf[(int) i] = v;
            }

            return m;
        }

        /// <summary>
        /// Gets a set of T s that will be used to evaluate the spectrums in tripartite chart
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static double[] GetTripartitePeriods()
        {
            var st = Math.Log10( ApplicationSettings.Current.TripartiteMinFrequency);
            var en = Math.Log10(ApplicationSettings.Current.TripartiteMaxFrequency);

            var d = 0.01;

            var l = (en - st) / d;
            var ts = new List<double>();

            var val = st;

            ts.Add(1 / Math.Pow(10, st));
            while ((val += d) < en)
            {
                ts.Add(1/Math.Pow(10, val));
            }

            ts.Add(1 / Math.Pow(10, en));
            

            return ts.ToArray();
        }





        /// <summary>
        /// Gets a PNG screen shot of the current UIElement
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        public static byte[] GetPngImage(this UIElement source, int dpi)
        {
            double actualHeight = source.RenderSize.Height;
            double actualWidth = source.RenderSize.Width;

            var src = PresentationSource.FromVisual(source);

            double dpiX, dpiY;

            dpiX = 96.0*src.CompositionTarget.TransformToDevice.M11;
            dpiY = 96.0*src.CompositionTarget.TransformToDevice.M22;

            var xScale = dpi / dpiX;
            var yScale = dpi / dpiY;


            var renderHeight = actualHeight * yScale;
            var renderWidth = actualWidth * xScale;

            var renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, dpi, dpi, PixelFormats.Pbgra32);
            var sourceBrush = new VisualBrush(source);

            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                //drawingContext.PushTransform(new ScaleTransform(xScale, yScale));
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }

            renderTarget.Render(drawingVisual);

            var encoder = new PngBitmapEncoder();
            //encoder. = quality;
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));
            /*
            var meta = new BitmapMetadata("jpeg");
            
            meta.ApplicationName = "http://SeismicResponseSpectrum.codeplex.com";
            meta.Keywords =
                new ReadOnlyCollection<string>(new[]
                {"seismic record", "response spectrum", "tripartite", "tripartite sectrum"});
            meta.Title = encoder.Metadata.Subject = "Tripartite spectrum";
            meta.Comment = "Diagram calculated and rendered by Seismic Response Spectrum available @ http://SeismicResponseSpectrum.codeplex.com";
            meta.DateTaken = "UTC " + DateTime.UtcNow.ToString();
            */

            Byte[] imageArray;

            using (MemoryStream outputStream = new MemoryStream())
            {
                encoder.Save(outputStream);
                imageArray = outputStream.ToArray();
            }

            return imageArray;
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }


        public static UIElement CloneElement(UIElement orig)
        {

            if (orig == null)

                return (null);

            string s = XamlWriter.Save(orig);

            StringReader stringReader = new StringReader(s);

            XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());

            return (UIElement)XamlReader.Load(xmlReader);

        }
    }
}
