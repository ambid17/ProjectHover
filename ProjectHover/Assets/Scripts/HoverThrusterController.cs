using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverThrusterController : MonoBehaviour {
    public float acceleration;
    public float rotationRate;

    public float turnRotationAngle;
    public float turnRotationSeekSpeed;

    //Reference Variables
    private float rotationVelocity;
    private float groundAngleVelocity;

    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, 3f))
        {
            rigidbody.drag = 1;

            Vector3 forwardForce = transform.forward * acceleration * Input.GetAxis("Vertical");

            forwardForce = forwardForce * Time.deltaTime * rigidbody.mass;

            rigidbody.AddForce(forwardForce);
        }
        else
        {
            rigidbody.drag = 0;
        }

        Vector3 turnTorque = Vector3.up * rotationRate * Input.GetAxis("Horizontal");

        turnTorque = turnTorque * Time.deltaTime * rigidbody.mass;
        rigidbody.AddTorque(turnTorque);


        //fakes the car rotation when strafing
        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -turnRotationAngle, ref rotationVelocity, turnRotationSeekSpeed);
        transform.eulerAngles = newRotation;
    }
}
