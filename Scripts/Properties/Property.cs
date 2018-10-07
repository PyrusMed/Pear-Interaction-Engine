using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Base class for properties. Contains a value and notifies listeners when that value changes
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Property<T>
    {
		public const string LOG_TAG = "[Property<T>]";

		// Event that's called when the property changes
        public delegate void OnPropertyChangeEventHandler(T oldValue, T newValue);
        public event OnPropertyChangeEventHandler ValueChangeEvent;

		public bool ShowLogs = false;

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
                if (notEqual)
				{
					if(ShowLogs)
						Debug.Log(String.Format("{0} Property value changed from '{1}' -> '{2}'", LOG_TAG, oldValue, _value));

					if (ValueChangeEvent != null)
					{
						if(ShowLogs)
							Debug.Log(String.Format("{0} Letting listeners know about propety value change", LOG_TAG));

						ValueChangeEvent(oldValue, _value);
					}
					else
					{
						if (ShowLogs)
							Debug.Log(String.Format("{0} There are no listeners listening to this property", LOG_TAG));
					}
				}
            }
        }

		/// <summary>
		/// Determines if two values are equal using the equality comparer
		/// </summary>
		/// <param name="val1">Value to check</param>
		/// <param name="val2">Value to check</param>
		/// <returns>True is values are equal. False otherwise.</returns>
		public static bool AreEqual(T val1, T val2)
		{
			return EqualityComparer<T>.Default.Equals(val1, val2);
		}
    }
}