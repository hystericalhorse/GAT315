using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllerCharacter : MonoBehaviour
{
	[SerializeField] float speed = 10;
	[SerializeField] float turnRate = 15;
	[SerializeField] float hitForce = 1;
	[Header("")]
	[SerializeField, Range(1, 5)] float maxJumpHeight = 3f;
	[SerializeField, Range(1, 5)] float doubleJumpHeight = 3f;
	[SerializeField, Range(1, 5)] float hopRateMultiplier = 3f;
	[SerializeField, Range(1, 5)] float fallRateMultiplier = 3f;

	[Header("")]
	[SerializeField] Transform feetTransform;
	[SerializeField] LayerMask groundLayerMask;

	[SerializeField] CharacterController characterController;
	Vector3 velocity = Vector3.zero;
	Vector3 facing = Vector3.zero;
	bool grounded;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		grounded = Physics.CheckSphere(feetTransform.position, 0.2f, groundLayerMask, QueryTriggerInteraction.Ignore);

		// get direction input
		Vector3 direction = Vector3.zero;
		direction.x = Input.GetAxis("Horizontal");
		direction.z = Input.GetAxis("Vertical");

		velocity.x = direction.x * speed;
		velocity.z = direction.z * speed;

		// set velocity
		if (grounded)
		{
			if (velocity.y < 0) velocity.y = 0;

			if (Input.GetButtonDown("Jump"))
			{
				velocity.y += Mathf.Sqrt(maxJumpHeight * -2 * Physics.gravity.y);
				StartCoroutine(ReJump(1));
			}
		}

		float gravityMultiplier = 1;
		if (!grounded && velocity.y < 0) gravityMultiplier = fallRateMultiplier;
		if (!grounded && velocity.y > 0 && !Input.GetButton("Jump")) gravityMultiplier = hopRateMultiplier;

		velocity.y += Physics.gravity.y * Time.deltaTime * gravityMultiplier;

		// move character
		characterController.Move(velocity * Time.deltaTime);

		facing.x = velocity.x;
		facing.z = velocity.z;

		if (facing.magnitude > 0)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facing), Time.deltaTime * turnRate);
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;

		// no rigidbody
		if (body == null || body.isKinematic)
		{
			return;
		}

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3)
		{
			return;
		}

		// Calculate push direction from move direction,
		// we only push objects to the sides never up and down
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// Apply the push
		body.velocity = pushDir * hitForce;
	}

	IEnumerator ReJump(float timer)
	{
		yield return new WaitForSeconds(0.01f);

		while (!grounded)
		{
			if (Input.GetButtonDown("Jump"))
			{
				velocity.y += Mathf.Sqrt(doubleJumpHeight * -2 * Physics.gravity.y);
				break;
			}

			yield return null;
		}
	}
}
