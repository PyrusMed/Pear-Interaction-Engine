using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Rotate a game object based on the value of a property
	/// </summary>
	public class Rotate : MonoBehaviour, IGameObjectPropertyEventHandler<Vector3>
	{
        [Tooltip("Rotation speed")]
        public float RotateSpeed = 10f;

        // Registered properies
        private List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

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
		public void RegisterProperty(GameObjectProperty<Vector3> property)
		{
			_properties.Add(property);
		}

		/// <summary>
		/// Remove the property from our saved list
		/// </summary>
		/// <param name="property"></param>
		public void UnregisterProperty(GameObjectProperty<Vector3> property)
		{
			_properties.Remove(property);
		}
	}
}
