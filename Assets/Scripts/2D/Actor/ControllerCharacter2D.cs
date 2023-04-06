using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControllerCharacter2D : MonoBehaviour
{
	[SerializeField] float forceMultiply = 10;
	[Header("")]
	[SerializeField, Range(1, 5)] float maxJumpHeight = 3f;
	[SerializeField, Range(1, 5)] float doubleJumpHeight = 3f;
	[SerializeField, Range(1, 5)] float hopRateMultiplier = 3f;
	[SerializeField, Range(1, 5)] float fallRateMultiplier = 3f;

	[Header("")]
	[SerializeField] Transform feetTransform;
	[SerializeField] LayerMask groundLayerMask;

	[SerializeField] Rigidbody2D rb;
	Vector3 velocity = Vector3.zero;
	Vector3 facing = Vector3.zero;
	bool grounded;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		grounded = Physics2D.OverlapCircle(feetTransform.position, 0.2f, groundLayerMask);

		// get direction input
		Vector3 direction = Vector3.zero;
		direction.x = Input.GetAxis("Horizontal");

		velocity.x = direction.x * forceMultiply;

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

		velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

		// move character
		rb.AddForce(velocity);
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
