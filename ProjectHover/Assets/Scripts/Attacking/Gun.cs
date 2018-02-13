using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float damage = 10f;
	public float range = 100f;
	public float impactForce = 60f;
	public float fireRate = 15f;
	private float timeToFire = 0f;

	public GameObject wepParent, impactEffect;

	public ParticleSystem muzzleflash;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time >= timeToFire) {
			timeToFire = Time.time + 1f / fireRate;
			Shoot();
		}
	}

	void Shoot() {
		muzzleflash.Play();

		RaycastHit hit;
		if(Physics.Raycast(wepParent.transform.position, wepParent.transform.forward, out hit, range)) {
			Debug.Log(hit.transform.name);
			if (hit.rigidbody != null) {
				hit.rigidbody.AddForce(-hit.normal);
			}
			GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactGO, 2f);
		}
	}
}
