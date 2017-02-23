using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Interactables.Behaviors
{
    /// <summary>
    /// Manages what happens when object are hovered over
    /// </summary>
    public class FadeOnHover : MonoBehaviour
    {
        [Tooltip("Name of the GameObjectProperty to listen for")]
        public string HoverPropertyName = "pie.hover";

        [Tooltip("Seconds between when the controller hovers over the object and when fading starts")]
        public float FadeDelay = 0.0f;

        [Tooltip("Seconds is takes for the fade to complete")]
        public float FadeTime = 0.5f;

        [Tooltip("Opacity that objects fade to")]
        public float FadeAlpha = 0.3f;

        private GameObjectPropertyCollection<bool> _hoverPropertyCollection;

        private void Awake()
        {
            _hoverPropertyCollection = GameObjectPropertyManager<bool>.Get(HoverPropertyName);
            _hoverPropertyCollection.OnAdded += SetOpacityOnHover;
            _hoverPropertyCollection.OnRemoved += StopListeningForHover;
        }

        /// <summary>
        /// Listen for when a controller hovers over the given object and fade when it happens
        /// </summary>
        /// <param name="interactable">object to fade on hover</param>
        public void SetOpacityOnHover(GameObjectProperty<bool> hoverProperty)
        {
            hoverProperty.OnChange.AddListener(HandleFade);

            Fader fader = hoverProperty.gameObject.AddComponent<Fader>();
            fader.fadeDelay = FadeDelay;
            fader.fadeTime = FadeTime;
            fader.fadeAplha = FadeAlpha;
        }

        /// <summary>
        /// Stop listening for hover events
        /// </summary>
        /// <param name="interactable">object to remove events from</param>
        public void StopListeningForHover(GameObjectProperty<bool> hoverProperty)
        {
            hoverProperty.OnChange.RemoveListener(HandleFade);
        }

        /// <summary>
        /// Event handler for hover events
        /// </summary>
        /// <param name="e"></param>
        private void HandleFade(bool oldHoverValue, bool newHoverValue)
        {
            FadeAll();
        }

        /// <summary>
        /// Fade all objects
        /// If at least one object is hovered over, fade the rest out
        /// If no object is hovered over, fade them all in
        /// </summary>
        private void FadeAll()
        {
            GameObjectProperty<bool>[] hoverProperties = _hoverPropertyCollection.All;

            // If at least one object is hovered over,
            //	Fade in all hovered objects
            //	Fade out all non-hovered objects
            if (hoverProperties.Any((prop) => prop.Value))
            {
                foreach (GameObjectProperty<bool> property in hoverProperties)
                {
                    Fader fader = property.GetComponent<Fader>();
                    if (property.Value)
                        fader.FadeIn();
                    else
                        fader.FadeOut();
                }
            }
            // Otherwise, fade in all objects
            else
            {
                foreach (GameObjectProperty<bool> property in hoverProperties)
                {
                    Fader fader = property.GetComponent<Fader>();
                    fader.FadeIn();
                }
            }
        }
    }
}