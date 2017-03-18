using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Move based on the change in property value
	/// </summary>
	public class Move : MonoBehaviour, IGameObjectPropertyEventHandler<Vector3>
	{
		[Tooltip("Move speed")]
		public float MoveSpeed = 1f;

		[Tooltip("Only move in a single direction based on the meximum direciton of the move vector")]
		public bool SingleDirection = false;

		List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

		/// <summary>
		/// Move each property's owner based on the property's value
		/// </summary>
		void Update()
		{
			_properties.ForEach(p =>
			{
				Vector3 moveValue = p.Value;

				// If we're moving in a single direction get the maximum direction an
				// set the other vector components to 0
				if (SingleDirection)
					moveValue = GetMaximumDirection(moveValue);

				p.Owner.transform.GetOrAddComponent<ObjectWithAnchor>()
					.AnchorElement
					.transform
					.position += moveValue * MoveSpeed * Time.deltaTime;
			});
		}

		public void RegisterProperty(GameObjectProperty<Vector3> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<Vector3> property)
		{
			_properties.Remove(property);
		}

		/// <summary>
		/// Gets a vector with the maximum dimension non-zero and all other dimensions set to 0
		/// </summary>
		/// <param name="moveVector">Vector to update</param>
		/// <returns>a vector with the maximum dimension non-zero and all other dimensions set to 0</returns>
		private Vector3 GetMaximumDirection(Vector3 moveVector)
		{
			Vector3 moveValue = moveVector;
			float absX = Mathf.Abs(moveValue.x);
			float absY = Mathf.Abs(moveValue.y);

			if (absX >= moveValue.y && absX >= moveValue.z)
				moveValue.y = moveValue.z = 0;
			else if (absY >= moveValue.x && absY >= moveValue.z)
				moveValue.x = moveValue.z = 0;
			else
				moveValue.x = moveValue.y = 0;

			return moveValue;
		}
	}
}
