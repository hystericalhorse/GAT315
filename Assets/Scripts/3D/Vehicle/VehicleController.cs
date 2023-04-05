using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static VehicleController;

public class VehicleController : MonoBehaviour
{
	[System.Serializable]
	public class Wheel
	{
		public WheelCollider collider;
		public Transform transform;
	}

	[System.Serializable]
	public class Axle
	{
		public Wheel leftWheel;
		public Wheel rightWheel;

		public bool isMotor;
		public bool isSteering;
	}

	[SerializeField] Axle[] axles;
	[SerializeField] float maxMotorTorque;
	[SerializeField] float maxSteeringAngle;

	public void FixedUpdate()
	{
		float motor = maxMotorTorque * Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

		foreach (Axle axle in axles)
		{
			if (axle.isSteering)
			{
				axle.leftWheel.collider.steerAngle = steering;
				axle.rightWheel.collider.steerAngle = steering;
			}
			if (axle.isMotor)
			{
				axle.leftWheel.collider.motorTorque = motor;
				axle.rightWheel.collider.motorTorque = motor;
			}

			WheelTransform(axle.leftWheel);
			WheelTransform(axle.rightWheel);
		}
	}

	public void WheelTransform(Wheel wheel)
	{
		wheel.collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
		position.y = wheel.transform.position.y;

		wheel.transform.position = position;
		wheel.transform.rotation = rotation;
	}
}