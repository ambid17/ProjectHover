using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackObject : MonoBehaviour {
    public Transform target;
    public float distanceUp;
    public float distanceBack;
    public float minimumHeight;
    public float smoothTime = 0.18f;

    private Vector3 positionVelocity;

	void FixedUpdate () {
        Vector3 newPosition = target.position + (target.forward * distanceBack);
        newPosition.y = Mathf.Max(newPosition.y + distanceUp, minimumHeight);

        //dampens the camera following the target (similar to lerp)
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref positionVelocity, smoothTime);

        Vector3 focalPoint = target.position + (target.forward * 5);
        transform.LookAt(focalPoint);
	}
}
