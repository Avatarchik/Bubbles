using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour 
{
	private void OnTriggerEnter(Collider other)
	{
        other.gameObject.SetActive(false);
    }
}
