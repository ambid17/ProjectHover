using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AveragedHoverThruster : MonoBehaviour {
	[Header("Input Axes")]
	public string roll = "Roll";
	public string yaw = "Mouse X";
	public string thrust = "Thrust";
	public string jump = "Jump"; 

	[Header("Thrust and Torque")]
	public float HoverThrust = 2000f;
	public float ForwardThrust = 400f;
	public float RotationTorque = 10f;

	[Header("Advanced")]
	public float StrafeForceRatio = 1f;
	public float SlopeCompensation = 1.5f;

	public Transform CenterOfMass;
	public Transform[] thrusters;
	public float HoverDistance = 2f;
	public float GroundDistance = 2.5f;
	public LayerMask GroundLayer;

	[Header("Extra Features")]
	public bool ClampTilt = true;
	public float MaxTilt = 45.0f;
	public bool AirControl = true;
	public bool IdleBraking = false;
	public float IdleBrakeFacter = 1.5f;

	public float hoverBoostFactor = 2000;
	public float hovercarHeatFactor = 10;
	public Slider heatSlider;

	Rigidbody hoverbody;

	bool grounded = false;
	float appliedHoverThrust;
	float appliedForwardThrust;

	float currentHeat = 1000;
	float maxHeat = 1000;


	// Use this for initialization
	void Start () {
		hoverbody = GetComponent<Rigidbody>();
	}

	// Update in sync with physics time
	void FixedUpdate () {
		Vector3 input = new Vector3(Input.GetAxis(roll),
			Input.GetAxis(yaw),
			Input.GetAxis(thrust));
		GroundCheck();
		HoverNormal();
		ProcessInput(input);
		ProcessHeat(Input.GetAxis(jump));
		ClampRotation();
		IdleDamp(input);
	}

	void GroundCheck()
	{
		Ray ray = new Ray(CenterOfMass.position, -CenterOfMass.up);
		RaycastHit hitInfo;

		if(Physics.Raycast(ray, out hitInfo, GroundDistance, GroundLayer)){grounded = true;}
		else { grounded = false; }
	}

	void HoverNormal()
	{
		Ray ray = new Ray(CenterOfMass.position, -CenterOfMass.up);
		RaycastHit hitInfo;
		Vector3 upVector;

		if (Physics.Raycast(ray, out hitInfo, HoverDistance, GroundLayer))
		{
			float distance = Vector3.Distance(CenterOfMass.position, hitInfo.point);

			//check slope of ground below and compensate
			if (Vector3.Magnitude(Quaternion.FromToRotation(Vector3.up, hitInfo.normal).eulerAngles) > 0)
			{
				appliedForwardThrust = ForwardThrust * SlopeCompensation;
				appliedHoverThrust = HoverThrust * (SlopeCompensation / 2);
				upVector = transform.up;
			}
			else
			{
				appliedForwardThrust = ForwardThrust;
				appliedHoverThrust = HoverThrust;
				upVector = Vector3.up;
			}

			if(distance < HoverDistance)
			{
				hoverbody.AddForce(upVector * appliedHoverThrust * (1f - distance / HoverDistance));
				hoverbody.rotation = Quaternion.Slerp(hoverbody.rotation, Quaternion.FromToRotation(transform.up, GetAverageNormal(thrusters, GroundDistance)) * hoverbody.rotation, Time.fixedDeltaTime * 3.75f);
			}

		}

	}

	Vector3 GetAverageNormal(Transform[] thrusters, float distance)
	{
		List<Vector3> normals = new List<Vector3>();
		Vector3 sumOfNormals = Vector3.zero;

		foreach (Transform thruster in thrusters)
		{
			Ray ray = new Ray(thruster.position, -thruster.up);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, distance, GroundLayer))
			{
				normals.Add(hitInfo.normal);
			}
		}

		foreach (Vector3 normal in normals)
		{
			sumOfNormals += normal;
		}

		if(normals.Count > 0){
			return sumOfNormals / normals.Count;
		}else{
			return Vector3.zero;
		}

	}

	void ProcessInput(Vector3 input)
	{
		if(grounded || AirControl)
		{
			hoverbody.AddRelativeForce(Vector3.forward * input.z * appliedForwardThrust, ForceMode.Force);

			hoverbody.AddRelativeForce(Vector3.right * input.x * (ForwardThrust * StrafeForceRatio), ForceMode.Force);
			if(input.x > 0){
				hoverbody.AddForceAtPosition(Vector3.up * input.x * RotationTorque, thrusters[0].position, ForceMode.Force);
				hoverbody.AddForceAtPosition(Vector3.up * input.x * RotationTorque, thrusters[2].position, ForceMode.Force);
			}else{
				hoverbody.AddForceAtPosition(Vector3.up * -input.x * RotationTorque, thrusters[1].position, ForceMode.Force);
				hoverbody.AddForceAtPosition(Vector3.up * -input.x * RotationTorque, thrusters[3].position, ForceMode.Force);
			}


			Vector3 appliedTorque = Vector3.up * RotationTorque * input.y;
			appliedTorque = appliedTorque * Time.deltaTime * hoverbody.mass;

			hoverbody.AddTorque(appliedTorque);
			//hoverbody.AddRelativeForce(Vector3.up * RotationTorque * input.y, ForceMode.Force);
		}
	}

	void ProcessHeat(float input){
		if (input > 0) {
			if (currentHeat > 0) {
				currentHeat -= hovercarHeatFactor;
				hoverbody.AddForce(Vector3.up * hoverBoostFactor);
			}
		} else if(currentHeat < maxHeat){
			currentHeat += hovercarHeatFactor;
		}
		heatSlider.normalizedValue = currentHeat / maxHeat;
	}


	void ClampRotation()
	{
		if (ClampTilt)
		{
			Vector3 eulerAngles = hoverbody.rotation.eulerAngles;

			eulerAngles = FixAngles(eulerAngles);

			eulerAngles = new Vector3(Mathf.Clamp(eulerAngles.x, -MaxTilt, MaxTilt), eulerAngles.y, Mathf.Clamp(eulerAngles.z, -MaxTilt, MaxTilt));

			hoverbody.rotation = Quaternion.Euler(eulerAngles);


		}
	}



	Vector3 FixAngles(Vector3 angles)
	{
		if (angles.x > 100) angles.x -= 360;
		if (angles.x < -100) angles.x += 360;

		if (angles.y > 100) angles.y -= 360;
		if (angles.y < -100) angles.y += 360;

		if (angles.z > 100) angles.z -= 360;
		if (angles.z < -100) angles.z += 360;
		return angles;
	}

	void IdleDamp(Vector3 input)
	{
		if(input.x + input.z == 0 && IdleBraking)
		{
			hoverbody.velocity = Vector3.Lerp(hoverbody.velocity, Vector3.zero, Time.fixedDeltaTime * IdleBrakeFacter);
		}
	}
}
