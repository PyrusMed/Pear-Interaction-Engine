using System;
using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
    /// <summary>
    /// Update the interactable object's state when we hover over it
    /// </summary>
    public class HoverOnGaze : ControllerBehavior<Controller>, IPropertyChanger<bool>
    {
		public GameObject HoveredObject
        {
            get { return _lastHovered != null ? _lastHovered.gameObject : null; }
        }

		private HoverOnGazeHelper _lastHovered;

        void Update()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 1000))
			{
				if(hitInfo.transform.gameObject != HoveredObject)
				{
					if (_lastHovered)
						_lastHovered.HoverOnGazeEnd();

					HoverOnGazeHelper newHovered = hitInfo.transform.gameObject.GetComponent<HoverOnGazeHelper>();
					if (newHovered)
						newHovered.HoverOnGazeStart();

					_lastHovered = newHovered;
				}

				
			}
        }

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			property.gameObject.AddComponent<HoverOnGazeHelper>().GazeStartEvent += () =>
			{
				property.Value = true;
			};

			property.gameObject.AddComponent<HoverOnGazeHelper>().GazeEndEvent += () =>
			{
				property.Value = false;
			};
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
		}
	}
}