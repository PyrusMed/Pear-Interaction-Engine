using HoloToolkit.Unity.InputModule;
using Pear.Core.Interactables;
using UnityEngine.VR.WSA.Input;

namespace Pear.Core.Controllers.Behaviors
{
    /// <summary>
    /// Selects an interactable object on tap
    /// </summary>
    public class TapToSelect : ControllerBehavior<HoloLensController>
    {
        // Used to recognize tap
        GestureRecognizer _recognizer = new GestureRecognizer();

        private void Start()
        {
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
                            interactable.Selected.Add(Controller);
                        // Otherwise, if we have already selected it, deselect it
                        else
                            interactable.Selected.Remove(Controller);
                    }
                }
            };
        }
    }
}