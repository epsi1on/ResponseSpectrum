﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Research.DynamicDataDisplay.Charts
{
	public class StackCanvas : Panel
	{
		public StackCanvas()
		{
			ClipToBounds = true;
		}

		#region EndCoordinate attached property

		[AttachedPropertyBrowsableForChildren]
		public static double GetEndCoordinate(DependencyObject obj)
		{
			return (double)obj.GetValue(EndCoordinateProperty);
		}

		public static void SetEndCoordinate(DependencyObject obj, double value)
		{
			obj.SetValue(EndCoordinateProperty, value);
		}

		public static readonly DependencyProperty EndCoordinateProperty = DependencyProperty.RegisterAttached(
			"EndCoordinate",
			typeof(double),
			typeof(StackCanvas),
			new PropertyMetadata(Double.NaN, OnCoordinateChanged));

		#endregion

		#region Coordinate attached property

		[AttachedPropertyBrowsableForChildren]
		public static double GetCoordinate(DependencyObject obj)
		{
			return (double)obj.GetValue(CoordinateProperty);
		}

		public static void SetCoordinate(DependencyObject obj, double value)
		{
			obj.SetValue(CoordinateProperty, value);
		}

		public static readonly DependencyProperty CoordinateProperty = DependencyProperty.RegisterAttached(
			"Coordinate",
			typeof(double),
			typeof(StackCanvas),
			new PropertyMetadata(0.0, OnCoordinateChanged));

		private static void OnCoordinateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement reference = d as UIElement;
			if (reference != null)
			{
				StackCanvas parent = VisualTreeHelper.GetParent(reference) as StackCanvas;
				if (parent != null)
				{
					parent.InvalidateArrange();
				}
			}
		}
		#endregion

		#region AxisPlacement property

		public AxisPlacement Placement
		{
			get { return (AxisPlacement)GetValue(PlacementProperty); }
			set { SetValue(PlacementProperty, value); }
		}

		public static readonly DependencyProperty PlacementProperty =
			DependencyProperty.Register(
			  "Placement",
			  typeof(AxisPlacement),
			  typeof(StackCanvas),
			  new FrameworkPropertyMetadata(
				  AxisPlacement.Bottom,
				  FrameworkPropertyMetadataOptions.AffectsArrange));

		#endregion

		private bool IsHorizontal
		{
			get { return Placement == AxisPlacement.Top || Placement == AxisPlacement.Bottom; }
		}

		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = constraint;
			Size size = new Size();

			bool isHorizontal = IsHorizontal;

			if (isHorizontal)
			{
				availableSize.Width = Double.PositiveInfinity;
				size.Width = constraint.Width;
			}
			else
			{
				availableSize.Height = Double.PositiveInfinity;
				size.Height = constraint.Height;
			}

			// measuring all children and determinimg self width and height
			foreach (UIElement element in base.Children)
			{
				if (element != null)
				{
					element.Measure(availableSize);
					Size desiredSize = element.DesiredSize;

					if (isHorizontal)
					{
						size.Height = Math.Max(size.Height, desiredSize.Height);
					}
					else
					{
						size.Width = Math.Max(size.Width, desiredSize.Width);
					}
				}
			}

			if (Double.IsPositiveInfinity(size.Width)) size.Width = 0;
			if (Double.IsPositiveInfinity(size.Height)) size.Height = 0;

			return size;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			bool isHorizontal = IsHorizontal;

			foreach (UIElement element in base.Children)
			{
				if (element == null)
				{
					continue;
				}

				Size elementSize = element.DesiredSize;
				double x = 0.0;
				double y = 0.0;

				switch (Placement)
				{
					case AxisPlacement.Left:
						x = finalSize.Width - elementSize.Width;
						break;
					case AxisPlacement.Right:
						x = 0;
						break;
					case AxisPlacement.Top:
						y = finalSize.Height - elementSize.Height;
						break;
					case AxisPlacement.Bottom:
						y = 0;
						break;
					default:
						break;
				}

				double coordinate = GetCoordinate(element);

				if (!Double.IsNaN(GetEndCoordinate(element)))
				{
					double endCoordinate = GetEndCoordinate(element);
					double size = endCoordinate - coordinate;
					if (size < 0)
					{
						size = -size;
						coordinate -= size;
					}
					if (isHorizontal)
						elementSize.Width = size;
					else
						elementSize.Height = size;
				}

				if (isHorizontal)
					x = coordinate;
				else
					y = coordinate;

				element.Arrange(new Rect(new Point(x, y), elementSize));
			}
			return finalSize;
		}
	}
}
