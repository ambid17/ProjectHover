using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMotor : MonoBehaviour {

    public float speed = 90f;
    public float turnspeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;

    private float powerInput;
    private float turnInput;
    private Rigidbody carRigidBody;
    
    void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        powerInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 boostForce = Vector3.up * hoverForce;
            carRigidBody.AddForce(boostForce * 10);
        }
	}

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            carRigidBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        carRigidBody.AddRelativeForce(0f, 0f, powerInput * speed);

        carRigidBody.AddRelativeTorque(0f, turnInput * turnspeed, 0f);  
    }
}
