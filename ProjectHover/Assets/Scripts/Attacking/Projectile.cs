using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField]
	private float speed, damage;

	[SerializeField]
	private bool explosive;

	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision c) {
		if (explosive) {

		}
	}
}
