using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAudio : MonoBehaviour {

    public AudioSource jetSound;
    private float jetPitch;
    private const float LowPitch = .1f;
    private const float HighPitch = 2.0f;
    private const float SpeedToRevs = .01f;
    Vector3 myVelocity;
    Rigidbody carRigidBody;

    private void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        myVelocity = carRigidBody.velocity;
        float forwardSpeed = transform.InverseTransformDirection(carRigidBody.velocity).z;
        float engineRevs = Mathf.Abs(forwardSpeed) * SpeedToRevs;
        jetSound.pitch = Mathf.Clamp(engineRevs, LowPitch, HighPitch);
	}
}
