using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Rotate a game object based on the value of a vector 2 property
	/// </summary>
	public class RotateVector2 : MonoBehaviour, IGameObjectPropertyEventHandler<Vector2>
	{
		[Tooltip("Rotation speed")]
		public float RotateSpeed = 10f;

		// Registered properies
		private List<GameObjectProperty<Vector2>> _properties = new List<GameObjectProperty<Vector2>>();

		/// <summary>
		/// Loops over each registered property and rotates it's owning game object
		/// </summary>
		void Update()
		{
			_properties.ForEach(p =>
			{
				p.Owner.transform.GetOrAddComponent<ObjectWithAnchor>()
					.AnchorElement
					.transform
					.Rotate(new Vector3(p.Value.y, -p.Value.x, 0) * RotateSpeed * Time.deltaTime, Space.World);
			});
		}

		/// <summary>
		/// Save the property
		/// </summary>
		/// <param name="property"></param>
		public void RegisterProperty(GameObjectProperty<Vector2> property)
		{
			_properties.Add(property);
		}

		/// <summary>
		/// Remove the property from our saved list
		/// </summary>
		/// <param name="property"></param>
		public void UnregisterProperty(GameObjectProperty<Vector2> property)
		{
			_properties.Remove(property);
		}
	}
}
