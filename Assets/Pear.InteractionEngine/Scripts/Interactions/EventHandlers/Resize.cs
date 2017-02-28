using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Resize a game object based on the value of a property
	/// </summary>
	public class Resize : MonoBehaviour, IGameObjectPropertyEventHandler<int>
	{
		// Registered properies
		private List<GameObjectProperty<int>> _properties = new List<GameObjectProperty<int>>();

		/// <summary>
		/// Loops over each registered property and resizes it's owning game object
		/// </summary>
		void Update()
		{
			foreach(GameObjectProperty<int> property in _properties)
			{
				// Better to resize the anchor element than the original object since there could be other
				// event handlers applying manipulations
				Anchor anchor = property.Owner.transform.GetOrAddComponent<ObjectWithAnchor>().AnchorElement;
				float currentScale = anchor.transform.localScale.x;
				float scaleAmount = property.Value * Time.deltaTime;
				float newScale = currentScale * (1 + scaleAmount);

				// Apply the new scale
				anchor.transform.localScale = Vector3.one * newScale;
			}
		}

		/// <summary>
		/// Save the property
		/// </summary>
		/// <param name="property"></param>
		public void RegisterProperty(GameObjectProperty<int> property)
		{
			_properties.Add(property);
		}

		/// <summary>
		/// Remove the property from our saved list
		/// </summary>
		/// <param name="property"></param>
		public void UnregisterProperty(GameObjectProperty<int> property)
		{
			_properties.Remove(property);
		}
	}
}
