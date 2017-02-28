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
		public GameObject HoveredObject
        {
            get { return _lastHovered != null ? _lastHovered.gameObject : null; }
        }

		private GazeHoverHelper _lastHovered;

        void Update()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 1000))
			{
				if(hitInfo.transform.gameObject != HoveredObject)
				{
					if (_lastHovered)
						_lastHovered.HoverOnGazeEnd();

					GazeHoverHelper newHovered = hitInfo.transform.gameObject.GetComponent<GazeHoverHelper>();
					if (newHovered)
						newHovered.HoverOnGazeStart();

					_lastHovered = newHovered;
				}
			}
			else if(_lastHovered != null)
			{
				_lastHovered.GetComponent<GazeHoverHelper>().HoverOnGazeEnd();
				_lastHovered = null;
			}
        }

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			GazeHoverHelper helper = property.Owner.transform.GetOrAddComponent<GazeHoverHelper>();
			helper.GazeStartEvent += () =>
			{
				property.Value = true;
			};

			helper.GazeEndEvent += () =>
			{
				property.Value = false;
			};
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			Destroy(property.Owner.GetComponent<GazeHoverHelper>());
		}
	}
}