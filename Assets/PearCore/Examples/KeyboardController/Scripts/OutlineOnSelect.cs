using Pear.Core.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Outline interactables when they're selected
/// </summary>
public class OutlineOnSelect : MonoBehaviour {

    [Tooltip("Outline material")]
    public Material Outline;

	// Use this for initialization
	void Awake () {
        InteractableObjectManager.Instance.OnAdded.AddListener((interactable) =>
        {
            Renderer renderer = interactable.GetComponent<Renderer>();
            Material originalMaterial = renderer.material;

            Material outlineMaterial = new Material(Outline);
            outlineMaterial.color = originalMaterial.color;

            // When an object is selectted apply the outline material
            interactable.Selected.OnStart.AddListener((e) => {
                renderer.materials = new Material[] {
                    originalMaterial,
                    outlineMaterial
                };
            });

            // When an object is deselected, use it's original material
            interactable.Selected.OnEnd.AddListener((e) => interactable.GetComponent<Renderer>().materials = new Material[] { originalMaterial });
        });
	}
}
