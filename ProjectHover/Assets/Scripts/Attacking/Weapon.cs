using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public enum wepTypes {
		Hitscan, Projectile, Spray

	}
	public wepTypes weaponType = wepTypes.Hitscan;

	public float damage, range, impactForce, fireRate, timeToFire;

	public GameObject impactEffect, projectileGO;

	public ParticleSystem muzzleflash;
	
	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButton("Fire1") && Time.time >= timeToFire) {
			timeToFire = Time.time + 1f / fireRate;
			Shoot();
		}
	}

	void Shoot() {
		muzzleflash.Play();

		if (weaponType == wepTypes.Hitscan) {
			Debug.Log("Hitscan!!!!");
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
				Debug.Log(hit.transform.name);
				if (hit.rigidbody != null) {
					hit.rigidbody.AddForce(-hit.normal);
				}
				GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impactGO, 2f);
			}
		}else if(weaponType == wepTypes.Projectile) {
			Debug.Log("Projectile!!!");
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
				Debug.Log(hit.transform.name);
			}
			GameObject projectile = Instantiate(projectileGO, transform.position, Quaternion.LookRotation(transform.forward));
			Destroy(projectile, 10f);
		}
		else if(weaponType == wepTypes.Spray) {

		}
	}
}
