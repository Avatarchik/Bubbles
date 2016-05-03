using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour 
{
	public float size;
	public float speed = 10f;

	public float calculatedSpeed = 0f;
	public float initialZ;

	public Texture2D left;

	public Vector3 direction;

	public void Init(float size, float initialZ)
	{
		this.size = size;

		calculatedSpeed = speed / (size / 3f);

		this.initialZ = initialZ;

		var width = 64;
		var height = 64;

		left = new Texture2D (width, height,TextureFormat.ARGB32,false);

		left.filterMode = FilterMode.Trilinear;
		left.anisoLevel = 20;

		//left.wrapMode = TextureWrapMode.Clamp;
		left = Circle (left, 32, -32 , 30,Color.black);



		left.Apply ();



		var s = Sprite.Create (left,
			       new Rect (0, 0, left.width, left.height),
			       new Vector2 (0.5f, 0.5f),1f);

		GetComponent<SpriteRenderer> ().sprite = s;
	}

	private static float Smooth (float t) {
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	public Texture2D Circle(Texture2D tex, int cx, int cy, int r, Color col)
	{
		int x, y, px, nx, py, ny, d;

		for (x = 0; x <= r; x++)
		{
			d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
			col.a = Smooth (col.a);
			for (y = 0; y <= d; y++)
			{
				px = cx + x;
				nx = cx - x;

				py = cy + y;
				ny = cy - y;

				tex.SetPixel(px, py, col);
				tex.SetPixel(nx, py, col);

				tex.SetPixel(px, ny, col);
				tex.SetPixel(nx, ny, col);

			}
		}

		for (int i = 0; i < tex.width; i++) 
		{
			for (int j = 0; j < tex.height; j++) 
			{
				if (left.GetPixel (i, j).a != 1f) 
				{
					left.SetPixel (i, j, Color.clear);
				} 
				else 
				{
					left.SetPixel (i, j, Color.Lerp(Color.white,Color.black,(float)(j + 10f)/tex.height));
				}
			}
		}

		return tex;
	}

	void Update () 
	{
		transform.Translate( Vector3.down* Time.deltaTime * calculatedSpeed, Space.World);
		var pos = transform.position;
		pos.z = initialZ;
		transform.position = pos;
	
	}
}
