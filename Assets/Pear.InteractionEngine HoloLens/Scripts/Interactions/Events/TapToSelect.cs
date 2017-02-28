using HoloToolkit.Unity.InputModule;
using Pear.InteractionEngine.Properties;
using UnityEngine.VR.WSA.Input;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Pear.InteractionEngine.Controllers;

namespace Pear.InteractionEngine.Interactions.Events
{
    /// <summary>
    /// Selects an interactable object on tap
    /// </summary>
    public class TapToSelect : ControllerBehavior<HoloLensController>, IGameObjectPropertyEvent<bool>
    {
        public delegate void SelectedEventHandler(GameObject gameObject);
        public event SelectedEventHandler SelectedEvent;

        // Used to recognize tap
        GestureRecognizer _recognizer;

        GameObject _lastSelectedObj;
        private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

        private void Start()
        {
            _recognizer = new GestureRecognizer();
            _recognizer.TappedEvent += (source, tapCount, headRay) =>
            {
                // If we're gazing at an interactable object, select it
                if (GazeManager.Instance.IsGazingAtObject)
                {
                    List<GameObjectProperty<bool>> selectedProperties = _properties
                        .Where(p => p.Owner == GazeManager.Instance.HitObject)
                        .ToList();
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

                            if (_lastSelectedObj != null)
                                _properties.Where(p => p.Owner == _lastSelectedObj).ToList().ForEach(p => p.Value = false);

                            _lastSelectedObj = representativeProp.Owner;
                        }
                    }
                }
                else
                {
                    if (SelectedEvent != null)
                        SelectedEvent(null);
                }
            };

            _recognizer.StartCapturingGestures();
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