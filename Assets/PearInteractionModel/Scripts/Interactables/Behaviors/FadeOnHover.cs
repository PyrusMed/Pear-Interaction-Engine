using PearMed.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PearMed.Interactables.Behaviors
{
    /// <summary>
    /// Manages what happens when object are hovered over
    /// </summary>
    public class FadeOnHover : Singleton<FadeOnHover>
    {
        [Tooltip("Seconds between when the controller hovers over the object and when fading starts")]
        public float FadeDelay = 0.0f;

        [Tooltip("Seconds is takes for the fade to complete")]
        public float FadeTime = 0.5f;

        [Tooltip("Opacity that objects fade to")]
        public float FadeAlpha = 0.3f;

        private void Awake()
        {
            InteractableObjectManager.Instance.OnAdded.AddListener((interactable) => SetOpacityOnHover(interactable));
            InteractableObjectManager.Instance.OnRemoved.AddListener((interactable) => StopListeningForHover(interactable));
        }

        /// <summary>
        /// Listen for when a controller hovers over the given object and fade when it happens
        /// </summary>
        /// <param name="interactable">object to fade on hover</param>
        public void SetOpacityOnHover(InteractableObject interactable)
        {
            interactable.Hovering.OnStart.AddListener(HandleFade);
            interactable.Hovering.OnEnd.AddListener(HandleFade);

            Fader fader = interactable.gameObject.AddComponent<Fader>();
            fader.fadeDelay = FadeDelay;
            fader.fadeTime = FadeTime;
            fader.fadeAplha = FadeAlpha;
        }

        /// <summary>
        /// Stop listening for hover events
        /// </summary>
        /// <param name="interactable">object to remove events from</param>
        public void StopListeningForHover(InteractableObject interactable)
        {
            interactable.Hovering.OnStart.RemoveListener(HandleFade);
            interactable.Hovering.OnEnd.RemoveListener(HandleFade);
        }

        /// <summary>
        /// Event handler for hover events
        /// </summary>
        /// <param name="e"></param>
        private static void HandleFade(InteractableObjectControllerEventData e)
        {
            FadeAll();
        }

        /// <summary>
        /// Fade all objects
        /// If at least one object is hovered over, fade the rest out
        /// If no object is hovered over, fade them all in
        /// </summary>
        private static void FadeAll()
        {
            IEnumerable<InteractableObject> allObjects = InteractableObjectManager.Instance.AllObjects;

            // If at least one object is hovered over,
            //	Fade in all hovered objects
            //	Fade out all non-hovered objects
            if (allObjects.Any((bo) => bo.Hovering.IsTrue()))
            {
                foreach (InteractableObject bo in allObjects)
                {
                    Fader fader = bo.GetComponent<Fader>();
                    if (bo.Hovering.IsTrue())
                        fader.FadeIn();
                    else
                        fader.FadeOut();
                }
            }
            // Otherwise, fade in all objects
            else
            {
                foreach (InteractableObject bo in allObjects)
                {
                    Fader fader = bo.GetComponent<Fader>();
                    fader.FadeIn();
                }
            }
        }
    }
}