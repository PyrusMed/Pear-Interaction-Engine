using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	public class InteractableObjectSounds : MonoBehaviour
	{
		public AudioClip StartHovering;
		public AudioClip StopHovering;
		public AudioClip StartMoving;
		public AudioClip StopMoving;
		public AudioClip StartResizing;
		public AudioClip StopResizing;
		public AudioClip Selected;

        public string HoverPropertyName = "pie.hover";
        public string MovePropertyName = "pie.move";
        public string ResizePropertyName = "pie.resize";
        public string SelectPropertyName = "pie.select";

		void Awake()
		{
			// When a new property is added hook up listeners
            PlaySoundOnChange(HoverPropertyName, StartHovering, StopHovering);
            PlaySoundOnChange(MovePropertyName, StartMoving, StopMoving);
            PlaySoundOnChange(ResizePropertyName, StartResizing, StopResizing);
            PlaySoundOnChange(SelectPropertyName, Selected, null);
        }

		/// <summary>
		/// Play sounds when the specified state object changes
		/// </summary>
		/// <param name="state">State to link events to</param>
		/// <param name="startSound">Sound to play when state starts</param>
		/// <param name="endSound">Sound to play when state ends</param>
		private void PlaySoundOnChange(string propertyName, AudioClip startSound, AudioClip endSound)
		{
            AudioSource startSource = CreateAudioSource(startSound);
            AudioSource endSource = CreateAudioSource(endSound);

            GameObjectPropertyCollection<bool> propertyCollection = GameObjectPropertyManager<bool>.Get(propertyName);
            propertyCollection.OnAdded += (property) =>
            {
                property.OnChange.AddListener((oldValue, newValue) =>
                {
                    if(newValue)
                    {
                        if (startSource)
                            startSource.Play();
                    }
                    else
                    {
                        if (endSource)
                            endSource.Play();
                    }

                });
            };
		}

		/// <summary>
		/// Create an audio source from the specified audio clip
		/// </summary>
		/// <param name="clip">Clip to create source from</param>
		/// <returns>Audio source</returns>
		private AudioSource CreateAudioSource(AudioClip clip)
		{
            if (clip == null)
                return null;

			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = clip;
			source.playOnAwake = false;
			return source;
		}
	}
}
