using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.Events
{
	/// <summary>
	/// Update the interactable object's state when we hover over it
	/// </summary>
	public class GazeHover : ControllerBehavior<Controller>, IGameObjectPropertyEvent<bool>
    {
		/// <summary>
		/// The currently hovered over object
		/// </summary>
		public GameObject HoveredObject
        {
            get { return _hoveredHelper != null ? _hoveredHelper.gameObject : null; }
        }

		/// <summary>
		/// The curently hovered over gaze helper
		/// </summary>
		private GazeHoverHelper _hoveredHelper;

        void Update()
        {
			// Send a ray out to see if we're looking at anything
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 1000))
			{
				// If we hit something new
				//	1) Stop gazing at the old object
				//	2) Start gazing at the new object
				if(hitInfo.transform.gameObject != HoveredObject)
				{
					if (_hoveredHelper)
						_hoveredHelper.HoverOnGazeEnd();

					GazeHoverHelper newHovered = hitInfo.transform.gameObject.GetComponent<GazeHoverHelper>();
					if (newHovered)
						newHovered.HoverOnGazeStart();

					_hoveredHelper = newHovered;
				}
			}
			// If we didn't hit anything and we were looking at something previously
			// update the state of the old object
			else if(_hoveredHelper != null)
			{
				_hoveredHelper.GetComponent<GazeHoverHelper>().HoverOnGazeEnd();
				_hoveredHelper = null;
			}
        }

		/// <summary>
		/// Add a hover helper to the property's owner
		/// The hover helper helps us update properties when we're looking at an object
		/// </summary>
		/// <param name="property"></param>
		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			// Get the hover helper, and if it doesn't exist add it
			GazeHoverHelper helper = property.Owner.transform.GetOrAddComponent<GazeHoverHelper>();

			// In Update we call GazeStart and GazeEnd when invoke GazeChanged.
			// It's a easy way to update the appropriate properties when we gaze at an object
			helper.GazeChanged += gazing => property.Value = gazing;
		}

		/// <summary>
		/// Remove our events from the gaze helper
		/// </summary>
		/// <param name="property"></param>
		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			// TODO: Unregister gaze change listener
		}
	}
}