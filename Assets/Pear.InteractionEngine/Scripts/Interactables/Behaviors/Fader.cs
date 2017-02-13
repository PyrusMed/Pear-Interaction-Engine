using UnityEngine;
using System.Collections;

namespace Pear.Core.Interactables.Behaviors
{
    /// <summary>
    /// Fades an object in and out
    /// </summary>
    public class Fader : MonoBehaviour
    {
        // publically editable speed
        public float fadeDelay = 0.0f;
        public float fadeTime = 0.5f;
        public float fadeAplha = 0.3f;

        // store colours
        private Color[] colors;

        // check the alpha value of most opaque object
        float MaxAlpha()
        {
            float maxAlpha = 0.0f;
            Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
            foreach (Renderer item in rendererObjects)
            {
                if (item.enabled)
                    maxAlpha = Mathf.Max(maxAlpha, item.material.color.a);
            }
            return maxAlpha;
        }

        /// <summary>
        /// Main fading function. Fades an object in or out depending on the given time parameter
        /// </summary>
        /// <returns>The sequence.</returns>
        /// <param name="fadingOutTime">Fading out time.</param>
        IEnumerator FadeSequence(float fadingOutTime)
        {
            // log fading direction, then precalculate fading speed as a multiplier
            bool fadingOut = (fadingOutTime < 0.0f);
            float fadingOutSpeed = 1.0f / fadingOutTime;

            // grab all child objects
            Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
            if (colors == null)
            {
                //create a cache of colors if necessary
                colors = new Color[rendererObjects.Length];

                // store the original colours for all child objects
                for (int i = 0; i < rendererObjects.Length; i++)
                {
                    colors[i] = rendererObjects[i].material.color;
                }
            }

            // get current max alpha
            float alphaValue = MaxAlpha();

            // Main fading function
            // Changes alpha value over time
            Shader transparentDiffuseShader = Shader.Find("Transparent/Diffuse");
            while ((alphaValue >= fadeAplha && fadingOut) || (alphaValue <= 1.0f && !fadingOut))
            {
                alphaValue += Time.deltaTime * fadingOutSpeed;

                // Loop over each renderer
                for (int i = 0; i < rendererObjects.Length; i++)
                {
                    // If the renderer is disabled skip it
                    // e.g. colliders
                    if (!rendererObjects[i].enabled)
                        continue;

                    // Set the new color with the updated alpha value
                    Color newColor = (colors != null ? colors[i] : rendererObjects[i].material.color);
                    newColor.a = Mathf.Min(newColor.a, alphaValue);
                    newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                    rendererObjects[i].material.SetColor("_Color", newColor);

                    // Make sure we use the transparent diffuse shader while transitioning
                    rendererObjects[i].material.shader = transparentDiffuseShader;
                }

                yield return null;
            }

            // If this obj was faded in, make sure we set the standard shader so we can't see through the obj
            if (!fadingOut)
            {
                Shader standardShader = Shader.Find("Standard");
                for (int i = 0; i < rendererObjects.Length; i++)
                {
                    rendererObjects[i].material.shader = standardShader;
                }
            }
        }

        /// <summary>
        /// Fades this object in
        /// </summary>
        public void FadeIn()
        {
            FadeIn(fadeTime);
        }

        /// <summary>
        /// Fades this object out
        /// </summary>
        public void FadeOut()
        {
            FadeOut(fadeTime);
        }

        /// <summary>
        /// Internal function for fading in
        /// </summary>
        /// <param name="newFadeTime">New fade time.</param>
        void FadeIn(float newFadeTime)
        {
            StopAllCoroutines();
            StartCoroutine("FadeSequence", newFadeTime);
        }
        /// <summary>
        /// Internal function for fading out
        /// </summary>
        /// <param name="newFadeTime">New fade time.</param>
        void FadeOut(float newFadeTime)
        {
            StopAllCoroutines();
            StartCoroutine("FadeSequence", -newFadeTime);
        }
    }
}