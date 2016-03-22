using UnityEngine;
using System.Collections;

public class ninjaStar : MonoBehaviour {

	public float speed = 15;
	public ParticleSystem explosion;

	// Use this for initialization
	void Start () {
		Vector3 newZ = transform.position;
		newZ.z = Foot.S.transform.position.z;
		transform.position = newZ;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, Wheelchair.S.transform.position, speed * Time.deltaTime);
		transform.Rotate(Vector3.forward, 800 * Time.deltaTime);
	}

	public void SetSpeed(float sp)
	{
		speed = sp;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
	}
		
}
