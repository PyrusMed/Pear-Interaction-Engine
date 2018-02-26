using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a collider to a game object
	/// </summary>
	public class ColliderToGameObject : MonoBehaviour, IPropertyConverter<Collider, GameObject>
	{
		/// <summary>
		/// Converts from a collider to a gameobject
		/// </summary>
		/// <param name="convertFrom">collider to convert from</param>
		/// <returns>Game object from collider</returns>
		public GameObject Convert(Collider convertFrom)
		{
			return convertFrom != null ? convertFrom.gameObject : null;
		}
	}
}
