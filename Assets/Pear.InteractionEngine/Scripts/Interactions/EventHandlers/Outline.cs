using Pear.InteractionEngine.Properties;
using UnityEngine;
using System.Collections.Generic;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Outline objects based on property changes
	/// </summary>
	public class Outline : MonoBehaviour, IGameObjectPropertyEventHandler<bool>
	{
		[Tooltip("Outline material")]
		public Material OutlineMaterial;

		// Maps a property to its OnChange handler
		// We need to instantiate a new material for each property's OnChange handler.
		// Those new materials are easiest to manage when they are in the scope of the OnChange handler.
		// So, since we're dynamically creating OnChange handlers we need to keep track of them so we can
		// unregister them when the time comes
		private Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChangeEventHandler> _propertyHandlers = new Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChangeEventHandler>();

		/// <summary>
		/// Listens for when a property's value changes and applies the appropriate materials
		/// </summary>
		/// <param name="property">property to listen to</param>
		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			Renderer renderer = property.Owner.GetComponent<Renderer>();
			Material originalMaterial = renderer.material;

			Material outlineMaterial = new Material(OutlineMaterial);
			outlineMaterial.color = originalMaterial.color;

			// When the property is true show the outline material
			// Otherwise default to the original material
			property.ChangeEvent += _propertyHandlers[property] = (oldValue, newValue) =>
			{
				if (newValue)
				{
					renderer.materials = new Material[] {
							originalMaterial,
							outlineMaterial
						};
				}
				else
				{
					renderer.materials = new Material[] { originalMaterial };
				}
			};
		}

		/// <summary>
		/// Unregister the property by removing the OnChange listener
		/// </summary>
		/// <param name="property">proeprty to unregister</param>
		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.ChangeEvent -= _propertyHandlers[property];
			_propertyHandlers.Remove(property);
		}
	}
}