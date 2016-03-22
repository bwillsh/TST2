using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {
	// Use this for initialization
	void Start () {
		if(GetComponent<ParticleSystem>())
		{
			GameObject.Destroy(gameObject, GetComponent<ParticleSystem>().duration + GetComponent<ParticleSystem>().startLifetime);
		}
	}
}
