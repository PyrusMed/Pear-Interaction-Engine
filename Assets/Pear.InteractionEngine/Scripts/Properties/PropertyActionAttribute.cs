using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class , AllowMultiple = false, Inherited = false)]
public class PropertyActionAttribute : Attribute {

	public string ActionName
	{
		get;
		set;
	}
}
