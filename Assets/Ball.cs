using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private void OnMouseDown()
	{
		Destroy (gameObject);
	}
}
