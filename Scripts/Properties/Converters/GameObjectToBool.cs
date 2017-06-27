using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class GameObjectToBool : MonoBehaviour, IPropertyConverter<GameObject, bool>
	{
		public bool Convert(GameObject convertFrom)
		{
			return convertFrom == gameObject;
		}
	}
}
