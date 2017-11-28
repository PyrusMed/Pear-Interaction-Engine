using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the kayboard with the arrow keys
/// </summary>
public class KeyboardMovement : MonoBehaviour {

	// Movement Speed
	public float Speed = 1;

	void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(Speed * Time.deltaTime, 0, 0));
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-Speed * Time.deltaTime, 0, 0));
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0, -Speed * Time.deltaTime, 0));
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));
		}
	}
}
