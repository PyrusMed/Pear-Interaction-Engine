using Pear.InteractionEngine.Utils;
using UnityEngine;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Interactions;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Move based on the change in event
	/// </summary>
	public class Move : MonoBehaviour, IEventListener<Vector3>
	{
		[Tooltip("Move speed")]
		public float MoveSpeed = 1f;

		[Tooltip("Only move in a single direction based on the meximum direciton of the move vector")]
		public bool SingleDirection = false;

		[Tooltip("True if the direction is relative to the source. World axis used otherwise.")]
		public bool RelativeToSource = true;

		// Movement velocity
		private Vector3 _velocity = Vector3.zero;

		// Anchor to move
		private Anchor _anchor;

		private void Start()
		{
			_anchor = transform.GetOrAddComponent<ObjectWithAnchor>().AnchorElement;
		}

		/// <summary>
		/// Move this object based on the velocity
		/// </summary>
		private void Update()
		{
			_anchor.transform.position += _velocity * MoveSpeed * Time.deltaTime;
		}

		/// <summary>
		/// Move the object based on the supplied vector
		/// </summary>
		/// <param name="args">Event args</param>
		public void ValueChanged(EventArgs<Vector3> args)
		{
			_velocity = SingleDirection ? GetMaximumDirection(args.NewValue) : args.NewValue;
			_velocity = RelativeToSource ? args.Source.transform.TransformVector(_velocity) : _velocity;
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
