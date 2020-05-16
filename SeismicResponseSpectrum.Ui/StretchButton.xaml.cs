using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Common.Auxiliary;

namespace SeismicResponseSpectrum.Ui
{
	/// <summary>
	/// Interaction logic for StretchButton.xaml
	/// </summary>
	public partial class StretchButton : IPlotterElement
	{
		public StretchButton()
		{
			this.InitializeComponent();
		}

	    public void OnPlotterAttached(Plotter plotter)
	    {
	        this.Plotter = plotter;
	    }

	    public void OnPlotterDetaching(Plotter plotter)
	    {
	        this.Plotter = null;
            
	        //throw new NotImplementedException();
	    }

	    public Plotter Plotter { get; set; }
	}
}