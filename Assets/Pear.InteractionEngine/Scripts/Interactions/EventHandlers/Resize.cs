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
        [Tooltip("Resize speed")]
        public float ResizeSpeed = 1f;

		// Registered properies
		private List<GameObjectProperty<int>> _properties = new List<GameObjectProperty<int>>();

		/// <summary>
		/// Loops over each registered property and resizes it's owning game object
		/// </summary>
		void Update()
		{
            _properties.ForEach(p =>
            {
                p.Owner.transform.GetOrAddComponent<ObjectWithAnchor>()
                    .AnchorElement
                    .transform
                    .localScale += Vector3.one * p.Value * ResizeSpeed * Time.deltaTime;
            });
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
