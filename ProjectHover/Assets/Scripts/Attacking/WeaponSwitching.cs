using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

	public int selectedWep;

	// Use this for initialization
	void Start () {
		SelectWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		int previousSelectedWeapon = selectedWep;

		if(Input.GetAxis("Mouse ScrollWheel") > 0f) {
			if (selectedWep >= transform.childCount - 1)
				selectedWep = 0;
			else
				selectedWep++;

		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
			if (selectedWep <= 0)
				selectedWep = transform.childCount - 1;
			else
				selectedWep--;

		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWep = 0;
		if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) selectedWep = 1;
		if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) selectedWep = 2;

		if (previousSelectedWeapon != selectedWep) SelectWeapon();
	}

	void SelectWeapon() {
		int i = 0;
		foreach (Transform weapon in transform) {
			if(i == selectedWep) 
				weapon.gameObject.SetActive(true);
			else 
				weapon.gameObject.SetActive(false);
			
			i++;
		}
	}
}
