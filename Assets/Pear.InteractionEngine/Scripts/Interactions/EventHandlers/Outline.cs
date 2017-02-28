using System;
using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;
using System.Collections.Generic;

namespace Pear.InteractionEngine.Examples
{
	/// <summary>
	/// Outline interactables when they're selected
	/// </summary>
	public class Outline : MonoBehaviour, IGameObjectPropertyEventHandler<bool>
	{
		[Tooltip("Outline material")]
		public Material OutlineMaterial;

		private Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChange> _propertyHandlers = new Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChange>();

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			Renderer renderer = property.Owner.GetComponent<Renderer>();
			Material originalMaterial = renderer.material;

			Material outlineMaterial = new Material(OutlineMaterial);
			outlineMaterial.color = originalMaterial.color;

			property.OnChange += _propertyHandlers[property] = (oldValue, newValue) =>
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

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.OnChange -= _propertyHandlers[property];
			_propertyHandlers.Remove(property);
		}
	}
}