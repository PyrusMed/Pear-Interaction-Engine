using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts from a raycast hit to a gameobject
	/// </summary>
	public class RaycastHitToGameObject : MonoBehaviour, IPropertyConverter<RaycastHit?, GameObject>
	{
		/// <summary>
		/// Converts from a raycast hit to a gameobject
		/// </summary>
		/// <param name="convertFrom">the raycast hit to convert from</param>
		/// <returns>The gameobject from the raycast hit, if any</returns>
		public GameObject Convert(RaycastHit? convertFrom)
		{
			return convertFrom.HasValue ? convertFrom.Value.transform.gameObject : null;
		}
	}
}
