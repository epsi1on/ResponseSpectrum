using System;

namespace SeismicResponseSpectrum.Ui
{

    public class PropertyValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }

        public PropertyValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public PropertyValueChangedEventArgs()
        {
        }
    }
}
