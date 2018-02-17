using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.EventListeners
{
	public class EmissionHighlight : MonoBehaviour, IEventListener<bool>
	{
		private const string LOG_TAG = "[EmissionHighlight]";

		[Tooltip("The emmission strength")]
		[Range(1f, 100f)]
		public float EmissionStrength = 1f;

		[Tooltip("Pulses per second")]
		[Range(1f, 100f)]
		public float PulsesPerSecond = 1;

		// Key for pulse routine
		private Coroutine _pulseRoutine;

		// If true the highlight will show
		// Will not show otherwise
		public bool Highlight
		{
			get { return _pulseRoutine != null; }
			set
			{
				if(value != Highlight)
				{
					if (value)
						_pulseRoutine = StartCoroutine(Pulse());
					else
						EndPulse();
				}
			}
		}

		public void ValueChanged(EventArgs<bool> args)
		{
			Highlight = args.NewValue;
		}

		private IEnumerator Pulse()
		{
			//Debug.Log(String.Format("{0} starting pulse on {1}", LOG_TAG, name));

			float timeElapsed = 0;
			while (true)
			{
				// Pulse each child
				foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
				{
					foreach(Material material in renderer.materials)
					{
						if (!material.HasProperty("_EmissionColor"))
							continue;

						material.EnableKeyword("_EMISSION");

						float emission = Mathf.PingPong(timeElapsed, 1 / PulsesPerSecond) * EmissionStrength;
						Color baseColor = material.color;
						Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
						material.SetColor("_EmissionColor", finalColor);
					}
				}

				timeElapsed += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}

		private void EndPulse()
		{
			//Debug.Log(String.Format("{0} ending pulse on {1}", LOG_TAG, name));

			if (_pulseRoutine != null)
				StopCoroutine(_pulseRoutine);

			_pulseRoutine = null;

			// Add the outline material to each renderer
			foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
			{
				foreach (Material material in renderer.materials)
				{
					if (!material.HasProperty("_EmissionColor"))
						continue;

					material.SetColor("_EmissionColor", material.color);
					material.DisableKeyword("_EMISSION");
				}
			}
		}
	}
}
