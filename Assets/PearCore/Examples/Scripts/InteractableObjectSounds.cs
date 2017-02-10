using Pear.Core.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectSounds : MonoBehaviour {

    public AudioClip StartHovering;
    public AudioClip StopHovering;
    public AudioClip StartMoving;
    public AudioClip StopMoving;
    public AudioClip StartResizing;
    public AudioClip StopResizing;

    void Awake()
    {
        // When a new object is added hook up sounds to it
        InteractableObjectManager.Instance.OnAdded.AddListener(interactable =>
        {
            AddSoundsToEvent(interactable.Hovering, StartHovering, StopHovering);
            AddSoundsToEvent(interactable.Moving, StartMoving, StopMoving);
            AddSoundsToEvent(interactable.Resizing, StartResizing, StopResizing);
        });
    }

    /// <summary>
    /// Play sounds when the specified state object changes
    /// </summary>
    /// <param name="state">State to link events to</param>
    /// <param name="startSound">Sound to play when state starts</param>
    /// <param name="endSound">Sound to play when state ends</param>
    private void AddSoundsToEvent(InteractableObjectState state, AudioClip startSound, AudioClip endSound)
    {
        AudioSource startSource = CreateAudioSource(startSound);
        AudioSource endSource = CreateAudioSource(endSound);

        state.OnStart.AddListener(e => startSource.Play());
        state.OnEnd.AddListener(e => endSource.Play());
    }

    /// <summary>
    /// Create an audio source from the specified audio clip
    /// </summary>
    /// <param name="clip">Clip to create source from</param>
    /// <returns>Audio source</returns>
    private AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.playOnAwake = false;
        return source;
    }
}
