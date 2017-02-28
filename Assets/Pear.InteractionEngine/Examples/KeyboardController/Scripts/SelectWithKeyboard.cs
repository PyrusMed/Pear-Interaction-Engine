using System;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Controllers.Behaviors;
using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Pear.InteractionEngine.Examples
{
	[RequireComponent (typeof(HoverOnGaze))]
	public class SelectWithKeyboard : ControllerBehavior<Controller>, IGameObjectPropertyChanger<bool>
    {
		[Tooltip("Selection key")]
        public KeyCode SelectKey = KeyCode.P;

		public delegate void SelectedEventHandler(GameObject gameObject);
		public event SelectedEventHandler SelectedEvent;

        private HoverOnGaze _hoverOnGaze;

		GameObjectProperty<bool> _lastSelectedProperty;
		private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

        // Use this for initialization
        void Start()
        {
            _hoverOnGaze = GetComponent<HoverOnGaze>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(SelectKey) && _hoverOnGaze.HoveredObject != null)
            {
				GameObjectProperty<bool> selectedProperty = _properties.FirstOrDefault(p => p.Owner == _hoverOnGaze.HoveredObject);
                if(selectedProperty != null)
                {
                    if (selectedProperty.Value)
                    {
                        selectedProperty.Value = false;
                        _lastSelectedProperty = null;
                    }
                    else
                    {
                        selectedProperty.Value = true;

						if (SelectedEvent != null)
							SelectedEvent(selectedProperty.Owner);

                        if (_lastSelectedProperty != null)
                            _lastSelectedProperty.Value = false;

						_lastSelectedProperty = selectedProperty;
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