using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	[Header("Movement")]
	[SerializeField]
	float speed = 4f;
	[SerializeField]
	float SpeedMultiplier = 2f; // increase speed multiplier while sprinting
	float tempSpeed; 
	[SerializeField]
    float jumpSpeed = 8.0F;
	[SerializeField]
    float gravity = 20.0F;
	[SerializeField]
	[Header("Head Vision")]
	GameObject head;
	float rotationX;
	float rotationY;
	float moveZ;
	float moveX;
    Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	
	void Awake()
    {
		controller = GetComponent<CharacterController>();
	}

	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

    void Update() {
        Move();
    }

	void Move()
    {
		tempSpeed = speed; //this rests our speed back to natural speed

		if (controller.isGrounded) {
			if(Input.GetKey(KeyCode.LeftShift)) 
			{
				tempSpeed *= SpeedMultiplier; //this increases our speed by the multiplier
			}
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= tempSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
	}

	public float pushPower = 2.0F;
    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        
        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }



}
