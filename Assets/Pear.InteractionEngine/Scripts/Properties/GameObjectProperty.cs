using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.InteractionEngine.Properties
{
    public class GameObjectProperty<T> : MonoBehaviour
    {
        public string Name;

		[SerializeField]
		private T DefaultVal;
		
        public T Value
		{
			get { return _property.Value; }
			set { _property.Value = value; }
		}

        public class GameObjectPropertyEvent : UnityEvent<T, T> { }
        public GameObjectPropertyEvent OnChange = new GameObjectPropertyEvent();

        private Property<T> _property;

        void Awake()
        {
            _property = new Property<T>(Name);
            _property.Value = Value;

            // Hook up the on change event
            _property.OnChange += (oldVal, newVal) =>
            {
                if (OnChange != null)
                    OnChange.Invoke(oldVal, newVal);
            };
        }

        void Start()
        {
            GameObjectPropertyManager<T>.Get(Name).Add(this);
        }

        void OnDestroy()
        {
            GameObjectPropertyManager<T>.Get(Name).Remove(this);
        }
    }
}