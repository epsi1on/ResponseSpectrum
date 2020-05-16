﻿using System;
using System.Windows;

namespace Microsoft.Research.DynamicDataDisplay.DataSources
{
	public interface IPointEnumerator : IDisposable {
        /// <summary>Move to next point in sequence</summary>
        /// <returns>True if successfully moved to next point 
        /// or false if end of sequence is reached</returns>
		bool MoveNext();

        /// <summary>Stores current value(s) in given point.</summary>
        /// <param name="p">Reference to store value</param>
        /// <remarks>Depending on implementing class this method may set only X or Y
        /// fiels in specified point. That's why GetCurrent is a regular method and
        /// not a property as in stardard enumerators</remarks>
		void GetCurrent(ref Point p);

		void ApplyMappings(DependencyObject target);
	}
}
