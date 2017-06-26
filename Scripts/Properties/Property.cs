using System.Collections.Generic;

namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Base class for properties. Contains a value and notifies listeners when that value changes
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Property<T>
    {
		// Event that's called when the property changes
        public delegate void OnPropertyChangeEventHandler(T oldValue, T newValue);
        public event OnPropertyChangeEventHandler ValueChangeEvent;

		/// <summary>
		///  The value of this property
		/// </summary>
        private T _value = default(T);
        public T Value
        {
            get { return _value; }
            set
            {
                T oldValue = _value;
                _value = value;

				// When the value changes fire an event
                bool notEqual = !AreEqual(oldValue, _value);
                if (notEqual && ValueChangeEvent != null)
                    ValueChangeEvent(oldValue, _value);
            }
        }

		public static bool AreEqual(T val1, T val2)
		{
			return EqualityComparer<T>.Default.Equals(val1, val2);
		}
    }
}