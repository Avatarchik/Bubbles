using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour 
{
	public float size;
	public float speed = 10f;

	public float calculatedSpeed = 0f;
	public float initialZ;

	public Texture left;

	public Vector3 direction;

	public void Init(float size, float initialZ)
	{
		this.size = size;

		calculatedSpeed = speed / (size / 3f);

		this.initialZ = initialZ;
	    left = TextureManager.Instance.GetTexture(TextureSize.Big, TextureColor.Blue);

        GetComponent<MeshRenderer> ().material.SetTexture("_MainTex", left);
	}

	void Update () 
	{
		transform.Translate( Vector3.down* Time.deltaTime * calculatedSpeed, Space.World);
		var pos = transform.position;
		pos.z = initialZ;
		transform.position = pos;
	
	}
}
