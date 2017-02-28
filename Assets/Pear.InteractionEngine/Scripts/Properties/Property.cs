using System.Collections.Generic;

namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Contains a value and tracks when that value changes
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Property<T>
    {
        public delegate void OnPropertyChange(T oldValue, T newValue);
        public event OnPropertyChange OnChange;

        private T _value = default(T);
        public T Value
        {
            get { return _value; }
            set
            {
                T oldValue = _value;
                _value = value;
                bool notEqual = !EqualityComparer<T>.Default.Equals(oldValue, _value);
                if (notEqual && OnChange != null)
                    OnChange(oldValue, _value);
            }
        }
    }
}