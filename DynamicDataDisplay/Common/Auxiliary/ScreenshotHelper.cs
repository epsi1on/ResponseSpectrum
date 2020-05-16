﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;

namespace Microsoft.Research.DynamicDataDisplay.Common.Auxiliary
{
	internal static class ScreenshotHelper
	{
		/// <summary>Gets the encoder by extension</summary>
		/// <param name="extension">The extension</param>
		/// <returns>BitmapEncoder object</returns>
		internal static BitmapEncoder GetEncoderByExtension(string extension)
		{
			switch (extension)
			{
				case "bmp":
					return new BmpBitmapEncoder();
				case "jpg":
					return new JpegBitmapEncoder();
				case "gif":
					return new GifBitmapEncoder();
				case "png":
					return new PngBitmapEncoder();
				case "tiff":
					return new TiffBitmapEncoder();
				case "wmp":
					return new WmpBitmapEncoder();
				default:
					throw new ArgumentException(Properties.Resources.CannotDetermineImageTypeByExtension, "extension");
			}
		}

		/// <summary>Creates the screenshot of entire plotter element</summary>
		/// <returns></returns>
		internal static BitmapSource CreateScreenshot(UIElement uiElement, Int32Rect screenshotSource)
		{
			Window window = Window.GetWindow(uiElement);
			if (window == null)
			{
				return CreateElementScreenshot(uiElement);
			}
			Size size = window.RenderSize;

			//double dpiCoeff = 32 / SystemParameters.CursorWidth;
			//int dpi = (int)(dpiCoeff * 96);
			double dpiCoeff = 1;
			int dpi = 96;

			RenderTargetBitmap bmp = new RenderTargetBitmap(
				(int)(size.Width * dpiCoeff), (int)(size.Height * dpiCoeff),
				dpi, dpi, PixelFormats.Default);

			bmp.Render(uiElement);

			CroppedBitmap croppedBmp = new CroppedBitmap(bmp, screenshotSource);
			return croppedBmp;
		}

		private static BitmapSource CreateElementScreenshot(UIElement uiElement)
		{
			bool measureValid = uiElement.IsMeasureValid;

			if (!measureValid)
			{
				double width = 300;
				double height = 300;

				FrameworkElement frElement = uiElement as FrameworkElement;
				if (frElement != null)
				{
					if (!Double.IsNaN(frElement.Width))
						width = frElement.Width;
					if (!Double.IsNaN(frElement.Height))
						height = frElement.Height;
				}

				Size size = new Size(width, height);
				uiElement.Measure(size);
				uiElement.Arrange(new Rect(size));
			}

			RenderTargetBitmap bmp = new RenderTargetBitmap(
				(int)uiElement.RenderSize.Width, (int)uiElement.RenderSize.Height,
				96, 96, PixelFormats.Default);

			// this is waiting for dispatcher to perform measure, arrange and render passes
			uiElement.Dispatcher.Invoke(((Action)(() => { })), DispatcherPriority.Background);

			bmp.Render(uiElement);

			return bmp;
		}

		private static Dictionary<BitmapSource, string> pendingBitmaps = new Dictionary<BitmapSource, string>();

		internal static void SaveBitmapToStream(BitmapSource bitmap, Stream stream, string fileExtension)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (String.IsNullOrEmpty(fileExtension))
				throw new ArgumentException(Properties.Resources.ExtensionCannotBeNullOrEmpty, fileExtension);

			BitmapEncoder encoder = ScreenshotHelper.GetEncoderByExtension(fileExtension);
			encoder.Frames.Add(BitmapFrame.Create(bitmap));
			encoder.Save(stream);
		}

		internal static void SaveBitmapToFile(BitmapSource bitmap, string filePath)
		{
			if (String.IsNullOrEmpty(filePath))
				throw new ArgumentException(Properties.Resources.FilePathCannotbeNullOrEmpty, "filePath");

			if (bitmap.IsDownloading)
			{
				pendingBitmaps[bitmap] = filePath;
				bitmap.DownloadCompleted += OnBitmapDownloadCompleted;
				return;
			}

			string extension = System.IO.Path.GetExtension(filePath).TrimStart('.');

			string dirPath = Path.GetDirectoryName(filePath);
			if (!String.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}

			try
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
				{
					SaveBitmapToStream(bitmap, fs, extension);
				}
			}
			catch (IOException exc)
			{
				Debug.WriteLine("Exception while saving bitmap to file: " + exc.Message);
			}
		}

		private static void OnBitmapDownloadCompleted(object sender, EventArgs e)
		{
			BitmapSource bmp = (BitmapSource)sender;
			bmp.DownloadCompleted -= OnBitmapDownloadCompleted;
			string filePath = pendingBitmaps[bmp];
			pendingBitmaps.Remove(bmp);

			SaveBitmapToFile(bmp, filePath);
		}
	}
}
