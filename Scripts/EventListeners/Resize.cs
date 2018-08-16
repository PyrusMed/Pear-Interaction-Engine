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
        [Tooltip("Resize percentage per second")]
        public float ResizePercentPerSecond = 10f;

		[Tooltip("Should we resize the same amount in each direction?")]
		public bool Uniform = true;

        [Tooltip("If true, this object will remain in it's relative position to the camera while zooming")]
        public bool InPlace;

		// The directions to resize in
		private Vector3 _directions = Vector3.zero;

		/// <summary>
		/// Loops over each registered property and resizes it's owning game object
		/// </summary>
		void Update()
		{
            if(_directions == Vector3.zero)
            {
                return;
            }

			Transform objectToResizeTransform = transform.GetOrAddComponent<ObjectWithAnchor>()
				.AnchorElement
				.transform;

			float currentSize = objectToResizeTransform.localScale.x;
			float maxResizeAmount = (ResizePercentPerSecond * currentSize / 100) * Time.deltaTime;

            // Get the relative position of the camera
            // In case we zoom in place
            Vector3 cameraOriginalLocalOffset = transform.InverseTransformPoint(Camera.main.transform.position);

            objectToResizeTransform.localScale += _directions * maxResizeAmount;

            if (InPlace)
            {
                Vector3 cameraCurrentLocalOffset = transform.InverseTransformPoint(Camera.main.transform.position);
                Vector3 offsetCorrection = transform.TransformPoint(cameraCurrentLocalOffset) - transform.TransformPoint(cameraOriginalLocalOffset);
                objectToResizeTransform.position += offsetCorrection;
            }
		}

		/// <summary>
		/// Saves the even't new value as the direction to resize in
		/// </summary>
		/// <param name="args">event args</param>
		public void ValueChanged(EventArgs<Vector3> args)
		{
			// If we're scalling the amount in each direction use the vector's magnitude
			if (Uniform)
			{
				_directions = Vector3.one * args.NewValue.magnitude;

				// If the direction is negative mult by -1
				if (args.NewValue.x + args.NewValue.y + args.NewValue.z < 0)
					_directions *= -1;
			}
			else
			{
				_directions = args.NewValue;
			}
        }
	}
}
