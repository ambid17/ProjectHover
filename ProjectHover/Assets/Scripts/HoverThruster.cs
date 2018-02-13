using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverThruster : MonoBehaviour {
    public float thrusterStrength;
    public float thrusterDistance;
    public Transform[] thrusters;
    
	
	void FixedUpdate () {
        RaycastHit hit;
        foreach(Transform thruster in thrusters)
        {
            Vector3 downwardForce;
            float distancePercentage;

            if(Physics.Raycast(thruster.position, -thruster.up, out hit, thrusterDistance))
            {
                //percentage of thrusterDistance away from the ground
                distancePercentage = 1 - (hit.distance / thrusterDistance);

                //calculate thrust
                downwardForce = transform.up * thrusterStrength * distancePercentage;

                //apply force/ thrust
                GetComponent<Rigidbody>().AddForceAtPosition(downwardForce, thruster.position);
            }
        }
	}
}
