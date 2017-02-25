using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	public abstract class PropertyAction<T> : MonoBehaviour, IPropertyAction where T : class
	{
		public abstract string ActionName { get; }

		public Type PropertyType
		{
			get { return typeof(T); }
		}
	}
}