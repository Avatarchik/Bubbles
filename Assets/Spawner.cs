using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Transform spawnArea;
	public GameObject prefab;
	public float timeSpawn = 0.5f;

	private float timer = 0f;

	private float z = 0f;

	void Update () 
	{
		timer += Time.deltaTime;
		
		if (timer >= timeSpawn) 
		{
			Spawn ();
			timer = 0f;
		}
	
	}

	private void Spawn()
	{
		var size = Random.Range (1f, 3f);

		var go = GameObject.Instantiate (prefab);

		go.transform.localScale = Vector3.one * size;

		var spawnSize = spawnArea.localScale.x / 2f;

		var xRandom = Random.Range (spawnArea.position.x - spawnSize + size * 64f / 2f, spawnArea.position.x + spawnSize - size * 64f / 2f);

		var pos = new Vector3(xRandom,spawnArea.position.y,0f);

		go.transform.position = pos;
		go.GetComponent<Mover> ().Init (size, z--);

	}
}
