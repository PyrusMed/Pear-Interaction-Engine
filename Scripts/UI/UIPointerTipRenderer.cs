using Pear.InteractionEngine.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIPointer))]
public class UIPointerTipRenderer : MonoBehaviour {

	[Tooltip("Width of the line")]
	public float LineWidth = 0.005f;

	[Tooltip("Length of pointer")]
	public float LineLength = 0.05f;

	[Tooltip("Radius of the sphere")]
	public float SphereScale = 0.1f;

	[Tooltip("Line material")]
	public Material LineMaterial;

	[Tooltip("Sphere Material")]
	public Material SphereMaterial;

	// Pointer
	private UIPointer _pointer;

	// Line renderer
	private LineRenderer _line;

	private GameObject _sphere;

	// Use this for initialization
	void Start () {
		_pointer = GetComponent<UIPointer>();

		_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_sphere.transform.localScale = new Vector3(SphereScale, SphereScale, SphereScale);
		_sphere.GetComponent<Renderer>().material = SphereMaterial;

		_line = gameObject.AddComponent<LineRenderer>();
		_line.startWidth = _line.endWidth = LineWidth;

		_line.material = LineMaterial;
		_pointer.HoverChangedEvent += (oldHovered, newHovered) =>
		{
			_line.enabled = newHovered != null;
			_sphere.SetActive(_line.enabled);
		};
	}
	
	// Update is called once per frame
	void Update () {
		if (_pointer.IsActive && _line.enabled && _pointer.HoveredElement != null)
		{
			Vector3 end = _pointer.pointerEventData.pointerCurrentRaycast.worldPosition;
			Vector3 start = end - _pointer.GetOriginForward() * LineLength;

			_line.SetPositions(new Vector3[]
			{
					start,
					end,
			});

			_sphere.transform.position = _pointer.pointerEventData.pointerCurrentRaycast.worldPosition;
		}
	}

	private void OnEnable()
	{
		_line.enabled = true;
		_sphere.SetActive(true);
	}

	private void OnDisable()
	{
		_line.enabled = false;
		_sphere.SetActive(false);
	}
}
