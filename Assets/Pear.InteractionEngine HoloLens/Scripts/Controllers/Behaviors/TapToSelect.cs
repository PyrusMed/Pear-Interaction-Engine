using HoloToolkit.Unity.InputModule;
using Pear.InteractionEngine.Interactables;
using UnityEngine.VR.WSA.Input;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
    /// <summary>
    /// Selects an interactable object on tap
    /// </summary>
    public class TapToSelect : ControllerBehavior<HoloLensController>
    {
        // Used to recognize tap
        GestureRecognizer _recognizer;

        // The last selected obj
        InteractableObject _lastSelected;

        private void Start()
        {
            _recognizer = new GestureRecognizer();
            _recognizer.TappedEvent += (source, tapCount, headRay) =>
            {
                // If we're gazing at an interactable object, select it
                if (GazeManager.Instance.IsGazingAtObject)
                {
                    InteractableObject interactable = GazeManager.Instance.HitObject.GetComponent<InteractableObject>();
                    if (interactable != null)
                    {
                        // If this controller has not selected this object, select it
                        if (!interactable.Selected.Contains(Controller))
                        {
                            // Deselect the last selected obj
                            if (_lastSelected != null)
                                _lastSelected.Selected.Remove(Controller);

                            interactable.Selected.Add(Controller);
                            _lastSelected = interactable;
                        }
                        // Otherwise, if we have already selected it, deselect it
                        else
                        {
                            interactable.Selected.Remove(Controller);
                            _lastSelected = null;
                        }
                    }
                }
            };

            _recognizer.StartCapturingGestures();
        }
    }
}