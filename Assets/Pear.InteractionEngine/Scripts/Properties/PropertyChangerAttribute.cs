using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PropertyChangerAttribute : Attribute
{
	public string PropertyName
	{
		get;
		private set;
	}

	public PropertyChangerAttribute(string propertyName)
	{
		PropertyName = propertyName;
	}
}
