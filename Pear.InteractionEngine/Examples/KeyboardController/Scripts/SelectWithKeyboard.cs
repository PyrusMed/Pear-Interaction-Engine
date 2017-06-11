using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Interactions.Events;
using Pear.InteractionEngine.Properties;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Pear.InteractionEngine.Examples
{
	[RequireComponent (typeof(GazeHover))]
	public class SelectWithKeyboard : ControllerBehavior<Controller>, IGameObjectPropertyEvent<bool>
    {
		[Tooltip("Selection key")]
        public KeyCode SelectKey = KeyCode.P;

		public delegate void SelectedEventHandler(GameObject gameObject);
		public event SelectedEventHandler SelectedEvent;

        private GazeHover _hoverOnGaze;

		GameObject _lastSelectedObj;
		private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

        // Use this for initialization
        void Start()
        {
            _hoverOnGaze = GetComponent<GazeHover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(SelectKey) && _hoverOnGaze.HoveredObject != null)
            {
				List<GameObjectProperty<bool>> selectedProperties = _properties.Where(p => p.Owner == _hoverOnGaze.HoveredObject).ToList();
				if (selectedProperties.Count > 0)
				{
					GameObjectProperty<bool> representativeProp = selectedProperties.First();
					if (representativeProp.Value)
					{
						selectedProperties.ForEach(p => p.Value = false);
						_lastSelectedObj = null;
					}
					else
					{
						selectedProperties.ForEach(p => p.Value = true);
						if (SelectedEvent != null)
							SelectedEvent(representativeProp.Owner);

						if(_lastSelectedObj != null)
							_properties.Where(p => p.Owner == _lastSelectedObj).ToList().ForEach(p => p.Value = false);

						_lastSelectedObj = representativeProp.Owner;
					}
				}
			}
        }

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			_properties.Remove(property);
		}
	}
}