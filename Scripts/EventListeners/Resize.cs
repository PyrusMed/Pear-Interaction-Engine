using Pear.InteractionEngine.Utils;
using UnityEngine;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Interactions;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Resize a game object based on change in event
	/// </summary>
	public class Resize : MonoBehaviour, IEventListener<Vector3>
	{
        [Tooltip("Resize speed")]
        public float ResizeSpeed = 1f;

		// The directions to resize in
		private Vector3 _directions;

		/// <summary>
		/// Loops over each registered property and resizes it's owning game object
		/// </summary>
		void Update()
		{
			transform.GetOrAddComponent<ObjectWithAnchor>()
				.AnchorElement
				.transform
				.localScale += _directions * ResizeSpeed * Time.deltaTime;
		}

		/// <summary>
		/// Saves the even't new value as the direction to resize in
		/// </summary>
		/// <param name="args">event args</param>
		public void ValueChanged(EventArgs<Vector3> args)
		{
			_directions = args.NewValue;
		}
	}
}
